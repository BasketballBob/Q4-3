using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : Enemy
{

    //Reference Variables


    //Enemy Base Variables


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

        //Flip Sprite Renderer
        sr.flipX = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        //Run Parent Update
        base.Update();

        //Manage Animations 
        anim.Play("FastBaby_walkCycle");
        
        //anim.Play("BabyDeathAnimation");

    }//UPDATE

    private void OnDestroy()
    {

        //Destroy Extra Enemy Components

    }
}
