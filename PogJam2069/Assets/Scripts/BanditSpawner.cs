using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditSpawner : MonoBehaviour
{
    public GameObject bandit;
    public float respawnRate = 35f;
    public bool canSpawn = false;

    private float lastTimeSince = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn)
        {
            if (lastTimeSince > respawnRate)
            {
                Instantiate(bandit, transform.position, Quaternion.identity);
                Instantiate(bandit, transform.position, Quaternion.identity);
                lastTimeSince = 0f;
            }

            lastTimeSince += Time.deltaTime;
        }
    }
}
