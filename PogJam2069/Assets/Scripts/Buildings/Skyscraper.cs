﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skyscraper : MonoBehaviour, IBuilding
{
    [SerializeField]
    private string _buildingName;
    [SerializeField]
    private int _cost;
    [SerializeField]
    private bool _isBuilt;

    public string BuildingName { get { return _buildingName; } set { _buildingName = value; } }
    public int Cost { get { return _cost; } set { _cost = value; } }
    public bool IsBuilt { get { return _isBuilt; } set { _isBuilt = value; } }
    public GameObject BuildingCanvas;
    public GameObject builtSprite;
    public int buildingLevel = 0;
    public List<int> upgradeCosts;
    public List<Sprite> buildingSprites;
    public Text notifTextBox;
    public float woodRate = 2f;
    public List<int> woodAmount = new List<int>();

    private bool canPressF = false;
    private float timeSinceLast = 0f;

    // Start is called before the first frame update
    void Start()
    {
        notifTextBox.text = Cost.ToString() + "Wood";
        BuildingCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canPressF)
        {
            if (Input.GetKeyDown(KeyCode.F) && WoodManager.Wmanager.Wood >= Cost)
            {
                BuildBuilding();
            }
        }

        if(IsBuilt)
        {
            if(timeSinceLast > woodRate)
            {
                WoodManager.Wmanager.addWood(10);
                timeSinceLast = 0f;
            }
            timeSinceLast += Time.deltaTime;
        }
    }

    public void CheckCanBuild(int currWood)
    {
        if (!IsBuilt && currWood >= Cost)
        {
            BuildingCanvas.SetActive(true);
            notifTextBox.text = Cost.ToString() + " Wood (F)";

        }
    }

    public void BuildBuilding()
    {
        if (!IsBuilt && WoodManager.Wmanager.Wood >= Cost)
        {
            WoodManager.Wmanager.PurchaseWithWood(Cost);
            builtSprite.SetActive(true);
            BuildingCanvas.SetActive(false);

            IsBuilt = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPressF = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPressF = false;
        }
    }
}
