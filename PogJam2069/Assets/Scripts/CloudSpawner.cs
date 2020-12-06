using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public float cloudSpawnTimer;
    public GameObject[] CloudSpawnLocations;
    private float timer;
    public GameObject cloud;
    private int cloudSpawnPointid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        
        if(timer >= cloudSpawnTimer)
        {
            cloudSpawnPointid = Random.Range(0, CloudSpawnLocations.Length);
            Instantiate(cloud, CloudSpawnLocations[cloudSpawnPointid].transform.position, transform.rotation);
            timer = 0;
        }
    }
}
