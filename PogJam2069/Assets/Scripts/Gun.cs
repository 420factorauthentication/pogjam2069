using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 10f;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * transform.parent.eulerAngles.z), Mathf.Sin(Mathf.Deg2Rad * transform.parent.eulerAngles.z)).normalized * speed;
    }
}
