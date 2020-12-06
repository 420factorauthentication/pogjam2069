using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    public Text currValText;
    public Button buy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(WoodManager.Wmanager.Wood < 50 && buy.enabled)
        {
            buy.enabled = false;
        }
        else if(WoodManager.Wmanager.Wood >= 50 && !buy.enabled)
        {
            buy.enabled = true;
        }
    }

    public void UpdateGain(int amount)
    {
        currValText.text = "Gain: " + amount.ToString() + " Wood";
    }
}
