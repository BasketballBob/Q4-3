using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Tower
{

    //Reference Variables
    Collider Collider2;
    Animator anim;

    //Tower Base Variables
    bool ColliderAdded = false;
    [SerializeField] int HealthCount = 3;
    [SerializeField] float Health;
    float HealthAmount = 4f; //Time in seconds of destruction
    float HealthRiseRate = .5f; //Multiplier
    

    //Define Reference Variables
    public override void OnEnable()
    {
        //Run Parent Event
        base.OnEnable();

        anim = GetComponent<Animator>();
    }

    //Add Additional Components
    public override void Start()
    {
        //Run Parent Start
        base.Start();

        //Set Initial Health
        Health = HealthAmount * (float)HealthCount;

        //Color Children
        SetColor(sr.color);
    }

    // Update is called once per frame
    public override void Update()
    {
        //Run Parent Update
        base.Update();

        //Add Wall Collider Component (if activated)
        if(Activated && !ColliderAdded)
        {
            Collider2 = gameObject.AddComponent<Collider>();
            Collider2.CollisionType = 4;
            Collider2.width = 1;
            Collider2.height = 3;
            ColliderAdded = true;

            //Kill All Enemies Inside
            while(Collider2.PlaceMeeting(trans.position.x, trans.position.y, 2))
            {
                Destroy(col.NearestCollider(trans.position.x, trans.position.y, 2));
            }
        }


        //Fire At Target 
        if (target != null)
        {

            //Check If Target Is Within Range
            /*if (Collider.TransDist(trans.position, target.GetComponent<Transform>().position) <= Range)
            {
                //Attack Target
                if (AttackAlarm <= 0)
                {

                }
            }*/
        }

        //Detect If Enemy Is Colliding With Wall
        if(Collider2.PlaceMeeting(trans.position.x-PhysicsObject.minMove, trans.position.y, 2))
        {
            //Deduct From Health 
            if (Health - Time.deltaTime > 0)
            {
                Health -= Time.deltaTime;
            }
            else Health = 0;

            //Deduct From Health Count
            if((HealthCount-1)*HealthAmount > Health)
            {
                HealthCount -= 1;
            }
        }
        else
        {
            //Rise Health
            if(Health + HealthRiseRate*Time.deltaTime < HealthCount * (float)HealthAmount)
            {
                Health += HealthRiseRate * Time.deltaTime;
            }
            else Health = HealthCount * (float)HealthAmount;
        }

        //Destroy Tower If Broken
        if(HealthCount <= 0)
        {
            Destroy(gameObject);
        }

        //Set Wall Sprites
        if (HealthCount == 3) anim.Play("WallPhase3");
        else if (HealthCount == 2) anim.Play("WallPhase2");
        else if (HealthCount == 1) anim.Play("WallPhase1");

      
    }//Update 

    private void OnDestroy()
    {
        //Destroy Extra Tower Components

    }
}
