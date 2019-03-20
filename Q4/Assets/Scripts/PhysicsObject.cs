using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    //Reference Variables
    public const float minMove = .001f;
    Transform trans;
    public Collider col;

    //Modifiable Variables
    public bool Gravity = false;
    public float GravRate = 20f;

    //Physics Object Variables
    int CollisionType = 0;
    public float hSpeed;
    public float vSpeed;

    //Define Reference Variables
    void OnEnable()
    {
        trans = GetComponent<Transform>();
        col = gameObject.AddComponent<Collider>();
        col.CollisionType = CollisionType;
    }

	// Update is called once per frame
	void Update () {
		
        //Horizontal Collision
        if(hSpeed != 0)
        {
            if(!col.PlaceMeeting(trans.position.x+hSpeed*Time.deltaTime, trans.position.y, 0))
            {
                trans.position += new Vector3(hSpeed * Time.deltaTime, 0);
            }
            else if(!col.PlaceMeeting(trans.position.x+minMove*Sign(hSpeed),trans.position.y,0))
            {
                do
                {
                    trans.position += new Vector3(minMove * Sign(hSpeed), 0);
                }
                while (!col.PlaceMeeting(trans.position.x + minMove * Sign(hSpeed), trans.position.y, 0));
            }

            //Cease HSpeed
            if(col.PlaceMeeting(trans.position.x + minMove * Sign(hSpeed), trans.position.y, 0))
            {
                hSpeed = 0;
            }
        }


        //Gravity
        if(Gravity && !col.PlaceMeeting(trans.position.x, trans.position.y-minMove, 0))
        {
            vSpeed -= GravRate * Time.deltaTime;
        }

        //Vertical Collision Management
        if(vSpeed != 0)
        {
            if(!col.PlaceMeeting(trans.position.x, trans.position.y+vSpeed*Time.deltaTime, 0))
            {
                trans.position += new Vector3(0, vSpeed) * Time.deltaTime;
            }
            else if(!col.PlaceMeeting(trans.position.x, trans.position.y+minMove*Sign(vSpeed),0))
            {
                do
                {
                    trans.position += new Vector3(0, minMove * Sign(vSpeed));
                }
                while (!col.PlaceMeeting(trans.position.x, trans.position.y + minMove * Sign(vSpeed), 0));
            }

            //Cease VSpeed
            if(col.PlaceMeeting(trans.position.x, trans.position.y + minMove * Sign(vSpeed), 0))
            {
                vSpeed = 0;
            }
        }
	}

    float Sign(float input)
    {
        if (input > 0) return 1;
        else if (input < 0) return -1;
        else return 0;
    }

    public bool PlaceMeeting(float xPos, float yPos, int collisionType)
    {
        //Call PlaceMeeting From Child Collider
        //(Used to make reference to collider unnecessary)
        return col.PlaceMeeting(xPos, yPos, collisionType);
    }
}
