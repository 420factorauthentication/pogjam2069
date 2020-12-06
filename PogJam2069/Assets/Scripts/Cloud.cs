using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    public float speed;
    public Vector3 CloudDirection;
    private Vector3 playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 dir = (playerTransform - gameObject.transform.position).normalized;
        CloudDirection = new Vector3(dir.x, dir.y, 0);
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
