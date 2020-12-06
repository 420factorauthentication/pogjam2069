using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour, IBuilding
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
    public GameObject canBeBuiltOutline;
    public GameObject builtSprite;
    public Text notifTextBox;

    private bool canPressF = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (!IsBuilt && currWood >= Cost)
        {
            canBeBuiltOutline.SetActive(true);
            builtSprite.SetActive(false);
            notifTextBox.gameObject.SetActive(true);
            notifTextBox.text = "Press F to Build";
        }
        else if (!IsBuilt && currWood < Cost && canBeBuiltOutline.activeSelf)
        {
            canBeBuiltOutline.SetActive(false);
            builtSprite.SetActive(false);
            notifTextBox.gameObject.SetActive(false);
            notifTextBox.text = "";
        }
    }

    public void BuildBuilding()
    {
        if (!IsBuilt && WoodManager.Wmanager.Wood >= Cost)
        {
            WoodManager.Wmanager.PurchaseWithWood(Cost);
            canBeBuiltOutline.SetActive(false);
            builtSprite.SetActive(true);
            notifTextBox.gameObject.SetActive(false);
            notifTextBox.text = "";
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
