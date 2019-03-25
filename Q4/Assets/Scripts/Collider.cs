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
    // 3 - Tower

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

    public GameObject NearestCollider(int collisionType)
    {
        //Initialize Variables
        GameObject nearestInst = null;
        float closestDist = Mathf.Pow(100, 3); //(Big Number)

        //Check All Colliders Instances
        foreach(Collider element in ColliderList)
        {
            //Check For Collision Type
            if(element.CollisionType == collisionType)
            {
                //Make Sure Not To Check Self
                if (element != this)
                {
                    //Reference Variables
                    Transform inputTrans = element.GetComponent<Transform>();

                    nearestInst = element.gameObject;

                    //Check If Closest Instance
                    if (Mathf.Sqrt(Mathf.Pow(trans.position.x - inputTrans.position.x, 2) + Mathf.Pow(trans.position.y - inputTrans.position.y, 2)) < closestDist)
                    {
                        nearestInst = element.gameObject;
                        closestDist = Mathf.Sqrt(Mathf.Pow(trans.position.x - inputTrans.position.x, 2) + Mathf.Pow(trans.position.y - inputTrans.position.y, 2));
                    }
                }
            }
        }

        //Return Nearest Inst
        return nearestInst;
    }

    public static float TransDist(Vector3 trans1, Vector3 trans2)
    {
        //Initialize Variables
        float x1 = trans1.x;
        float y1 = trans1.y;
        float x2 = trans2.x;
        float y2 = trans2.y;

        //Return Magnitude (Distance)
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
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
