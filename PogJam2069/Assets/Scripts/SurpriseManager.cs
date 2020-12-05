using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;  //UnityEngine.Events.UnityAction used for delegates to button onClicks
using UnityEngine.UI;

public class SurpriseManager : MonoBehaviour {
    public static SurpriseManager Smanager;

    public GameObject surpriseFramePrefab; // Prefab used for dialog prompts.
    public List<Sprite> images;            // List of possible images for dialog prompt descriptions.
    public List<GameObject> activeFrames;  // Current list of all active Surprise Event dialog prompts.

    
    /////////////////
    // Constructor //
    /////////////////
    public SurpriseManager() {
        this.activeFrames = new List<GameObject>();
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
        /* noFunc  */ if (s.noFunc != null) {newSurpriseFrame.transform.GetChild(5).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(s.noFunc);}

        /* c1Title */ newSurpriseFrame.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = s.c1Title;
        /* c1Desc  */ newSurpriseFrame.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = s.c1Desc;
        /* c1Func  */ if (s.c1Func != null) {newSurpriseFrame.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(s.c1Func);}

        /* c2Title */ newSurpriseFrame.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = s.c2Title;
        /* c2Desc  */ newSurpriseFrame.transform.GetChild(4).GetChild(1).gameObject.GetComponent<Text>().text = s.c2Desc;
        /* c2Func  */ if (s.c2Func != null) {newSurpriseFrame.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(s.c2Func);}
        

        // Close dialog on button click
        newSurpriseFrame.transform.GetChild(5).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(delegate() {
            SurpriseManager.Smanager.activeFrames.Remove(GameObject.Find("SurpriseFrame(Clone)"));
            Destroy(GameObject.Find("SurpriseFrame(Clone)"));
        }));
        newSurpriseFrame.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(delegate() {
            SurpriseManager.Smanager.activeFrames.Remove(GameObject.Find("SurpriseFrame(Clone)"));
            Destroy(GameObject.Find("SurpriseFrame(Clone)"));
        }));
        newSurpriseFrame.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(delegate() {
            SurpriseManager.Smanager.activeFrames.Remove(GameObject.Find("SurpriseFrame(Clone)"));
            Destroy(GameObject.Find("SurpriseFrame(Clone)"));
        }));
        
        // Final stuff
        newSurpriseFrame.GetComponent<CanvasScaler>().scaleFactor = 0.8f;
        this.activeFrames.Add(newSurpriseFrame);
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
    public bool choice;  // Whether or not this surprise has choices.

    public string noTitle;     // Button text for no-choice surprises.
    public string noDesc;      // Description text for no-choice surprises.
    public UnityAction noFunc; // Function() to call when no-choice surprise is accepted.

    public string c1Title;     // Button text for choice 1.
    public string c1Desc;      // Description text for choice 1.
    public UnityAction c1Func; // Function() to call when choice 1 is selected.

    public string c2Title;     // Button text for choice 2
    public string c2Desc;      // Description text for choice 2.
    public UnityAction c2Func; // Function() to call when choice 2 is selected.


    /////////////////
    // Constructor //
    /////////////////
    public Surprise(string title = "Huey Dies!",
                    string desc  = "Huey died. Should we take his dog in, or sell it for wood?",
                    int img      = 0,
                    bool choice  = false,

                    string noTitle     = "Wtf",
                    string noDesc      = "Haha just kidding. You have no choice. Get rekt scrub.",
                    UnityAction noFunc = null,

                    string c1Title     = "Take Dog In",
                    string c1Desc      = "+1 Doge",
                    UnityAction c1Func = null,

                    string c2Title     = "Sell The Dog",
                    string c2Desc      = "+1 Wood",
                    UnityAction c2Func = null) {
        this.title = title;
        this.desc  = desc;
        this.img   = img;
        this.choice  = choice;
        
        this.noTitle   = noTitle;
        this.noDesc    = noDesc;
        this.noFunc = noFunc;

        this.c1Title   = c1Title;
        this.c1Desc    = c1Desc;
        this.c1Func = c1Func;

        this.c2Title   = c2Title;
        this.c2Desc    = c2Desc;
        this.c2Func = c2Func;
    }
}
