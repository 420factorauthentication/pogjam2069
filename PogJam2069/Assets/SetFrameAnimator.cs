using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrameAnimator : MonoBehaviour
{
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SurpriseManager.Smanager.setAnimator(anim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
