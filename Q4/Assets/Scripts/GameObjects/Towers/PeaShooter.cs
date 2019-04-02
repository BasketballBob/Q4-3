using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Tower
{

    //Reference Variables
    public Sprite HeadSprite;
    GameObject TurretHead;

    //Pea Shooter Variables
    public Vector2 HeadPos; //OffSet from Parent Pos

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

        //Create Turret Head
        TurretHead = new GameObject();
        TurretHead.GetComponent<Transform>().position = new Vector3(trans.position.x + HeadPos.x, trans.position.y + HeadPos.y, trans.position.z);
        TurretHead.AddComponent<SpriteRenderer>();
        TurretHead.GetComponent<SpriteRenderer>().sprite = HeadSprite;
        TurretHead.GetComponent<SpriteRenderer>().sortingOrder = -4;
        Children.Add(TurretHead);
    }

    // Update is called once per frame
    public override void Update()
    {
        //Run Parent Update
        base.Update();

        //Specialized Code (Different for each turret)
        if (target != null)
        {
            float tvDistX = target.GetComponent<Transform>().position.x - trans.position.x;
            float tvDistY = target.GetComponent<Transform>().position.y - trans.position.y;
            TurretHead.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(tvDistY, tvDistX) / Mathf.PI) * 180);
        }

        //Fire At Target 
        if (target != null)
        {

            //Check If Target Is Within Range
            if (Collider.TransDist(trans.position + ShotOffset, target.GetComponent<Transform>().position) <= Range)
            {
                //Attack Target
                if (AttackAlarm <= 0)
                {
                    //Fire Weapon
                    GameObject tvObj = Instantiate(projectile, trans.position + ShotOffset, Quaternion.identity);
                    tvObj.GetComponent<PhysicsObject>().Gravity = false;

                    //Calculate Trajectory
                    float xDist = target.GetComponent<Transform>().position.x - (trans.position.x + ShotOffset.x);
                    float yDist = target.GetComponent<Transform>().position.y - (trans.position.y + ShotOffset.y);
                    float initialMag = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));
                    float initalHSpeedRatio = xDist / (Mathf.Abs(xDist) + Mathf.Abs(yDist));
                    float initalVSpeedRatio = yDist / (Mathf.Abs(xDist) + Mathf.Abs(yDist));

                    tvObj.GetComponent<PhysicsObject>().hSpeed = BulletSpeed *
                    initalHSpeedRatio * (initialMag / Collider.TransDist(trans.position + ShotOffset, target.GetComponent<Transform>().position));
                    tvObj.GetComponent<PhysicsObject>().vSpeed = BulletSpeed *
                    initalVSpeedRatio * (initialMag / Collider.TransDist(trans.position + ShotOffset, target.GetComponent<Transform>().position));


                    //Reset Alarm
                    AttackAlarm = AttackTime;
                }
            }

        }
    }//Update 

    private void OnDestroy()
    {
        //Destroy Extra Tower Components
        Destroy(TurretHead);
    }
}
