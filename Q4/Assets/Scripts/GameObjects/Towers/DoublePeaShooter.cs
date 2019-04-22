using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePeaShooter : Tower
{

    //Reference Variables
    public Sprite HeadSprite;
    GameObject TurretHead;
    GameObject TurretHead2;

    //Pea Shooter Variables
    public Vector2 HeadPos; //OffSet from Parent Pos
    public Vector2 HeadPos2;
    int FiringHead = 0;
    int FiringHeadMin = 0;
    int FiringHeadMax = 1;

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

        //Create Second Turret Head 
        TurretHead2 = new GameObject();
        TurretHead2.GetComponent<Transform>().position = new Vector3(trans.position.x + HeadPos2.x, trans.position.y + HeadPos2.y, trans.position.z);
        TurretHead2.AddComponent<SpriteRenderer>();
        TurretHead2.GetComponent<SpriteRenderer>().sprite = HeadSprite;
        TurretHead2.GetComponent<SpriteRenderer>().sortingOrder = -4;
        Children.Add(TurretHead2);

        //Color Children
        SetColor(sr.color);
    }

    // Update is called once per frame
    public override void Update()
    {
        //Run Parent Update
        base.Update();

        //Specialized Code (Different for each turret)
        if (target != null)
        {
            //Aim Head 1
            float tvDistX = target.GetComponent<Transform>().position.x - (HeadPos.x + trans.position.x);
            float tvDistY = target.GetComponent<Transform>().position.y - (HeadPos.y + trans.position.y);
            TurretHead.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(tvDistY, tvDistX) / Mathf.PI) * 180);

            //Aim Head 2 
            tvDistX = target.GetComponent<Transform>().position.x - (HeadPos2.x + trans.position.x);
            tvDistY = target.GetComponent<Transform>().position.y - (HeadPos2.y + trans.position.y);
            TurretHead2.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(tvDistY, tvDistX) / Mathf.PI) * 180);
        }

        //Fire At Target 
        if (target != null)
        {

            //Check If Target Is Within Range
            if (Collider.TransDist(trans.position, target.GetComponent<Transform>().position) <= Range)
            {
                //Set Shot Position 
                Vector3 ShotPos = new Vector3();
                if (FiringHead == 0) ShotPos = HeadPos;
                else if (FiringHead == 1) ShotPos = HeadPos2;

                //Attack Target
                if (AttackAlarm <= 0)
                {
                    //Fire Weapon
                    GameObject tvObj = Instantiate(projectile, trans.position + ShotPos, Quaternion.identity);
                    tvObj.GetComponent<PhysicsObject>().Gravity = false;

                    //Calculate Trajectory
                    float xDist = target.GetComponent<Transform>().position.x - (trans.position.x + ShotPos.x);
                    float yDist = target.GetComponent<Transform>().position.y - (trans.position.y + ShotPos.y);
                    float initialMag = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));
                    float initalHSpeedRatio = xDist / (Mathf.Abs(xDist) + Mathf.Abs(yDist));
                    float initalVSpeedRatio = yDist / (Mathf.Abs(xDist) + Mathf.Abs(yDist));

                    tvObj.GetComponent<PhysicsObject>().hSpeed = BulletSpeed *
                    initalHSpeedRatio * (initialMag / Collider.TransDist(trans.position + ShotPos, target.GetComponent<Transform>().position));
                    tvObj.GetComponent<PhysicsObject>().vSpeed = BulletSpeed *
                    initalVSpeedRatio * (initialMag / Collider.TransDist(trans.position + ShotPos, target.GetComponent<Transform>().position));

                    //Switch Weapon Firing 
                    if (FiringHead < FiringHeadMax)
                    {
                        FiringHead++;
                    }
                    else FiringHead = FiringHeadMin;


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
        Destroy(TurretHead2);
    }
}
