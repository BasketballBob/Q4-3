using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour {

    //Reference Variables 
    Transform trans;
    public GameObject spawnObject;

    //Spawner Variables
    float spawnAlarm = 0;
    float spawnTime = 3f;

    //Define Reference Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
		
        //Deduct Spawn Alarm
        if(spawnAlarm-Time.deltaTime > 0)
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
