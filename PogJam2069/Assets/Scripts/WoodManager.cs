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
    }

    public void SubtractWood(int woodToSubtrack)
    {
        if(woodToSubtrack <= Wood)
        {
            Wood -= woodToSubtrack;
        }
    }

}
