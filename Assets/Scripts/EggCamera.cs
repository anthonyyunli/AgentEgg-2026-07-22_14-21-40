using UnityEngine;
using UnityEngine.InputSystem;

public class EggCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Camera Position")]
    [SerializeField] private float distance = 6f;
    [SerializeField] private float targetHeight = 1f;

    [Header("Mouse")]
    [SerializeField] private float mouseSensitivity = 0.12f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 65f;

    private Vector2 lookInput;

    private float yaw;
    private float pitch = 20f;

    private void Start()
    {
        if (cameraTransform) yaw = cameraTransform.eulerAngles.y;
        LockCursor();
    }

    public void OnLook(InputValue value)
    {
        Debug.Log(value.Get<Vector2>());
        lookInput = value.Get<Vector2>();
    }

    private void Update()
    {
        if (Keyboard.current!=null && Keyboard.current.escapeKey.wasPressedThisFrame) UnlockCursor();
        if (Mouse.current!=null && Mouse.current.leftButton.wasPressedThisFrame) LockCursor();

    }

    private void LateUpdate()
    {
        if (cameraTransform==null) return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += lookInput.x * mouseSensitivity;
            pitch -= lookInput.y * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        lookInput = Vector2.zero;

        // camera slightly above egg
        Vector3 focusPoint = transform.position + Vector3.up * targetHeight;

        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        
        // camera slightly behind where its looking at
        Vector3 cameraPosition = focusPoint - cameraRotation * Vector3.forward * distance;

        cameraTransform.SetPositionAndRotation(cameraPosition, cameraRotation);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
