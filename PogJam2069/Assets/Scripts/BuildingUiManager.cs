using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUiManager : MonoBehaviour
{
    public static BuildingUiManager buildingUi;

    public GameObject CasinoUI;
    public GameObject BankUi;


    // Start is called before the first frame update
    void Start()
    {
        if (buildingUi != null && buildingUi != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            buildingUi = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
