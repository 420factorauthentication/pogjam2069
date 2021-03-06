﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb2D;

    float horizontal;
    float vertical;
    public bool facingRight;
    public float speed;
    public Transform CursorDirection;
    public SpriteRenderer WeaponSprite;
    public GameObject Fabove;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!NpcManager.npcManager.isFrozen) {
            if (!NpcManager.npcManager.isMirrored) {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }
            else {
                horizontal = Input.GetAxis("Horizontal") * (-1);
                vertical = Input.GetAxis("Vertical") * (-1);
            }
        }
        else {
            horizontal = 0;
            vertical = 0;
        }
    }

    private void FixedUpdate()
    {
        WeaponFlipping();
        rb2D.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void WeaponFlipping()
    {
        //current mouse position
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        var delta = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;

        //CharacterFlipping
        if (delta.x >= 0 && !facingRight)
        { // mouse is on right side of player

            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
            CursorDirection.transform.localScale = newScale;
            //sr.flipX = false;
            WeaponSprite.flipY = false;
            facingRight = true;
            Fabove.transform.localScale = new Vector3(1, Fabove.transform.localScale.y, Fabove.transform.localScale.z);
        }
        else if (delta.x < 0 && facingRight)
        { // mouse is on left side
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
            CursorDirection.transform.localScale = newScale;
            WeaponSprite.flipY = true;

            //sr.flipX = true;
            facingRight = false;

            Fabove.transform.localScale = new Vector3(-1, Fabove.transform.localScale.y, Fabove.transform.localScale.z);
        }
    }
}
