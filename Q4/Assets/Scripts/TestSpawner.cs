using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour {

    //Reference Variables 
    Transform trans;
    public GameObject spawnObject;

    //Spawner Variables
    public bool Enabled = true;
    float spawnAlarm = 0;
    public float spawnTime = 3f;

    //Define Reference Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {

        //Only Spawn If Enabled
        if (Enabled)
        {
            //Deduct Spawn Alarm
            if (spawnAlarm - Time.deltaTime > 0)
            {
                spawnAlarm -= Time.deltaTime;
            }
            //Create Spawner Gameobject
            else
            {
                Instantiate(spawnObject, trans.position, Quaternion.identity);
                spawnAlarm = spawnTime;
            }
        }
	}
}
