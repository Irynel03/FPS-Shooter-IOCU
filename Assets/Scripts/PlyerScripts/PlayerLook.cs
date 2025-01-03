using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float xRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        // Ensure the cursor remains locked during gameplay
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= mouseY * ySensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX * xSensitivity * Time.deltaTime);
    }
}
