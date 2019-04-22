using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Reference Variables
    public GameObject Base;
    Transform BasePos;
    Collider BaseCol;
    EnemyRepository er;

    //Game Manager Variables
    public static int Health = 10;
    public const int HealthCap = 10;

    //Wave Manager Variablesx
    SemiWave[] SpawnWave;
    public Vector2 SpawnPos;
    int SpawnWaveCap = 100;
    [SerializeField] int SpawnWavePos = 0;
    int PrevSpawnWavePos = 0;
    [SerializeField] int SemiWavePos = 0;
    [SerializeField] float SpawnAlarm = 0;
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
            Defined = true;
        }
    }


    //Define Reference Variables
    private void OnEnable()
    {
        //Define General Reference Variables
        er = GetComponent<EnemyRepository>();

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

        //Reset Health
        Health = HealthCap;

        //Test Wave Spawning 
        SpawnWave[0] = new SemiWave(er.FastBaby, 10, 2f);
        SpawnWave[1] = new SemiWave(er.NormalBaby, 10, 1f);
        SpawnWave[2] = new SemiWave(er.FastBaby, 1000, .75f);

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
  
            //Spawn Existing Semiwaves
            if (SpawnWave[SpawnWavePos].Defined == true)
            {
                //Begin New Semiwave
                if(SpawnWavePos != PrevSpawnWavePos)
                {
                    SpawnAlarm = SpawnWave[SemiWavePos].DeployRate;
                    SemiWavePos = 0;
                }
                PrevSpawnWavePos = SpawnWavePos;

                //Deduct Semiwave Alarm
                if (SpawnAlarm-Time.deltaTime > 0)
                {
                    SpawnAlarm -= Time.deltaTime;
                }
                //Spawn Enemy 
                else
                {                  
                    //Advance SemiWave Pos 
                    if(SemiWavePos < SpawnWave[SpawnWavePos].DeployCount)
                    {
                        //Spawn Enemy
                        GameObject tvInst = Instantiate(SpawnWave[SpawnWavePos].EnemyObject);
                        tvInst.GetComponent<Transform>().position = SpawnPos;

                        //Reset Alarm
                        SpawnAlarm = SpawnWave[SpawnWavePos].DeployRate;

                        //Move On To Next Instance To Spawn
                        SemiWavePos++;
                    }
                    else
                    {
                        //Turn Off Used Semiwave
                        SpawnWave[SpawnWavePos].Defined = false;

                        //Move On To Next Semiwave
                        SpawnWavePos++;
                    }
                }
            }
        
        }//MANAGE ENEMY SPAWNING
	}
}
