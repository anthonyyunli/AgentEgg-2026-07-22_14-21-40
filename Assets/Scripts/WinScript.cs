using UnityEngine;

public class WinScript : MonoBehaviour
{

    public Transform gate;

    [SerializeField] private LayerMask PlayerMask;
    [SerializeField] private LayerMask BoilEggMask;

    public GameObject winscreen;

    public EggHealth egghealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        isWin();

    }

    public void isWin()
    {
        Vector3 position = gate.position;

         if (Physics.CheckSphere(position,5f,PlayerMask,QueryTriggerInteraction.Ignore) == true && Physics.CheckSphere(position, 5f, BoilEggMask, QueryTriggerInteraction.Ignore) == true)
        {

            Debug.Log("Game won");
            winscreen.SetActive(true);
            egghealth.Pause();

        }


    }


}
