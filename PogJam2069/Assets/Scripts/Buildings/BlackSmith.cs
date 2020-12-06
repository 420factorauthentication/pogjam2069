using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmith : MonoBehaviour, IBuilding
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
    public bool canBeBuilt = false;
    public bool didGuardEvet = false;

    // every guard that you can get and change how they spawn in as you like
    public Guard guard1;
    public Guard guard2;

    public BanditSpawner spawn1;
    public BanditSpawner spawn2;

    private bool canPressF = false;

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
            if (Input.GetKeyDown(KeyCode.F) && WoodManager.Wmanager.Wood >= Cost)
            {
                BuildBuilding();
            }
        }
    }

    public void CheckCanBuild(int currWood)
    {
        if (!IsBuilt && canBeBuilt && currWood >= Cost)
        {
            BuildingCanvas.SetActive(true);
            notifTextBox.text = Cost.ToString() + " Wood (F)";

        }
        else if (!IsBuilt && canBeBuilt)
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
            guard1.gameObject.SetActive(true);
            postBlackAndMineEvent();
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

    private void postBlackAndMineEvent()
    {
        didGuardEvet = true;
        guard1.gameObject.SetActive(true);
        spawn1.canSpawn = true;
        spawn2.canSpawn = true;
        Surprise surprise1 = new Surprise(
            "No Swiping!",
            "There have been some recent cases of bandits appearing and stopping your wood productions! Thankfully, you can hire some guards in front of the blacksmith to help you out.",
            30,
            3, //blacksmith
            false,

            "Nice!",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }
}
