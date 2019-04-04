using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : Tower
{

    //Reference Variables


    //Tower Base Variables

    
    //Define Reference Variables
    public override void OnEnable()
    {
        //Run Parent Event
        base.OnEnable();
    }

    //Add Additional Components
    public override void Start()
    {
        //Run Parent Start
        base.Start();


        //Color Children
        SetColor(sr.color);
    }

    // Update is called once per frame
    public override void Update()
    {
        //Run Parent Update
        base.Update();

        //Specialized Code (Different for each turret)


        //Fire At Target 
        if (target != null)
        {

            //Check If Target Is Within Range
            if (Collider.TransDist(trans.position + ShotOffset, target.GetComponent<Transform>().position) <= Range)
            {
                //Attack Target
                if (AttackAlarm <= 0)
                {
                    
                }
            }

        }
    }//Update 

    private void OnDestroy()
    {
        //Destroy Extra Tower Components

    }
}
