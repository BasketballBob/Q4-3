using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Reference Variables
    Transform trans;
    PhysicsObject po;

    //Modifiable Variables
    public bool FollowAI = false;

    //Enemy Variables
    public int health;
    public float moveSpeed;
    public float jumpSpeed;
    public static Vector2 FollowPos;

    //Define Reference Vars
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        po = GetComponent<PhysicsObject>();
    }

    // Update is called once per frame
    void Update () {
		
        //Following AI
        if(FollowAI)
        {
            if(po.Sign(trans.position.x-FollowPos.x) == po.Sign(trans.position.x-FollowPos.x+moveSpeed*Time.deltaTime*po.Sign(FollowPos.x - trans.position.x)))
            {
                po.hSpeed = moveSpeed * po.Sign(FollowPos.x - trans.position.x);

                //Jump If Obstacle In Way
                if(po.PlaceMeeting(trans.position.x+PhysicsObject.minMove*po.Sign(FollowPos.x - trans.position.x), trans.position.y, 0)
                && po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0))
                {
                    po.vSpeed = jumpSpeed;
                }
            }
            else
            {
                trans.position = new Vector3(FollowPos.x, trans.position.y, trans.position.z);
                po.hSpeed = 0;
            }
        } 

        //Die Once Out Of Health
        if(health <= 0)
        {
            Destroy(gameObject);
        }
	}
}
