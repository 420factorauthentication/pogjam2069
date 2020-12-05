using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTree : MonoBehaviour
{
    public float range;
    public Transform tree;
    public bool pointToTree = true;
    private float defaultAngle = 60f;
    private bool facingRight = false;
    private SpriteRenderer WeaponSprite;

    // Start is called before the first frame update
    void Start()
    {
        WeaponSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pointToTree && tree != null && Mathf.Abs(Vector2.Distance(tree.position, transform.position)) < range)
        {
            var dir = (tree.position - transform.position).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            WeaponFlipping();
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
        }
    }

    void WeaponFlipping()
    {
        var delta = tree.position - transform.position;

        //CharacterFlipping
        if (delta.x >= 0 && !facingRight)
        {
            WeaponSprite.flipY = false;
            facingRight = true;

        }
        else if (delta.x < 0 && facingRight)
        { 
            WeaponSprite.flipY = true;
            facingRight = false;
        }
    }
}
