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

    private void Start()
    {
        if (maxTime <= 0f) maxTime = 180f;
        time = maxTime;
        SetMaxTime(maxTime);
    }

    private void Update()
    {
        time = Mathf.Max(0f, time - Time.deltaTime);
        SetTime(time);
        if (timetext) timetext.text = FormatTime(time);
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
