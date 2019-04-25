using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Reference Variables
    [System.NonSerialized] public Transform trans;
    [System.NonSerialized] public SpriteRenderer sr;
    [System.NonSerialized] public PhysicsObject po;
    [System.NonSerialized] public Animator anim;

    //Modifiable Variables
    public bool FollowAI = true;
    public bool CanJump = true;

    //Enemy Variables
    public int health = 1;
    public float moveSpeed;
    public float jumpSpeed;
    public static Vector2 FollowPos;


    //Define Reference Vars
    public virtual void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        po = GetComponent<PhysicsObject>();
        anim = GetComponent<Animator>();
    }

    //Initialize Variables
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update ()
    {
        
        //Following AI
        if(FollowAI)
        {
            if(po.Sign(trans.position.x-FollowPos.x) == po.Sign(trans.position.x-FollowPos.x+moveSpeed*Time.deltaTime*po.Sign(FollowPos.x - trans.position.x)))
            {
                po.hSpeed = moveSpeed * po.Sign(FollowPos.x - trans.position.x);

                //Jump If Obstacle In Way
                if(po.PlaceMeeting(trans.position.x+PhysicsObject.minMove*po.Sign(FollowPos.x - trans.position.x), trans.position.y, 0)
                && po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0) && CanJump)
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
