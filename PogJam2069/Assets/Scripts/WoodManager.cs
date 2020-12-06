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
    private bool didWoodStockpileEvent = false;
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

    public void addWood(int woodToAdd, bool isFromMine = false, bool isFromTree = false, bool isFromCasino=false)
    {
        Wood += woodToAdd;
        CheckBuilding();
        if(isFromMine)
        {
            WoodFromMine += woodToAdd;
        }
        if(isFromTree)
        {
            WoodFromTreeFromNpc += woodToAdd;
        }
        if(isFromCasino)
        {
            WoodFromCasino += woodToAdd;
        }
        EventCheck();
    }

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

    public void PurchaseWithWood(int woodToSubtrack)
    {
        if (woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
        CheckBuilding();
        EventCheck();
    }

    private void CheckBuilding()
    {
        foreach (GameObject build in buildings)
        {
            build.GetComponent<IBuilding>().CheckCanBuild(Wood);
        }
    }

    private void EventCheck()
    {
        if(Wood > 20 && !didWoodStockpileEvent)
        {
            if(buildings.Find(x => x.GetComponent<Storage>() != null) != null)
            {
                // put event data here
                didWoodStockpileEvent = true;
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


    //////////////////////////////
    // Event Function Delegates //
    //////////////////////////////
}
