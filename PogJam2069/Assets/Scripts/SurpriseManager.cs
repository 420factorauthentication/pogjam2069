using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurpriseManager : MonoBehaviour {
    public static SurpriseManager Smanager;

    public GameObject surpriseFramePrefab;   // Prefab used for dialog prompts.
    public List<Sprite> images;              // List of possible images for dialog prompt descriptions.
    private List<GameObject> surpriseFrames; // Current list of all active Surprise Event dialog prompts.

    
    /////////////////
    // Constructor //
    /////////////////
    public SurpriseManager() {
        this.surpriseFrames = new List<GameObject>();
    }

    void Awake() {
        if (Smanager != null && Smanager != this){
            Destroy(this.gameObject);
        }
        else {
            Smanager = this;
        }
    }

    void Start() {
        
    }

    void Update() {
        
    }

    ///////////////////////////////////////////////
    // PostSurprise()                            //
    //   Surprise s: The Surprise to show.       //
    //                                           //
    // Shows a new Surprise Event dialog prompt. //
    ///////////////////////////////////////////////
    public void PostSurprise(Surprise s) {
        GameObject newSurpriseFrame = Instantiate(this.surpriseFramePrefab);
        /* title */ newSurpriseFrame.transform.GetChild(1).gameObject.GetComponent<Text>().text = s.title;
        /* desc  */ newSurpriseFrame.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = s.desc;
        /* img   */ newSurpriseFrame.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Image>().sprite = this.images[s.img];
        
        /* choice */
        if (s.choice) {
            newSurpriseFrame.transform.GetChild(3).gameObject.SetActive(true);
            newSurpriseFrame.transform.GetChild(4).gameObject.SetActive(true);
            newSurpriseFrame.transform.GetChild(5).gameObject.SetActive(false);
        } else {
            newSurpriseFrame.transform.GetChild(3).gameObject.SetActive(false);
            newSurpriseFrame.transform.GetChild(4).gameObject.SetActive(false);
            newSurpriseFrame.transform.GetChild(5).gameObject.SetActive(true);
        }

        /* noTitle */ newSurpriseFrame.transform.GetChild(5).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = s.noTitle;
        /* noDesc  */ newSurpriseFrame.transform.GetChild(5).GetChild(1).gameObject.GetComponent<Text>().text = s.noDesc;
        /* c1Title */ newSurpriseFrame.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = s.c1Title;
        /* c1Desc  */ newSurpriseFrame.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = s.c1Desc;
        /* c2Title */ newSurpriseFrame.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = s.c2Title;
        /* c2Desc  */ newSurpriseFrame.transform.GetChild(4).GetChild(1).gameObject.GetComponent<Text>().text = s.c2Desc;
        
        newSurpriseFrame.GetComponent<CanvasScaler>().scaleFactor = 0.8f;
        this.surpriseFrames.Add(newSurpriseFrame);
    }
}


/////////////////////////////////////////////
// class Surprise                          //
//                                         //
// Container object for a Surprise prompt. //
// Contains text, images, and config.      //
// TODO: implement choice resource changes //
/////////////////////////////////////////////
public class Surprise {
    public string title; // Title text of prompt.
    public string desc;  // Description text of prompt.
    public int img;      // Description image. Index into list.
    
    public bool choice;    // Whether or not this surprise has choices.
    public string noTitle; // Button text for no-choice surprises.
    public string noDesc;  // Description text for no-choice surprises.
    public string c1Title; // Button text for choice 1.
    public string c1Desc;  // Description text for choice 1.
    public string c2Title; // Button text for choice 2
    public string c2Desc;  // Description text for choice 2.


    /////////////////
    // Constructor //
    /////////////////
    public Surprise(string title = "Huey Dies!",
                    string desc  = "Huey died. Should we take his dog in, or sell it for wood?",
                    int img      = 0,
                    
                    bool choice    = false,
                    string noTitle = "Wtf",
                    string noDesc  = "Haha just kidding. You have no choice. Get rekt scrub.",
                    string c1Title = "Take Dog In",
                    string c1Desc  = "+1 Doge",
                    string c2Title = "Sell The Dog",
                    string c2Desc  = "+1 Wood") {
        this.title = title;
        this.desc  = desc;
        this.img   = img;

        this.choice  = choice;
        this.noTitle = noTitle;
        this.noDesc  = noDesc;
        this.c1Title = c1Title;
        this.c1Desc  = c1Desc;
        this.c2Title = c2Title;
        this.c2Desc  = c2Desc;
    }
}
