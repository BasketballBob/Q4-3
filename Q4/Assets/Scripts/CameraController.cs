using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //Reference Variables
    Transform trans;
    public GameObject followingObject;

    //Camera Variables
    public Vector2 DestPos;
    Vector2 OverrideDestPos;
    public float MinX = -10;
    public float MaxX = 30;
    float SpeedPercent = .02f;

    //Define Reference Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate () {

        //Set Camera Destination (Follows object unless overriden by "OverrideDestPos")
        if (OverrideDestPos != new Vector2())
        {
            DestPos = OverrideDestPos;
            OverrideDestPos = new Vector2();
        }
        else DestPos = new Vector2(followingObject.GetComponent<Transform>().position.x, DestPos.y);
		
        //Move Towards Destination
        if(new Vector2(trans.position.x, trans.position.y) != DestPos)
        {
            //Move Position
            trans.position += new Vector3(SpeedPercent * (DestPos.x - trans.position.x), 0); // SpeedPercent * (DestPos.y - trans.position.y)); (Y-AXIS CURRENTLY LOCKED)

            //Set Position Once Within Min Move
            /*if(Mathf.Abs(DestPos.x - trans.position.x) < PhysicsObject.minMove) //Horizontal
            {
                trans.position = new Vector3(DestPos.x, trans.position.y, trans.position.z);
            }
            if (Mathf.Abs(DestPos.y - trans.position.y) < PhysicsObject.minMove) //Vertical
            {
                trans.position = new Vector3(trans.position.y, DestPos.y, trans.position.z);
            }*/
        }

        //Clamp Camera Position
        if(trans.position.x < MinX)
        {
            trans.position = new Vector3(MinX, trans.position.y, trans.position.z);
        }
        if(trans.position.x > MaxX)
        {
            trans.position = new Vector3(MaxX, trans.position.y, trans.position.z);
        }

        //trans.position = new Vector3(DestPos.x, DestPos.y, trans.position.z);
	}

    public void CameraPosFollow(Vector3 DestPosition)
    {
        OverrideDestPos = DestPosition;
    }
}
