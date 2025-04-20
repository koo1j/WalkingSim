using System.Collections;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string[] dialogueLines;
    public float dialogueDistance = 3f;
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public GameObject talkPrompt;
    //this is for the interaction when u press e

    public AudioSource beepAudioSource;
    //for the beeping sound when it types out
    public float typeSpeed = 0.03f;

    private int currentLine = 0;
    private Transform player;
    private bool playerInRange = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        player = Camera.main.transform;
        dialogueCanvas.SetActive(false);
        if (talkPrompt != null)
            talkPrompt.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        playerInRange = distance <= dialogueDistance;

        dialogueCanvas.transform.rotation = Quaternion.LookRotation(dialogueCanvas.transform.position - player.position);

        if (talkPrompt != null)
            talkPrompt.SetActive(playerInRange && !dialogueCanvas.activeSelf);

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueCanvas.activeSelf)
            {
                currentLine = 0;
                dialogueCanvas.SetActive(true);
                StartTyping(dialogueLines[currentLine]);
            }
            else if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                currentLine++;
                if (currentLine < dialogueLines.Length)
                {
                    StartTyping(dialogueLines[currentLine]);
                }
                else
                {
                    dialogueCanvas.SetActive(false);
                }
            }
        }
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            if (beepAudioSource != null && letter != ' ')
            {
                beepAudioSource.pitch = Random.Range(0.9f, 1.1f);
                beepAudioSource.PlayOneShot(beepAudioSource.clip);
            }



            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueLines[currentLine];
        isTyping = false;
        //a typing skip function
    }
}
