using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    House,
    NpcHouse,
    Storage,
    Blackum,
    Armory,
    Mine,
    Bank
}

public class Building : MonoBehaviour
{
    public string buildingName;
    public BuildingType buildingType;
    public int WoodGain = 2;
    public float WoodGainRate = 0f; // set this to 0 for it to not do anything
    public bool isBuilt = false;
    public SpriteRenderer sr;
    public Sprite canBeBuild;
    public Sprite buildingSprite;

    private float timeSinceLastTask = 0f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilt)
        {
            if (WoodGainRate > 0 && timeSinceLastTask > WoodGainRate)
            {
                WoodManager.Wmanager.addWood(WoodGain);
                // then but animation stuff here
                switch (buildingType)
                {
                    case BuildingType.House:
                        break;
                    case BuildingType.NpcHouse:
                        break;
                    case BuildingType.Storage:
                        break;
                    case BuildingType.Blackum:
                        break;
                    case BuildingType.Armory:
                        break;
                    case BuildingType.Mine:
                        break;
                    case BuildingType.Bank:
                        break;
                }
                timeSinceLastTask = 0f;
            }
        }

        if (WoodGainRate > 0)
        {
            timeSinceLastTask += Time.deltaTime;
        }
    }

    public void CanBuild()
    {
        if (!isBuilt)
        {
            sr.transform.localScale = new Vector3(1f, 1f, 1f);
            sr.sprite = canBeBuild;
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = buildingSprite.rect.size / canBeBuild.pixelsPerUnit;
        }
    }

    public void CanNotBuild()
    {
        if (!isBuilt && sr.sprite != null)
        {
            sr.sprite = null;
        }
    }

    public void Build()
    {
        if (!isBuilt)
        {
            sr.drawMode = SpriteDrawMode.Simple;
            sr.transform.localScale = new Vector3(1f, 1f, 1f);
            sr.sprite = buildingSprite;
            isBuilt = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("hey");
        if(collision.tag == "Player" &&  Input.GetKey(KeyCode.F))
        {
            if(BuildingManager.buildManager.TryPayForBuildings(buildingType))
            {
                BuildingManager.buildManager.PayForBuilding(buildingType);
                Build();
            }
            else
            {
                // say there was problem building building or something or no money but not really possible.
            }
        }
    }
}
