using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EggHealth : MonoBehaviour
{
    public Slider healthslider;
    public Image healthfill;
    public Gradient healthgradient;
    public Rigidbody player;
    public float health = 100f;
    public GroundSensor groundSensor;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject deathPanel;
    public GameObject splatObject;

    [SerializeField] private float safeFallHeight = 1.2f;
    [SerializeField] private float damagePerMeter = 22f;
    [SerializeField] private float impactSpeedThreshold = 10f;
    [SerializeField] private float impactDamage = 20f;
    [SerializeField] private float cushionMultiplier = 0.2f;

    private float peakY;
    private Vector3 lastAirVelocity;
    private bool wasGrounded = true;
    private bool dead;
    private bool paused;

    private void Awake()
    {
        if (player == null) player = GetComponent<Rigidbody>();
        if (groundSensor == null) groundSensor = GetComponent<GroundSensor>();

        if (pausePanel) pausePanel.SetActive(false);
        if (pauseButton) pauseButton.SetActive(true);
        if (deathPanel) deathPanel.SetActive(false);
        if (splatObject) splatObject.SetActive(false);
        SetMaxHealth(health);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (!dead && Keyboard.current.escapeKey.wasPressedThisFrame) SetPaused(!paused);
        if (dead && Keyboard.current.rKey.wasPressedThisFrame) Respawn();
    }

    private void FixedUpdate()
    {
        if (dead || player == null || groundSensor == null) return;

        bool grounded = groundSensor.IsGrounded(out RaycastHit hit);
        if (!grounded)
        {
            if (wasGrounded)
            {
                peakY = transform.position.y;
            }

            peakY = Mathf.Max(peakY, transform.position.y);
            lastAirVelocity = player.linearVelocity;
        }

        if (grounded && !wasGrounded) ApplyFallDamage(hit);
        wasGrounded = grounded;
        SetHealth(health);
    }

    private void LateUpdate()
    {
        if (!dead || Camera.main == null) return;

        Vector3 focus = splatObject && splatObject.activeSelf ? splatObject.transform.position : transform.position;
        focus += Vector3.up * 0.35f;
        float angle = Time.unscaledTime * 0.65f;
        Vector3 offset = new Vector3(Mathf.Cos(angle) * 4f, 2.2f, Mathf.Sin(angle) * 4f);
        Vector3 position = focus + offset;
        Camera.main.transform.SetPositionAndRotation(position, Quaternion.LookRotation(focus - position, Vector3.up));
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;

        health = Mathf.Max(0f, health - damage);
        SetHealth(health);

        if (health > 0f) return;
        dead = true;
        Die();
    }

    public void SetMaxHealth(float value)
    {
        if (healthslider == null) return;

        healthslider.maxValue = value;
        healthslider.value = health;
        if (healthfill) healthfill.color = healthgradient.Evaluate(1f);
    }

    public void SetHealth(float value)
    {
        if (healthslider == null) return;

        healthslider.value = value;
        if (healthfill) healthfill.color = healthgradient.Evaluate(value / healthslider.maxValue);
    }

    public void Pause()
    {
        if (!dead) SetPaused(true);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ApplyFallDamage(RaycastHit hit)
    {
        float fallHeight = Mathf.Max(0f, peakY - transform.position.y - safeFallHeight);
        float damage = fallHeight * damagePerMeter;

        float impactSpeed = Mathf.Max(0f, Vector3.Dot(lastAirVelocity, -hit.normal));
        if (impactSpeed > impactSpeedThreshold) damage += impactDamage;
        if (damage <= 0f) return;

        if (hit.collider.CompareTag("Cushion")) damage *= cushionMultiplier;
        TakeDamage(damage);
    }

    private void Die()
    {
        SetPaused(false);
        if (deathPanel) deathPanel.SetActive(true);
        CrackEgg();
    }

    private void SetPaused(bool value)
    {
        paused = value;
        Time.timeScale = paused ? 0f : 1f;
        if (pausePanel) pausePanel.SetActive(paused);
        if (pauseButton) pauseButton.SetActive(!paused && !dead);
    }

    private void CrackEgg()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) renderer.enabled = false;

        if (TryGetComponent(out EggRolling movement)) movement.enabled = false;
        if (TryGetComponent(out MouseGrabber grabber)) grabber.enabled = false;
        if (TryGetComponent(out EggCamera eggCamera)) eggCamera.enabled = false;
        if (TryGetComponent(out Collider collider)) collider.enabled = false;
        if (player)
        {
            player.linearVelocity = Vector3.zero;
            player.angularVelocity = Vector3.zero;
            player.isKinematic = true;
        }

        if (splatObject == null) return;

        Vector3 position = transform.position;
        Quaternion rotation = Quaternion.identity;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
        {
            position = hit.point + Vector3.up * 0.02f;
            rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        splatObject.transform.SetPositionAndRotation(position, rotation);
        splatObject.SetActive(true);
    }
}
