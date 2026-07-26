using UnityEngine;

public class ResetEnd : MonoBehaviour
{


    public EggHealth egghealth;
    public Timer timer;

    public Transform player;
    public Transform startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {

        Debug.Log("Reset Called");

        // egghealth.setMaxHealth(180);

        // timer.setMaxTime(180);

        player.position = startPosition.position;


            
            }

    public void Lose()
    {
        Debug.Log("Lose Called");
    }

    public void Win()
    {
        Debug.Log("Win Called");
    }
}
