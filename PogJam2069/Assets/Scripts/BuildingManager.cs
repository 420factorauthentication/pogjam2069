using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager buildManager;

    public int houseCost;
    public int npcHouseCost;
    public int storageCost;
    public int blackumCost;
    public int armoryCost;
    public int mineCost;
    public int bankCost;
    public List<Building> buildings = new List<Building>();

    [SerializeField]
    private bool canPurchaseHouse;
    private bool canPurchaseNpcHouse;
    private bool canPurchaseStorage;
    private bool canPurchaseBlackum;
    private bool canPurchaseArmory;
    private bool canPurchaseMine;
    private bool canPurchaseBank;

    private void Awake()
    {
        if (buildManager != null && buildManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            buildManager = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        // oh god
        if (canPurchaseHouse)
        {
            CheckCanBuild(BuildingType.House);
        }
        else
        {
            CheckNoBuild(BuildingType.House);
        }
        if (canPurchaseNpcHouse)
        {
            CheckCanBuild(BuildingType.NpcHouse);
        }
        else
        {
            CheckNoBuild(BuildingType.NpcHouse);
        }
        if (canPurchaseStorage)
        {
            CheckCanBuild(BuildingType.Storage);
        }
        else
        {
            CheckNoBuild(BuildingType.Storage);
        }
        if (canPurchaseBlackum)
        {
            CheckCanBuild(BuildingType.Blackum);
        }
        else
        {
            CheckNoBuild(BuildingType.Blackum);
        }
        if (canPurchaseArmory)
        {
            CheckCanBuild(BuildingType.Armory);
        }
        else
        {
            CheckNoBuild(BuildingType.Armory);
        }
        if (canPurchaseMine)
        {
            CheckCanBuild(BuildingType.Mine);
        }
        else
        {
            CheckNoBuild(BuildingType.Mine);
        }
        if (canPurchaseBank)
        {
            CheckCanBuild(BuildingType.Bank);
        }
        else
        {
            CheckNoBuild(BuildingType.Bank);
        }
    }

    public void UpdateWoodAvailibility(int currWoodCount)
    {
        canPurchaseHouse = currWoodCount > houseCost;
        canPurchaseNpcHouse = currWoodCount > npcHouseCost;
        canPurchaseStorage = currWoodCount > storageCost;
        canPurchaseBlackum = currWoodCount > blackumCost;
        canPurchaseArmory = currWoodCount > armoryCost;
        canPurchaseMine = currWoodCount > mineCost;
        canPurchaseBank = currWoodCount > bankCost;
    }

    private void CheckCanBuild(BuildingType type)
    {
        Building build = buildings.Find(x => x.buildingType == type);
        if (build != null)
        {
            build.CanBuild();
        }
    }

    private void CheckNoBuild(BuildingType type)
    {
        Building build = buildings.Find(x => x.buildingType == type);
        if (build != null)
        {
            build.CanNotBuild();
        }
    }
}
