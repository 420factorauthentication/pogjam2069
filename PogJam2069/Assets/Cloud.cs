using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    public float speed;
    public Vector3 CloudDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position += CloudDirection * step;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.tag == "CloudDestroy")
        {
            Destroy(gameObject);
        }
    }
}
