using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform panTarget;
    [SerializeField] private EggCamera eggCamera;
    [SerializeField] private EggRolling eggRolling;
    [SerializeField] private MouseGrabber mouseGrabber;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float panDistance = 7f;
    [SerializeField] private float panHeight = 3f;
    [SerializeField] private float moveToPanTime = 2f;
    [SerializeField] private float panHoldTime = 1f;
    [SerializeField] private float moveBackTime = 1.4f;

    private IEnumerator Start()
    {
        if (cameraTransform == null && Camera.main) cameraTransform = Camera.main.transform;
        if (eggCamera == null) eggCamera = GetComponent<EggCamera>();
        if (eggRolling == null) eggRolling = GetComponent<EggRolling>();
        if (mouseGrabber == null) mouseGrabber = GetComponent<MouseGrabber>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (cameraTransform == null || panTarget == null) yield break;

        SetControl(false);

        Pose eggView = GetEggView();
        SetCamera(eggView);

        Pose panView = GetPanView();
        yield return MoveCamera(eggView, panView, moveToPanTime);
        yield return new WaitForSeconds(panHoldTime);
        yield return MoveCamera(panView, GetEggView(), moveBackTime);

        SetControl(true);
    }

    private void SetControl(bool enabled)
    {
        if (eggCamera) eggCamera.enabled = enabled;
        if (eggRolling) eggRolling.enabled = enabled;
        if (mouseGrabber) mouseGrabber.enabled = enabled;
        if (playerInput) playerInput.enabled = enabled;
    }

    private IEnumerator MoveCamera(Pose from, Pose to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
            SetCamera(new Pose(Vector3.Lerp(from.position, to.position, t), Quaternion.Slerp(from.rotation, to.rotation, t)));
            yield return null;
        }

        SetCamera(to);
    }

    private Pose GetEggView()
    {
        Vector3 focus = transform.position + Vector3.up;
        Vector3 direction = cameraTransform.position - focus;
        if (direction.sqrMagnitude < 0.01f) direction = new Vector3(0f, 0.35f, -1f);

        Vector3 position = focus + direction.normalized * 6f;
        return LookAt(position, focus);
    }

    private Pose GetPanView()
    {
        Vector3 focus = panTarget.position + Vector3.up * 0.5f;
        Vector3 direction = transform.position - panTarget.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
        if (direction.sqrMagnitude < 0.01f) direction = Vector3.back;

        Vector3 position = focus + direction.normalized * panDistance + Vector3.up * panHeight;
        return LookAt(position, focus);
    }

    private Pose LookAt(Vector3 position, Vector3 focus)
    {
        return new Pose(position, Quaternion.LookRotation(focus - position, Vector3.up));
    }

    private void SetCamera(Pose pose)
    {
        cameraTransform.SetPositionAndRotation(pose.position, pose.rotation);
    }
}
