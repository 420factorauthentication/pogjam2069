using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;  //UnityEngine.Events.UnityAction used for delegates to button onClicks

public class WoodManager : MonoBehaviour
{
    public static WoodManager Wmanager;

    // Resources //
    public int Wood;

    // Events //
    private bool didWoodSurprise1 = false;


    // Start is called before the first frame update
    void Awake()
    {
        if (Wmanager != null && Wmanager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Wmanager = this;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addWood(int woodToAdd)
    {
        Wood += woodToAdd;

        // Wood Surprise 1
        if (Wood >= 1 && !didWoodSurprise1)
        {
            didWoodSurprise1 = true;
            SurpriseManager.Smanager.PostSurprise(new Surprise(
                "Found a lootbox?",
                "After chopping that last tree, you found a wooden loot box " +
                  "buried in the soil behind it! What surprises could it hold?",
                10,
                true,

                "",
                "",
                null,

                "Open the lootbox!",
                "Gain what's inside?",
                new UnityAction(delegate() {print("YOU GOT PRANKD");}),

                "Sell the lootbox for wood",
                "+1 Wood",
                new UnityAction(delegate() {addWood(1);})
            ));
        }
    }

    public void SubtractWood(int woodToSubtrack)
    {
        if(woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
    }


    //////////////////////////////
    // Event Function Delegates //
    //////////////////////////////


}
