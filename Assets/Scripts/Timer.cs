using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timerslider;
    public Image timerfill;
    public float time = 180f;
    public float maxTime = 180f;
    public TextMeshProUGUI timetext;
    public GameObject BoilEgg;
    public GameObject SplatBoilEgg;
  //  public GameObject player;
  //  public Camera MainCamera;

    public GameObject LoseScreen;

    public EggHealth egghealth;


    private void Start()
    {
        if (maxTime <= 0f) maxTime = 600f;
        time = maxTime;
        SetMaxTime(maxTime);
    }

    private void Update()
    {
        time = Mathf.Max(0f, time - Time.deltaTime);
        if (time > 0)
        {
            SetTime(time);
            if (timetext) timetext.text = FormatTime(time-1);
        }
        else if (time <= 0)
        {
            Debug.Log("Tiem out");

            LoseScreen.SetActive(true);
            /*
            foreach (Renderer renderer in player.GetComponentsInChildren<Renderer>()) renderer.enabled = false;

            if (player.transform.TryGetComponent(out EggRolling movement)) movement.enabled = false;
            if (player.transform.TryGetComponent(out MouseGrabber grabber)) grabber.enabled = false;
            if (player.transform.TryGetComponent(out EggCamera eggCamera)) eggCamera.enabled = false;
            if (player.transform.TryGetComponent(out Collider collider)) collider.enabled = false;
            if (player)
            {
                player.linearVelocity = Vector3.zero;
                player.angularVelocity = Vector3.zero;
                player.isKinematic = true;
            }


            Vector3 position = BoilEgg.transform.position;
            Quaternion rotation = Quaternion.identity;
            if (Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
            {
                position = hit.point + Vector3.up * 0.02f;
                rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
            */

          //  BoilEgg.SetActive(false);

           // SplatBoilEgg.transform.position = BoilEgg.transform.position;


            // EggCamera =  new Vector3[Mathf.RoundToInt(BoilEgg.transform.position.x), Mathf.RoundToInt(BoilEgg.transform.position.y+5), Mathf.RoundToInt(BoilEgg.transform.position.z)];

            //  MainCamera.transform.position = EggCamera;

           // MainCamera.transform.position = SplatBoilEgg.transform.position;

            //  MainCamera.transform.position.y += 5;
         //   MainCamera.transform.Translate(Vector3[0, 5, 0]);


            egghealth.Pause();
        }
    }

    public void SetMaxTime(float value)
    {
        if (timerslider == null) return;

        timerslider.maxValue = value;
        timerslider.value = value;
    }

    public void SetTime(float value)
    {
        if (timerslider) timerslider.value = value;
    }

    private string FormatTime(float secondsLeft)
    {
        int seconds = Mathf.CeilToInt(secondsLeft);
        return $"{seconds / 60:00}:{seconds % 60:00}";
    }
}
