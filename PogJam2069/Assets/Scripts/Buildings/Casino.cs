using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Casino : MonoBehaviour, IBuilding
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
    public Text notifTextBox;
    public GameObject FabovePlayer;

    private bool canPressF = false;
    private bool casinoIsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        notifTextBox.text = Cost.ToString() + "Wood";
        BuildingCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canPressF)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                Debug.Log("hey");
                if(!IsBuilt && WoodManager.Wmanager.Wood >= Cost)
                {
                    BuildBuilding();
                }
                else if(IsBuilt && casinoIsOpen)
                {
                    BuildingUiManager.buildingUi.CasinoUI.SetActive(false);
                    Time.timeScale = 1f;
                    casinoIsOpen = false;
                }
                else if(IsBuilt && !casinoIsOpen)
                {
                    BuildingUiManager.buildingUi.CasinoUI.SetActive(true);
                    Time.timeScale = 0f;
                    casinoIsOpen = true;
                }
            }
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
            if(IsBuilt)
            {
                FabovePlayer.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPressF = false;
            FabovePlayer.SetActive(false);
        }
    }
}
