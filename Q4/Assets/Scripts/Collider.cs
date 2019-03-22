using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour {

    //Global List Variable
    static List<Collider> ColliderList = new List<Collider>();

    //Reference Variables
    Transform trans;
    SpriteRenderer sr;

    //Collision Types:
    //-1 - Nothing 
    // 0 - Wall
    // 1 - Player
    // 2 - Enemy

    //Collider Variables 
    public int CollisionType;
    public float width;
    public float height;

    //Define Reference Variables
    void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {

        //Add Self To Collider List
        ColliderList.Add(this);

        //Set Automatic Dimensions
        if (sr != null)
        {
            width = sr.bounds.size.x;
            height = sr.bounds.size.y;
        }
    }
	
	// Update is called once per frame
	void Update () {
		

        //Debug
        if(PlaceMeeting(trans.position.x, trans.position.y, 0))
        {
            //Debug.Log("BRRRE");
        }
	}

    //Destroy List Reference
    void OnDestroy()
    {
        ColliderList.Remove(this);
    }

    public bool PlaceMeeting(float xPos, float yPos, int collisionType)
    {
        //Initialize Variables
        bool returnVal = false;

        //Set Initial Position
        Vector3 prevPos = this.trans.position;
        this.trans.position = new Vector3(xPos, yPos);


        //Check For Collision With All Colliders
        foreach(Collider element in ColliderList)
        {
            //Check To See If Element Exists
            if (element != null)
            {
                //Avoid Collision With Self
                if (this != element)
                {
                    //Check Only For Input Collider Type
                    if (element.CollisionType == collisionType)
                    {
                        //Rectangular Collision
                        if (RectToRect(this, element))
                        {
                            returnVal = true;
                            break;
                        }
                    }
                }
            }
            //Remove Null Element
            else
            {
                ColliderList.Remove(element);
            }
        }


        //Reset Position
        this.trans.position = prevPos;

        //Return Value
        return returnVal;
    }

    public GameObject InstanceMeeting(float xPos, float yPos, int collisionType)
    {
        //Initialize Variables
        GameObject returnVal = null;

        //Set Initial Position
        Vector3 prevPos = this.trans.position;
        this.trans.position = new Vector3(xPos, yPos);


        //Check For Collision With All Colliders
        foreach (Collider element in ColliderList)
        {
            //Check To See If Element Exists
            if (element != null)
            {
                //Avoid Collision With Self
                if (this != element)
                {
                    //Check Only For Input Collider Type
                    if (element.CollisionType == collisionType)
                    {
                        //Rectangular Collision
                        if (RectToRect(this, element))
                        {
                            returnVal = element.gameObject;
                            break;
                        }
                    }
                }
            }
            //Remove Null Element
            else
            {
                ColliderList.Remove(element);
            }
        }


        //Reset Position
        this.trans.position = prevPos;

        //Return Value
        return returnVal;
    }

    bool RectToRect(Collider Rect1, Collider Rect2)
    {
        //Check To See If Rectangles Overlapping
        if(Mathf.Min(Rect1.trans.position.x + Rect1.width / 2, Rect2.trans.position.x + Rect2.width / 2) 
        > Mathf.Max(Rect1.trans.position.x - Rect1.width / 2, Rect2.trans.position.x - Rect2.width / 2)
        && Mathf.Min(Rect1.trans.position.y + Rect1.height / 2, Rect2.trans.position.y + Rect2.height / 2) 
        > Mathf.Max(Rect1.trans.position.y - Rect1.height / 2, Rect2.trans.position.y - Rect2.height / 2))
        {
            return true;
        }

        //Return Nothing If Nothing 
        return false;
    }
}
