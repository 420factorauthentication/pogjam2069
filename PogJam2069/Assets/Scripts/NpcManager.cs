using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcManager : MonoBehaviour
{
    public static NpcManager npcManager;

    // for player movement im tired so im putting it here
    public bool isMirrored = false;
    public bool isFrozen = false;

    public BaseSlave npc1;
    public BaseSlave npc2;
    public BaseSlave npc3;
    public BaseSlave npc4;

    public BaseSlave robot1;
    public BaseSlave robot2;
    public BaseSlave robot3;
    public BaseSlave robot4;

    // match npcs to npc houses 1 to one even if they aren't there yet. 
    public List<BaseSlave> allNpcs;
    public List<House> allNpcHouses;
    public List<BaseSlave> robots;

    private bool unlockedBlackandMine = false;

    // Start is called before the first frame update
    void Start()
    {
        if (npcManager != null && npcManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            npcManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(allNpcs.Count >=2 && !npc3.gameObject.activeSelf)
        {
            npc3.gameObject.SetActive(true);
            npc4.gameObject.SetActive(true);
        }
        if(allNpcHouses.Count >= 1 && !unlockedBlackandMine)
        {
            postBlackAndMineEvent();
        }
    }

    public bool AddNpc(BaseSlave npc, bool mineCheck = false)
    {
        if(mineCheck)
        {
            robots.Add(npc);
            return true;
        }
        if(allNpcHouses.Count > allNpcs.Count)
        {
            allNpcs.Add(npc);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddNpcHouse(House house)
    {
        allNpcHouses.Add(house);
    }

    // for player movement im tired so im putting it here
    public void MirrorMovement() {
        if (NpcManager.npcManager.isMirrored)
            NpcManager.npcManager.isMirrored = false;
        else
            NpcManager.npcManager.isMirrored = true;
    }















    private void postBlackAndMineEvent()
    {
        unlockedBlackandMine = true;
        WoodManager.Wmanager.buildings.Find(x => x.GetComponent<BlackSmith>() != null).GetComponent<BlackSmith>().canBeBuilt = true;
        WoodManager.Wmanager.buildings.Find(x => x.GetComponent<Mine>() != null).GetComponent<Mine>().canBebuilt = true;
        robot1.gameObject.SetActive(true);
        Surprise surprise1 = new Surprise(
            "Wood You Look At That",
            "All is good in the hood, but now there are too many people and only one tree! " +
            "You wake up one morning and are suprised to see some people start a blacksmith " +
            "company. There's also a peculiar figure to the north...",
            30,
            3, //blacksmith
            false,

            "Okay",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }
}
