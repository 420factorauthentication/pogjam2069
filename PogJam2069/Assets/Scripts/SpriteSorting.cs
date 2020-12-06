using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorting : MonoBehaviour
{
    public bool isplayer;
    public SpriteRenderer axeSr;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
        if (isplayer == true)
        {
            axeSr.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
        }

    }
}
