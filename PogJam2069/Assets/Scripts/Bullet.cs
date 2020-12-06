using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    private void Update()
    {
        
    }

    private void Start()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
        rb2d.MoveRotation(angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
