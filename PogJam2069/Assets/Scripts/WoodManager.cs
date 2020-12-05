using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodManager : MonoBehaviour
{
    public static WoodManager Wmanager;
    public int Wood;

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
        BuildingManager.buildManager.UpdateWoodAvailibility(Wood);
    }

    public void SubtractWood(int woodToSubtrack)
    {
        if(woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
        else
        {
            Wood = 0;
        }
        BuildingManager.buildManager.UpdateWoodAvailibility(Wood);
    }

    public void PurchaseWithWood(int woodToSubtrack)
    {
        if (woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
        BuildingManager.buildManager.UpdateWoodAvailibility(Wood);
    }



}
