using UnityEngine;

public class Billboard : MonoBehaviour
{
public float rotationSpeed = 5f;

void LateUpdate()
{
    if (Camera.main == null) return;

    Vector3 direction = Camera.main.transform.position - transform.position;
    direction.y = 0f;
    Quaternion targetRotation = Quaternion.LookRotation(-direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
}

}


