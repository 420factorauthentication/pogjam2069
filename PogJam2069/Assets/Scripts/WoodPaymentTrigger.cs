using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WoodPaymentTrigger : MonoBehaviour
{
    public UnityEvent WoodTriggeredEvent;
    public int woodCost;
    bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange == true)
        {
            //enable world cost canvas ui object


            if (WoodManager.Wmanager.Wood >= woodCost)
            {
                //Change to Able to Buy UI Chart
                if (Input.GetKeyDown(KeyCode.F))
                {

                    WoodManager.Wmanager.SubtractWood(woodCost);

                    WoodTriggeredEvent.Invoke();
                }
            }

            

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;


        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
