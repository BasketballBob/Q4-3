using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Reference Variables
    public GameObject Base;
    Transform BasePos;
    Collider BaseCol;

    //Game Manager Variables
    public int Health = 10;
    int HealthCap;

    //Wave Manager Variables
    SemiWave[] SpawnWave;
    int SpawnWaveCap = 100;
    int SpawnWavePos = 0;
    int PrevSpawnWavePos = 0;
    int SemiWavePos = 0;
    float SpawnAlarm = 0;
    public struct SemiWave
    {
        public GameObject EnemyObject;
        public int DeployCount;
        public float DeployRate;
        public bool Defined;

        public SemiWave(GameObject enemyObject, int deployCount, float deployRate)
        {
            //Define Input Variables
            EnemyObject = enemyObject;
            DeployCount = deployCount;
            DeployRate = deployRate;

            //Set Variable Defaults
            Defined = false;
        }
    }


    //Define Reference Variables
    private void OnEnable()
    {

        //Initialize Base Object
        if (Base.GetComponent<Collider>() == null)
        {
            Base.AddComponent<Collider>();
        }
        BasePos = Base.GetComponent<Transform>();
        BaseCol = Base.GetComponent<Collider>();
        BaseCol.CollisionType = -1;
    }

    // Use this for initialization
    void Start () {

        //Define SpawnWave Array
        SpawnWave = new SemiWave[SpawnWaveCap];

        //Set Enemy Follow Pos
        Enemy.FollowPos = new Vector2(BasePos.position.x, BasePos.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		
        //Damage Base On Enemy Collision
        if(BaseCol.PlaceMeeting(BasePos.position.x, BasePos.position.y, 2))
        {
            Destroy(BaseCol.InstanceMeeting(BasePos.position.x, BasePos.position.y, 2));
            Health -= 1;
        }

        //Lose Game (One Base Out Of Health)
        if(Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        //Manage Enemy Spawning
        if(SpawnWave != null)
        {
            //Set Previous Variables
            PrevSpawnWavePos = SpawnWavePos;

            //Spawn Existing Semiwaves
            if (SpawnWave[SpawnWavePos].Defined == true)
            {
                //Begin New Semiwave
                if(SpawnWavePos != PrevSpawnWavePos)
                {
                    SpawnAlarm = SpawnWave[SemiWavePos].DeployRate;
                    SemiWavePos = 0;
                }

                //Deduct Semiwave Alarm
                if(SpawnAlarm-Time.deltaTime > 0)
                {
                    SpawnAlarm -= Time.deltaTime;
                }
                //Spawn Enemy 
                else
                {

                    //Advance SemiWave Pos 
                    if(SemiWavePos < SpawnWave[SpawnWavePos].DeployCount)
                    {
                        SemiWavePos++;
                    }
                    else
                    {
                        SpawnWavePos++;
                    }
                }
            }
        }
	}
}
