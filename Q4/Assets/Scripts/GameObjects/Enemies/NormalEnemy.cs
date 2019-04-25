using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
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

        //Control Animations
        anim.Play("NormalBabyWalk");


    }//UPDATE

    private void OnDestroy()
    {
        //Destroy Extra Enemy Components

    }
}
