using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    private bool dirRight = true;
    public float speed = 2.0f;
    public float difference;
    private float rightclamp;
    private float leftclamp;
    SpriteRenderer sr;

    private void Start()
    {
        rightclamp = transform.position.x + difference;
        leftclamp = transform.position.x - difference;
        sr = GetComponent<SpriteRenderer>();
    }



    void Update()
    {
        if(dirRight == true)
        {
            sr.flipX = false;
        }
        if(dirRight == false)
        {
            sr.flipX = true;
        }
        

        if (dirRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

        if (transform.position.x >= rightclamp)
        {
            dirRight = false;
        }

        if (transform.position.x <= leftclamp)
        {
            dirRight = true;
        }
    }
}
