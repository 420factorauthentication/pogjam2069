using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;  //UnityEngine.Events.UnityAction used for delegates to button onClicks
using UnityEngine.SceneManagement;   //SceneManager.LoadSceneAsync()

public class Tree : MonoBehaviour
{

    public int treehitsToDestroy;
    private int currenthits;
    public int woodToGive;
    public float treeRespawnTimer;
    private float timer;
    public bool treeisDead;
    public GameObject TreeTop;
    public GameObject TreeBottom;
    public Sprite TreeDead;
    private bool didTreeEvent1 = false;
    private bool didTreeEvent2 = false;

    public Animator anim;
    private int totalChops = 0;


    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currenthits = 0;
        treeisDead = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currenthits >= treehitsToDestroy)
        {
            treeChop();
        }

        if(treeisDead == true)
        {
            sr.sprite = TreeDead;
            TreeTop.SetActive(false);
            TreeBottom.SetActive(false);
            timer += Time.deltaTime;
        }


        if(timer >= treeRespawnTimer)
        {
            timer = 0;
            sr.sprite = null;
            TreeBottom.SetActive(true);
            TreeTop.SetActive(true);
            treeisDead = false;
        }

    }

    ////////////////////////////////////////////////////////////////////
    // hitTree()                                                      //
    //                                                                //
    // This function is called when a player or a worker hits a tree. //
    ////////////////////////////////////////////////////////////////////
    public void hitTree()
    {
        if (treeisDead == false)
        {
            currenthits += 1;
            anim.SetTrigger("Hit");
        }
    }

    ////////////////////////////////////////////////////
    // treeChop()                                     //
    //                                                //
    // This function fires when the tree is cut down. //
    ////////////////////////////////////////////////////
    private void treeChop() {
        //increment chop counter
        totalChops++;
        Debug.Log("Chops: " + totalChops);

        //give wood
        WoodManager.Wmanager.addWood(woodToGive);
        treeisDead = true;
        currenthits = 0;

        //sfx
        AudioManager.Amanager.treeFall();
        
        // Tree Event 1: Lootbox at 1 Chop
        if (totalChops >= 1 && !didTreeEvent1) { PostTreeEvent1(); }

        // Tree Event 2: Better Tree At 30 Chops
        if (totalChops >= 30 && !didTreeEvent2) { PostTreeEvent2(); }
    }

    /////////////////////////////////////
    // Tree Event 1: Lootbox at 1 Chop //
    /////////////////////////////////////
    private void PostTreeEvent1() {
        didTreeEvent1 = true;
        Surprise surprise1 = new Surprise(
            "Found a lootbox?",
            "After chopping that last tree, you found a wooden loot box " +
                "buried in the soil behind it! What surprises could it hold?",
            30,
            17, //Tree1_Cut
            true,

            "",
            "",
            null,

            "Open the lootbox!",
            "Gain what's inside?",
            // Choice 1: Open it. Lose the game.
            new UnityAction(delegate () {
                Surprise surprise2 = new Surprise(
                    "",
                    "You open the chest. A speaker playing copyrighted " +
                        "music is inside. You get DMCA'd by 42069 record " +
                        "labels and lose the game.",
                    30,
                    0, //Fence
                    false,

                    "Game Over",
                    "Son of a Twitch!",
                    new UnityAction(delegate () {
                        // PauseGame();   ADD FUNCTION TO PAUSE ALL GAMEPLAY
                        SceneManager.LoadSceneAsync("MainMenu");
                    })
                );
                SurpriseManager.Smanager.PostSurprise(surprise2, true);
            }),

            "Sell the box for wood",
            "+1 Wood",
            // Choice 2: Sell it for +1 Wood.
            new UnityAction(delegate () {
                Surprise surprise3 = new Surprise(
                    "",
                    "You sell the box on BoxBay and turned a tidy profit.",
                    30,
                    18, //WoodPile
                    false,

                    "Continue",
                    "+1 Wood",
                    new UnityAction(delegate () { WoodManager.Wmanager.addWood(1); })
                );
                SurpriseManager.Smanager.PostSurprise(surprise3, true);
            })
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, false);
    }

    ///////////////////////////////////////////
    // Tree Event 2: Better Tree At 30 Chops //
    ///////////////////////////////////////////
    private void PostTreeEvent2() {
        didTreeEvent2 = true;
        Surprise surprise4 = new Surprise(
            "Treeureka!",
            "Why does this tree regrow so fast? What surprises do other dimensions " +
                "hold? You see in 3D. Maybe switching to Tree-D would reveal more?",
            30,
            10, //Cloud
            true,

            "",
            "",
            null,

            "Switch to Tree-D",
            "Learn more about the Tree Mystery?",
            // Choice 1: Switch to Tree-D. Reveal Giving Tree.
            new UnityAction(delegate () {
                Surprise surprise5 = new Surprise(
                    "",
                    "You can see more trees in this new dimension.",
                    30,
                    11, //Flower1
                    false,

                    "Continue",
                    "+1 Tree Mystery Progress",
                    null //new UnityAction(delegate () { spawnGivingTree(); })    ADD FUNCTION TO SPAWN GIVING TREE
                );
                SurpriseManager.Smanager.PostSurprise(surprise5, true);
            }),

            "Switch to 2D",
            "Learn more about the Tree Mystery?",
            // Choice 2: Switch to 2D. Movement controls are now mirrored.
            new UnityAction(delegate () {
                Surprise surprise6 = new Surprise(
                    "",
                    "You find out it's always opposite day in this dimension. " +
                        "Movement controls are now mirrored.",
                    30,
                    5, //Grass1
                    false,

                    "eunitnoC",
                    "ssergorP yretsyM eerT 0+",
                    new UnityAction(delegate () {
                        NpcManager.npcManager.MirrorMovement();
                    })
                );
                SurpriseManager.Smanager.PostSurprise(surprise6, true);
            })
        );
        SurpriseManager.Smanager.PostSurprise(surprise4, false);
    }
}
