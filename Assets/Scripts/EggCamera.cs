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
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        if (cameraTransform==null) return;

        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            yaw += lookInput.x * mouseSensitivity;
            pitch -= lookInput.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        lookInput = Vector2.zero;

        
        Vector3 focusPoint = transform.position + Vector3.up * targetHeight; // camera slightly above egg
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 cameraPosition = focusPoint - cameraRotation * Vector3.forward * distance; // camera slightly behind where its looking at

        cameraTransform.SetPositionAndRotation(cameraPosition, cameraRotation);
    }
}
