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

        if(po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0))
        {
            Debug.Log("REEEP");
        }

        //Jumping 
        if(up && po.PlaceMeeting(trans.position.x, trans.position.y-PhysicsObject.minMove, 0))
        {
            po.vSpeed = jumpSpeed;
        }

        //Horizontal Movement
        if (right) po.hSpeed = moveSpeed;
        else if (left) po.hSpeed = -moveSpeed;
        else po.hSpeed = 0;
	}
}
