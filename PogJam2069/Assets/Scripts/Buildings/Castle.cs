using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Castle : MonoBehaviour, IBuilding
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
    public GameObject FabovePlayer;

    private bool canPressF = false;
    private bool casinoIsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (canPressF)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                Debug.Log("hey");
                if (!IsBuilt && WoodManager.Wmanager.Wood >= Cost)
                {
                    BuildBuilding();
                }
                else if (IsBuilt && casinoIsOpen)
                {
                    BuildingUiManager.buildingUi.CasinoUI.SetActive(false);
                    Time.timeScale = 1f;
                    casinoIsOpen = false;
                }
                else if (IsBuilt && !casinoIsOpen)
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

        }
    }

    public void BuildBuilding()
    {
        if (!IsBuilt && WoodManager.Wmanager.Wood >= Cost)
        {
            WoodManager.Wmanager.PurchaseWithWood(Cost);
            IsBuilt = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPressF = true;
            FabovePlayer.SetActive(true);
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

    private void endGame()
    {
        Surprise surprise1 = new Surprise(
            "Wood You Look At That",
            "You have dominated the wood kingdom.\n" +
            "Congratulations!",
            30,
            23, //castle
            false,

            "Quit Game",
            "",
                                new UnityAction(delegate () { Application.Quit(); })
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }
}
