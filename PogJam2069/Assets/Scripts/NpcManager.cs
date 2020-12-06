using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager npcManager;

    // match npcs to npc houses 1 to one even if they aren't there yet. 
    public List<BaseSlave> allNpcs;
    public List<House> allNpcHouses;

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
        
    }

    public bool AddNpc(BaseSlave npc)
    {
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
}
