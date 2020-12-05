using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurpriseManager : MonoBehaviour {
    public GameObject surpriseFramePrefab;
    public List<Sprite> images;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newFrame = Instantiate(surpriseFramePrefab);
        newFrame.GetComponent<CanvasScaler>().scaleFactor = 0.8f;
        print("Hello World!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

/////////////////////////////////////////////
// class Surprise                          //
//                                         //
// Container object for a Surprise prompt. //
// Contains text, images, and config.      //
/////////////////////////////////////////////
//public class Surprise {
//    public GameObject framePrefab;         // Prefab used for dialog prompts.
//    public readonly GameObject currFrame;  // GameObj for dialog prompt. gameObject.SetActive() is toggled to show/hide prompt.
//    public readonly bool visible = false;  // Is the dialog prompt currently showing?
    
//    public readonly string title; // Title text of prompt.
//    public readonly string desc;  // Description text of prompt.
//    public readonly int img;      // Description image. Index into list.
    
//    public readonly bool choice;    // Whether or not this surprise has choices.
//    public readonly string noTitle; // Button text for no-choice surprises.
//    public readonly string noDesc;  // Description text for no-choice surprises.
//    public readonly string 1cTitle; // Button text for choice 1.
//    public readonly string 1cDesc;  // Description text for choice 1.
//    public readonly string 2cTitle; // Button text for choice 2
//    public readonly string 2cDesc;  // Description text for choice 2.


//    /////////////////////////////////////////////////////
//    // Constructor                                     //
//    //                                                 //
//    // After construction, use functions to configure. //
//    /////////////////////////////////////////////////////
//    public Surprise(GameObject framePrefab) {
//        this.framePrefab = framePrefab;
//        this.currFrame = Instantiate(framePrefab);
//        this.currFrame.GetComponent<CanvasScaler>().scaleFactor = 0.8f;
//        this.currFrame.SetActive(false);
//    }

//    /////////////////////////
//    // ShowPrompt()        //
//    //                     //
//    // Show dialog prompt. //
//    /////////////////////////
//    public ShowPrompt() {
//        this.visible = true;
//        this.currFrame.SetActive(true);
//    }

//    //////////////////////////////////////////////////////////
//    // HidePrompt()                                         //
//    //                                                      //
//    // Hide dialog prompt.                                  //
//    // Also called when clicking a choice or accept button. //
//    //////////////////////////////////////////////////////////
//    public HidePrompt() {
//        this.visible = false;
//        this.currFrame.SetActive(false);
//    }
    
//    //////////////////////////////////////////////////////////////////////////
//    // SetProperty()                                                        //
//    //   string property: The class property to modify.                     //
//    //   |int, string, bool| value: The value to assign to the property.    //
//    //                                                                      //
//    // Updates a class property, and updates the frame display accordingly. //
//    //////////////////////////////////////////////////////////////////////////
//    // public SetProperty(string property, int value) {
//    // }
//}
