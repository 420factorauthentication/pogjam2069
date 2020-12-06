using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;  //UnityEngine.Events.UnityAction used for delegates to button onClicks

public class WoodManager : MonoBehaviour
{
    public static WoodManager Wmanager;

    // Resources //
    public int Wood;
    public int WoodFromMine = 0;
    public int WoodFromTreeFromNpc = 0;
    public List<GameObject> buildings;

    // Events //
    private bool didStockpileEvent = false;
    private bool didMineEvent = false;

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

    ////////////////////////////////////////////////////////////////////////////
    // addWood()                                                              //
    //                                                                        //
    // Adds wood, checks events, and checks if enough wood to build anything. //
    ////////////////////////////////////////////////////////////////////////////
    public void addWood(int woodToAdd, bool isFromMine = false, bool isFromTree = false)
    {
        Wood += woodToAdd;
        CheckBuilding();
        Debug.Log("Wood: " + Wood.ToString());

        // Wood Event 1: First worker at 20 wood + stockpile building
        if (Wood >= 20 && !didStockpileEvent) {
            if(GameObject.Find("Storage").GetComponent<Storage>().IsBuilt) {
                postStockpileEvent();
            }
        }

        // Wood Event 2: Unlock robots at 200 wood + mine building
        if(Wood > 200 && !didMineEvent) {
            if(GameObject.Find("Mine").GetComponent<Mine>().IsBuilt) {
                postMineEvent();
            }
        }
    }

    /////////////////////////////////////////////////////////////////
    // SubtractWood()                                              //
    //                                                             //
    // Subtracts wood and checks if enough wood to build anything. //
    /////////////////////////////////////////////////////////////////
    public void SubtractWood(int woodToSubtrack)
    {
        if (woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
        else
        {
            Wood = 0;
        }
        CheckBuilding();
    }

    //////////////////////////////////////////////////////////////////////////////////
    // PurchaseWithWood()                                                           //
    //                                                                              //
    // Spends wood, builds a building, and checks if enough wood to build anything. //
    //////////////////////////////////////////////////////////////////////////////////
    public void PurchaseWithWood(int woodToSubtrack)
    {
        if (woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
        CheckBuilding();
    }

    ///////////////////////////////////////////////////////////////
    // CheckBuilding()                                           //
    //                                                           //
    // Checks if player has enough wood to afford any buildings. //
    ///////////////////////////////////////////////////////////////
    private void CheckBuilding()
    {
        foreach (GameObject build in buildings)
        {
            build.GetComponent<IBuilding>().CheckCanBuild(Wood);
        }
    }

    ////////////////////////////////////////////////////////////////
    // Wood Event 1: First worker at 20 wood + stockpile building //
    ////////////////////////////////////////////////////////////////
    private void postStockpileEvent() {
        didStockpileEvent = true;
        Surprise surprise1 = new Surprise(
            "Capitalism ho!",
            "Your wood stockpiles are bursting with inventory. Maybe someone " +
                "out there is willing to buy some. What surprises does the free " +
                "market have in store?",
            30,
            3, //Blacksmith
            true,

            "",
            "",
            null,

            "Find a buyer",
            "Sell some wood for money?",
            // Choice 1: Gain first worker.
            new UnityAction(delegate () {
                Surprise surprise2 = new Surprise(
                    "",
                    "Searching for buyers, you run into Todd Howard. At first, " +
                        "he tries to rob you for Fallout 4 Microtransaction " +
                        "Assets. Seeing your stockpile, he realizes he would " +
                        "make more money working for you, and he mugs you for " +
                        "a job.",
                    24,
                    15, //Player
                    false,

                    "Continue",
                    "+1 Worker",
                    null //new UnityAction(delegate () { AddWorker(1); })    ADD FUNCTION TO ADD +1 WORKER
                );
                SurpriseManager.Smanager.PostSurprise(surprise2, true);
            }),

            "I wood not sell would!",
            "Gain some vintage aged wood?",
            // Choice 2: Bank now requires solving a math problem to use it.
            new UnityAction(delegate () {
                Surprise surprise3 = new Surprise(
                    "",
                    "Gabe Newell releases Half Life Tree, causing Tesla's " +
                        "stock price to skyrocket. Your stock pile becomes " +
                        "worthless, and your credit score plummets.",
                    30,
                    1, //Line1
                    false,

                    "Oof",
                    "You now have to solve a math problem every time you use " +
                        "the Bank.",
                    null //new UnityAction(delegate () { EnableBankCaptcha; })    ADD FUNCTION TO ENABLE BANK CAPTCHA
                );
                SurpriseManager.Smanager.PostSurprise(surprise3, true);
            })
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, false);
    }

    /////////////////////////////////////////////////////////////
    // Wood Event 2: Unlock robots at 200 wood + mine building //
    /////////////////////////////////////////////////////////////
    private void postMineEvent() {
        
    }
}
