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
    float prevXAxis = 0f;
    float prevYAxis = 0f;

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
        //if(up && po.PlaceMeeting(trans.position.x, trans.position.y-PhysicsObject.minMove, 0))
        //{
            //po.vSpeed = jumpSpeed;
            //CalcVelocity(.5f,1,100);
        //}

        //Controller Jumping (Detected by flicking)
        if(Mathf.Round(xAxis) == 0 && Mathf.Round(prevXAxis) != 0
        || Mathf.Round(yAxis) == 0 && Mathf.Round(prevYAxis) != 0)
        {
            po.hSpeed = -CalcVelocity(prevXAxis, prevYAxis, 10f).x;
            po.vSpeed = -CalcVelocity(prevXAxis, prevYAxis, 10f).y;
        }

        if(xAxis != prevXAxis) Debug.Log(xAxis + " " + prevXAxis);
        //Set Previous Axis Vars
        prevXAxis = xAxis;
        prevYAxis = yAxis;        

        //Horizontal Movement
        //if (right) po.hSpeed = moveSpeed;
        //else if (left) po.hSpeed = -moveSpeed;
        //else po.hSpeed = 0;

        //Controller Test Movement
        //po.hSpeed = moveSpeed * xAxis;

       
	}

    Vector2 CalcVelocity(float xPercent, float yPercent, float speed)
    {
        //This Equation Returns A Velocity With A Magnitude of "speed" based on the ratio
        //between xPercent and yPercent

        float returnX = speed * (xPercent / (Mathf.Abs(xPercent) + Mathf.Abs(yPercent)));
        float returnY = speed * (yPercent / (Mathf.Abs(xPercent) + Mathf.Abs(yPercent)));

        //Edgecase: Cannot find returnX/Y accurately to speed without finding the ratio
        //of return hypotenuse to speed (Don't entirely understand)
        float initialSpeed = Mathf.Sqrt(Mathf.Pow(returnX, 2) + Mathf.Pow(returnY, 2));
        returnX *= speed / initialSpeed;
        returnY *= speed / initialSpeed;

        //Avoid Returning Null
        if (double.IsNaN((double)returnX)) returnX = 0;
        if (double.IsNaN((double)returnY)) returnY = 0;

        //Debug.Log("1 " + speed + " 2 " + Mathf.Sqrt(Mathf.Pow(returnX, 2) + Mathf.Pow(returnY, 2)));

        return new Vector2(returnX, returnY);
    }
}
