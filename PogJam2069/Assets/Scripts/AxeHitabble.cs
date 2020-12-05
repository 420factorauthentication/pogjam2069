using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AxeHitabble : MonoBehaviour
{

    public UnityEvent AxeHitEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void axeHit()
    {
        AxeHitEvent.Invoke();
    }
}
