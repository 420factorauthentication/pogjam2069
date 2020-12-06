using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour, IBuilding
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
    public bool bankIsOpen = false;
    public float rate = 30f;
    public int gain = 0;
    public Animator anim;
    public Text gainSnippet;
    public bool canBeBuilt = false;

    private bool wasPinged = false;

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
        if (canPressF)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if (!IsBuilt && WoodManager.Wmanager.Wood >= Cost)
                {
                    BuildBuilding();
                }
                else if (IsBuilt && bankIsOpen)
                {
                    BuildingUiManager.buildingUi.BankUi.SetActive(false);
                    Time.timeScale = 1f;
                    bankIsOpen = false;
                }
                else if (IsBuilt && !bankIsOpen)
                {
                    BuildingUiManager.buildingUi.BankUi.SetActive(true);
                    Time.timeScale = 0f;
                    bankIsOpen = true;
                }
            }
        }

        if(IsBuilt)
        {
            if (timeSinceLast > rate && gain != 0)
            {
                float plusNeg = Random.Range(-0f, 1f) < 0.5 ? -1 : 1;
                float rate = Random.Range(0.01f, 0.20f);

                gain += (int)(gain * rate * plusNeg);
                BuildingUiManager.buildingUi.BankUi.GetComponent<BankUI>().UpdateGain(gain);

                gainSnippet.text = (gain * rate * plusNeg) > 0 ? "+" + (gain * rate * plusNeg).ToString() : (gain * rate * plusNeg).ToString();
                anim.SetTrigger("show");
                timeSinceLast = 0f;
            }
            else
            {
                timeSinceLast += Time.deltaTime;
            }
        }
        if(canBeBuilt && !wasPinged)
        {
            BuildingCanvas.SetActive(true);
            notifTextBox.text = Cost.ToString() + " Wood (F)";
            wasPinged = true;
        }
    }

    public void CollectGain()
    {
        BuildingUiManager.buildingUi.BankUi.GetComponent<BankUI>().UpdateGain(0);
        if(WoodManager.Wmanager.Wood + gain > 0)
        {
            WoodManager.Wmanager.addWood(gain, isFromBank: true);
            gain = 0;
        }
    }

    public void Deposit50Gold()
    {
        gain += 50;
        WoodManager.Wmanager.SubtractWood(50);
        BuildingUiManager.buildingUi.BankUi.GetComponent<BankUI>().UpdateGain(gain);
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
