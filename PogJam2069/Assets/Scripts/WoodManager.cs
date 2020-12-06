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
    private bool didWoodStockpileEvent = false;
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

    public void addWood(int woodToAdd, bool isFromMine = false, bool isFromTree = false)
    {
        Wood += woodToAdd;
        CheckBuilding();
        Debug.Log("Wood: " + Wood.ToString());
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
    }

    public void PurchaseWithWood(int woodToSubtrack)
    {
        if (woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
        CheckBuilding();
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

        }
    }


    //////////////////////////////
    // Event Function Delegates //
    //////////////////////////////
}
