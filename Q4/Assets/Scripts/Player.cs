using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Reference Variables
    Transform trans;
    PhysicsObject po;

    //Player Variables
    float jumpSpeed = 10f;
    float moveSpeed = 3f;

    //Input Variables
    float xAxis = 0f;
    float yAxis = 0f;
    float xAxis2 = 0f;
    float yAxis2 = 0f;

    //Define Reference Variables
    void OnEnable()
    {
        trans = GetComponent<Transform>();
        po = GetComponent<PhysicsObject>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Input Variables
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);
        bool left = Input.GetKey(KeyCode.LeftArrow);

        //Controller Input Variables
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        xAxis2 = Input.GetAxis("Horizontal2");
        yAxis2 = Input.GetAxis("Vertical2");
        

        if(po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0))
        {
            //Debug.Log("REEEP");
        }

        //Jumping 
        if(up && po.PlaceMeeting(trans.position.x, trans.position.y-PhysicsObject.minMove, 0))
        {
            po.vSpeed = jumpSpeed;
        }

        //Horizontal Movement
        //if (right) po.hSpeed = moveSpeed;
        //else if (left) po.hSpeed = -moveSpeed;
        //else po.hSpeed = 0;

        //Controller Test Movement
        po.hSpeed = moveSpeed * xAxis;

       
	}

    Vector2 CalcVelocity(float xPercent, float yPercent, float speed)
    {
        float returnX = speed * (xPercent / (Mathf.Abs(xPercent) + Mathf.Abs(yPercent)));
        float returnY = speed * (yPercent / (Mathf.Abs(xPercent) + Mathf.Abs(yPercent)));

        return new Vector2(returnX, returnY);
    }
}
