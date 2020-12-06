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
    public int WoodFromCasino = 0;
    public List<GameObject> buildings;

    // Events //
    private bool didStockpileEvent = false;
    private bool didMineEvent = false;
    private bool didCasinoEvent = false;

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
    public void addWood(int woodToAdd, bool isFromMine = false, bool isFromTree = false, bool isFromCasino=false)
    {
        Wood += woodToAdd;
        CheckBuilding();
        Debug.Log("Wood: " + Wood.ToString());

        // Wood Event 1: First worker at 20 wood + stockpile building
        if (Wood >= 20 && !didStockpileEvent) {
            if(buildings.Find(x => x.GetComponent<Storage>() != null) != null) {
                postStockpileEvent();
            }
        }

        // Wood Event 2: Unlock robots at 200 wood + mine building
        if(Wood > 200 && !didMineEvent) {
            if(buildings.Find(x => x.GetComponent<Mine>() != null) != null) {
                postMineEvent();
            }
        }

        if (isFromMine)
        {
            WoodFromMine += woodToAdd;
        }
        if (isFromTree)
        {
            WoodFromTreeFromNpc += woodToAdd;
        }
        if (isFromCasino)
        {
            WoodFromCasino += woodToAdd;
        }
        EventCheck();

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
        EventCheck();
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
        EventCheck();
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

    private void EventCheck()
    {
        if(Wood > 20 && !didStockpileEvent)
        {
            if(buildings.Find(x => x.GetComponent<Storage>() != null) != null)
            {
                // put event data here
                didStockpileEvent = true;
            }
        }
        if(Wood > 200 && !didMineEvent)
        {
            didMineEvent = true;
        }
        if(WoodFromCasino <= -400 && !didCasinoEvent)
        {
            // do casino losing event
            didCasinoEvent = true;
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
            17, //Tree1_Cut
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
                    0, //Fence
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
                    18, //WoodPile
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
