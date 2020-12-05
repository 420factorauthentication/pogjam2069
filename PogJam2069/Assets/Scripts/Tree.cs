using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public int treehitsToDestroy;
    private int currenthits;
    public int woodToGive;
    public float treeRespawnTimer;
    private float timer;
    public bool treeisDead;
    public Sprite TreeAlive;
    public Sprite TreeDead;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currenthits = 0;
        treeisDead = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(currenthits >= treehitsToDestroy)
        {

            //give wood
            WoodManager.Wmanager.addWood(woodToGive);
            treeisDead = true;
            currenthits = 0;

        }

        if(treeisDead == true)
        {
            sr.sprite = TreeDead;
            timer += Time.deltaTime;
        }


        if(timer >= treeRespawnTimer)
        {
            timer = 0;
            sr.sprite = TreeAlive;
            treeisDead = false;
        }

    }

    public void hitTree()
    {
        if (treeisDead == false)
        {
            currenthits += 1;
        }
    }
}
