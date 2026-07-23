using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Slider timerslider;
    public Image timerfill;
    //   public Gradient healthgradient;

    public float time;
    public float maxTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxTime = 180;
        time = maxTime;
        setMaxTime(time);
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        setMaxTime(time);
    }

    public void setMaxTime(float time)
    {
        timerslider.maxValue = 180;//update 180 to maxtime manually
        timerslider.value = time;
        //healthfill.color = healthgradient.Evaluate(1f);
    }

    public void setTime(float time)
    {
        timerslider.value = time;
      //  healthfill.color = healthgradient.Evaluate(time / maxTime);
    }




}
