using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Canvas stuff

public class SurpriseCanvas : MonoBehaviour
{
    public static GameObject Scanvas;
    void Awake() {
        if (Scanvas != null && Scanvas != this.gameObject){
            Destroy(this.gameObject);
        }
        else {
            Scanvas = this.gameObject;
        }
    }
}
