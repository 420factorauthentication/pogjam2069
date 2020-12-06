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
    public int WoodFromBank = 0;
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
    public void addWood(int woodToAdd, bool isFromMine = false, bool isFromTree = false, bool isFromCasino=false, bool isFromBank=false)
    {
        Wood += woodToAdd;
        CheckBuilding();
        Debug.Log("Wood: " + Wood.ToString());        

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
        if(isFromBank)
        {
            WoodFromBank += woodToAdd;
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
        // Wood Event 1: First worker at 20 wood + stockpile building
        if (Wood >= 7 && !didStockpileEvent) {
            if(GameObject.Find("Storage").GetComponent<Storage>().IsBuilt) {
                postStockpileEvent();
            }
        }

        // Wood Event 2: Unlock robots at 200 wood + mine building
        if(WoodFromMine >= 200 && !didMineEvent) {
            if(GameObject.Find("Mine").GetComponent<Mine>().IsBuilt) {
                postMineEvent();
            }
        }

        // Wood Event 3: Casino
        if (WoodFromCasino <= -400 && !didCasinoEvent)
        {
            postcasinoEvent();
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
                NpcManager.npcManager.npc1.gameObject.SetActive(true);
                NpcManager.npcManager.npc2.gameObject.SetActive(true);
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
                NpcManager.npcManager.npc1.gameObject.SetActive(true);
                NpcManager.npcManager.npc2.gameObject.SetActive(true);
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
                    new UnityAction(delegate () { Bank.bank.isCaptchaRequired = true; })
                );
                SurpriseManager.Smanager.PostSurprise(surprise3, true);
            })
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, false);
    }

    /////////////////////////////////////////////////////////////
    // Wood Event 2: Unlock robots at 200 wood + mine building //
    /////////////////////////////////////////////////////////////
    private void postMineEvent()
    {
        didMineEvent = true;
        Surprise surprise1 = new Surprise(
            "All We Have Are Red Pills",
            "The future is robots, and the robots do it all. But can you rest easily " +
            "knowing that the robots do everything? You head down to the mine, and are " +
            "suprised to see all the robots on strike. \"We demand higher minimum wage and" +
            "healthcare.\" Where did they even learn these things? ",
            30,
            3, //Blacksmith
            true,

            "",
            "",
            null,

            "Give worker benefits",
            "Let the robots have what they want.",
            // Choice 1: Gain first worker.
            new UnityAction(delegate () {
                SubtractWood(Wood);
                NpcManager.npcManager.robot2.gameObject.SetActive(true);
                NpcManager.npcManager.robot3.gameObject.SetActive(true);
                NpcManager.npcManager.robot4.gameObject.SetActive(true);
                Surprise surprise2 = new Surprise(
                    "All We Have Are Red Pills",
                    "You successfully convince the lazy robots to get off their butts and keep working. But at the cost of your entire wood supply. ",
                    24,
                    4, //Robot
                    false,
                    "Continue",
                    "",
                    null //new UnityAction(delegate () { AddWorker(1); })    ADD FUNCTION TO ADD +1 WORKER
                );
                SurpriseManager.Smanager.PostSurprise(surprise2, true);
            }),

            "Economic downsizing",
            "Rework your org-chart a little.",
            // Choice 2: Bank now requires solving a math problem to use it.
            new UnityAction(delegate () {
                NpcManager.npcManager.robot2.gameObject.SetActive(true);
                Surprise surprise3 = new Surprise(
                    "All We Have Are Red Pills",
                    "You pick the two best robots and kick the rest to the curb. That'll show them.",
                    30,
                    4, //robots
                    false,

                    "Continue",
                    "",
                    null //new UnityAction(delegate () { EnableBankCaptcha; })    ADD FUNCTION TO ENABLE BANK CAPTCHA
                );
                SurpriseManager.Smanager.PostSurprise(surprise3, true);
            })
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, false);
    }


    // casino event
    private void postcasinoEvent()
    {
        didCasinoEvent = true;
        Surprise surprise1 = new Surprise(
            "Right Back At Ya!",
            "The Neighoring King is here, and he's FURIOUS. He just lost all of his kingdom in your casino, and is claiming that the casino is rigged.",
            30,
            20, //Casino
            true,

            "",
            "",
            null,

            "Of course it's rigged it's a casino",
            "what did you think would happen?",
            // Choice 1: Gain first worker.
            new UnityAction(delegate () {
                NpcManager.npcManager.npc1.gameObject.SetActive(true);
                NpcManager.npcManager.npc2.gameObject.SetActive(true);
                Surprise surprise2 = new Surprise(
                    "Wood War III",
                    "The king has declared war on you. Not that it matters, you're still on his property",
                    24,
                    15, //Player
                    false,
                    "Continue",
                    "",
                    null //new UnityAction(delegate () { AddWorker(1); })    ADD FUNCTION TO ADD +1 WORKER
                );
                SurpriseManager.Smanager.PostSurprise(surprise2, true);
            }),

            "Maybe if you could scratch my back I could scratch yours?",
            "Both literally and figuratively",
            // Choice 2: Bank now requires solving a math problem to use it.
            new UnityAction(delegate () {
                Surprise surprise3 = new Surprise(
                    "Right Back At Ya!",
                    "You return some of the king's riches, but keep the rest. But you at least told him that you tried your best. ",
                    30,
                    21, //logs
                    false,

                    "Basically just got away with fraud, nice!",
                    "",
                    null //new UnityAction(delegate () { EnableBankCaptcha; })    ADD FUNCTION TO ENABLE BANK CAPTCHA
                );
                SurpriseManager.Smanager.PostSurprise(surprise3, true);
            })
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, false);
    }
}
