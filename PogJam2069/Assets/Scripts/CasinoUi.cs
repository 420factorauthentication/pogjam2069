using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasinoUi : MonoBehaviour
{
    public int lowWoodCost = 10;
    public int midWoodCost = 50;
    public int highWoodCost = 100;

    public Button lowWoodButton;
    public Button midWoodButton;
    public Button highWoodButton;

    public Text lowWoodText;
    public Text midWoodText;
    public Text highWoodText;

    // Start is called before the first frame update
    void Start()
    {
        lowWoodText.text = lowWoodCost.ToString() + " Wood";
        midWoodText.text = midWoodCost.ToString() + " Wood";
        highWoodText.text = highWoodCost.ToString() + " Wood";
    }

    // Update is called once per frame
    void Update()
    {
        if(WoodManager.Wmanager.Wood >= lowWoodCost)
        {
            lowWoodButton.enabled = true;
        }
        else
        {
            lowWoodButton.enabled = false;
        }
        if (WoodManager.Wmanager.Wood >= midWoodCost)
        {
            midWoodButton.enabled = true;
        }
        else
        {
            midWoodButton.enabled = false;
        }
        if (WoodManager.Wmanager.Wood >= highWoodCost)
        {
            highWoodButton.enabled = true;
        }
        else
        {
            highWoodButton.enabled = false;
        }
    }

    public void DoLowWoodRoll()
    {
        WoodManager.Wmanager.SubtractWood(lowWoodCost);
        WoodManager.Wmanager.WoodFromCasino -= lowWoodCost;
        float rate = Random.value + 0.00001f;
        float roll = Random.value;
        if(roll < rate)
        {
            WoodManager.Wmanager.addWood(lowWoodCost * 2, isFromCasino:true);
            Debug.Log("WINNER");
        }
        else
        {
            Debug.Log("haha you lost");
        }
    }

    public void DoMidWoodRool()
    {
        WoodManager.Wmanager.SubtractWood(midWoodCost);
        WoodManager.Wmanager.WoodFromCasino -= midWoodCost;
        float rate = Random.value + 0.00001f;
        float roll = Random.value;
        if (roll < rate)
        {
            WoodManager.Wmanager.addWood(midWoodCost * 2, isFromCasino: true);
            Debug.Log("WINNER");
        }
        else
        {
            Debug.Log("haha you lost");
        }
    }

    public void DoHighWoodRool()
    {
        WoodManager.Wmanager.SubtractWood(highWoodCost);
        WoodManager.Wmanager.WoodFromCasino -= highWoodCost;
        float rate = Random.value + 0.00001f;
        float roll = Random.value;
        if (roll < rate)
        {
            WoodManager.Wmanager.addWood(highWoodCost * 2, isFromCasino: true);
            Debug.Log("WINNER");
        }
        else
        {
            Debug.Log("haha you lost");
        }
    }
}
