using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //Reference Variables
    Transform trans;
    public GameObject followingObject;

    //Camera Variables
    public Vector2 DestPos;
    float SpeedPercent = .02f;

    //Define Reference Variables
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
		
        //Move Towards Destination
        if(new Vector2(trans.position.x, trans.position.y) != DestPos)
        {

        }
	}
}
