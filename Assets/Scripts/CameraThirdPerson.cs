using UnityEngine;

public class CameraThirdPerson : MonoBehaviour
{
    public Transform target;
    public float distance = 3.0f;
    public float height = 1.5f;
    public float rotationSpeed = 5f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target tidak diatur! Tetapkan pemain sebagai target.");
            return;
        }

        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY = Mathf.Clamp(rotationY, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        Vector3 position = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
