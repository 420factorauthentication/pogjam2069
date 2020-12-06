using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodText : MonoBehaviour
{

    public Text woodtext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        woodtext.text = WoodManager.Wmanager.Wood.ToString();
    }
}
