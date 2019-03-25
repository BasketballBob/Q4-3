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

        //Set Enemy Follow Pos
        Enemy.FollowPos = new Vector2(BasePos.position.x, BasePos.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		
        //Damage Base On Enemy Collision
        if(BaseCol.PlaceMeeting(BasePos.position.x, BasePos.position.y, 2))
        {
            //Delete Colliding Enemy Instances (and deal damage)
            //do
            //{
                Destroy(BaseCol.InstanceMeeting(BasePos.position.x, BasePos.position.y, 2));
                Health -= 1;
            //}
            //while (BaseCol.PlaceMeeting(BasePos.position.x, BasePos.position.y, 2));
        }

        //Lose Game (One Base Out Of Health)
        if(Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}
}
