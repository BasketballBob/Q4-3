using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Reference Variables
    Transform trans;
    SpriteRenderer sr;
    PhysicsObject po;
    TowerRepository tr;
    public GameObject ProjectileReference;
    public GameObject DirectionalReference;
    public GameObject DirectionalReference2;
    GameObject ConstructingTower;

    //Tower References
    GameObject[] TowerReferences;
    //GameObject tr_PeaShooter;

    //Player Variables
    public int Cash = 300;
    bool Suspended = false; //Equivalent to "Cutscene" var from "Functional Platformer" 
    float jumpSpeed = 10f;
    float bulletSpeed = 15f;

    //Input Variables
    bool LockAxis = false;
    bool LockAxis2 = false;
    float xAxis = 0f;
    float yAxis = 0f;
    float xAxis2 = 0f;
    float yAxis2 = 0f;
    float prevXAxis = 0f;
    float prevYAxis = 0f;
    float prevXAxis2 = 0f;
    float prevYAxis2 = 0f;
    float prevUpdateAlarm = 0;
    float prevUpdateTime = .025f;

    //Attacking Variables
    float attackAlarm = 0f;
    float attackTime = 1f;

    //Construction UI Vars
    bool TowerUIInitialized = false;
    GameObject[] TowerUIArray;
    float TowerUIOffSetDist = 2;
    int TowerArrayMax = 10;
    int TowerArrayCount = 5;
    float BuildAlarm = 0;
    float BuildTime = 2f;
    Color BuildRed = new Color(1f, 0f, 0f);
    Color BuildGreen = new Color(0f, 1f, 0f);
    float ReferenceAlpha = .5f;
    float UIAlpha = .5f;

    //Define Reference Variables
    void OnEnable()
    {
        //Basic Reference Variables
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        po = GetComponent<PhysicsObject>();
        tr = GetComponent<TowerRepository>();

        //Tower Reference Variables
        TowerReferences = new GameObject[TowerArrayMax];
        TowerReferences[0] = tr.tr_PeaShooter;
        TowerReferences[1] = tr.tr_LaserRay;
        TowerReferences[2] = tr.tr_LaserRay;
        TowerReferences[3] = tr.tr_PeaShooter;
        TowerReferences[4] = tr.tr_PeaShooter;
        TowerReferences[5] = tr.tr_PeaShooter;
        TowerReferences[6] = tr.tr_PeaShooter;
        TowerReferences[7] = tr.tr_PeaShooter;
        TowerReferences[8] = tr.tr_PeaShooter;
        TowerReferences[9] = tr.tr_PeaShooter;
    }

	// Use this for initialization
	void Start () {

        //Define Tower Array
        TowerUIArray = new GameObject[TowerArrayMax];
        for(int i = 0;i < TowerArrayMax; i++)
        {
            TowerUIArray[i] = new GameObject("Tower UI");
            TowerUIArray[i].AddComponent<SpriteRenderer>();
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        //Input Variables
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);
        bool left = Input.GetKeyDown(KeyCode.LeftArrow);

        //Controller Input Variables
        //NOTE: The var LockAxis is always reset to false as to automatically 
        //turn itself off once is stops recieving the signal to lock itself
        if (!LockAxis) //Recieve Input From Axis 1
        {
            xAxis = Input.GetAxis("Horizontal");
            yAxis = Input.GetAxis("Vertical");
        }
        else LockAxis = false;
        if (!LockAxis2) //Recieve Input From Axis 1
        {
            xAxis2 = Input.GetAxis("Horizontal2");
            yAxis2 = Input.GetAxis("Vertical2");
        }
        else LockAxis2 = false;
        bool TowerUI = Mathf.Round(Input.GetAxis("LeftTrigger")) > 0;
        bool BuildTower = Input.GetKey(KeyCode.Joystick1Button0);


        //TOWER UI (Testing)
        if (right && TowerArrayCount < 10) TowerArrayCount++;
        else if (left && TowerArrayCount > 0) TowerArrayCount--;
        //ManageTowerConstructionUI(true, TowerArrayCount);

        //TESTING PLAYER CURRENCY
        //Debug.Log(Cash);

        //Active Tower UI
        if (TowerUI && po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0))
        {
            ManageTowerConstructionUI(true, TowerArrayCount, BuildTower);
            Suspended = true;
        }
        //Deactivated Tower UI
        else
        {
            ManageTowerConstructionUI(false, TowerArrayCount, BuildTower);
            Suspended = false;

            //Destroy Unwanted Reference Tower
            if(ConstructingTower != null)
            {
                Destroy(ConstructingTower);
            }
        }

        //Test Tower Placement
        /*if (PlaceTower && po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0))
        {
            GameObject tvInst = Instantiate(tr_PeaShooter, trans.position, Quaternion.identity);
            tvInst.GetComponent<Transform>().position += new Vector3(0, tvInst.GetComponent<SpriteRenderer>().bounds.size.y / 2
            - GetComponent<Collider>().height/2);
            //Debug.Log(tvInst.GetComponent<SpriteRenderer>().bounds.size.y/2);
        }*/

        //Jumping 
        //if(up && po.PlaceMeeting(trans.position.x, trans.position.y-PhysicsObject.minMove, 0))
        //{
        //po.vSpeed = jumpSpeed;
        //CalcVelocity(.5f,1,100);
        //}

        //Controller Jumping (Detected by flicking)
        if (po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0) && !Suspended)
        {
            //if (Mathf.Round(xAxis) == 0 && Mathf.Round(prevXAxis) != 0
            //|| Mathf.Round(yAxis) == 0 && Mathf.Round(prevYAxis) != 0)
            if(Mathf.Round(Mathf.Sqrt(Mathf.Pow(xAxis,2)+Mathf.Pow(yAxis,2))) == 0
            && Mathf.Round(Mathf.Sqrt(Mathf.Pow(prevXAxis, 2) + Mathf.Pow(prevYAxis, 2))) != 0)
            {
                po.hSpeed = -CalcVelocity(prevXAxis, prevYAxis, jumpSpeed).x;
                po.vSpeed = -CalcVelocity(prevXAxis, prevYAxis, jumpSpeed).y;
            }
        }

        //Projectile Delay
        if (attackAlarm > 0) attackAlarm -= Time.deltaTime;
        else
        {
            attackAlarm = 0;
        }

        //Projectile Flinging (Also detected by flicking)
        if (attackAlarm == 0 && Mathf.Round(Mathf.Sqrt(Mathf.Pow(xAxis2, 2) + Mathf.Pow(yAxis2, 2))) == 0
        && Mathf.Round(Mathf.Sqrt(Mathf.Pow(prevXAxis2, 2) + Mathf.Pow(prevYAxis2, 2))) != 0 && !Suspended)
        {
            //Create Bullet
            GameObject tvInst = Instantiate(ProjectileReference, trans.position, Quaternion.identity); 
            tvInst.GetComponent<PhysicsObject>().hSpeed = -CalcVelocity(prevXAxis2, prevYAxis2, bulletSpeed).x;
            tvInst.GetComponent<PhysicsObject>().vSpeed = -CalcVelocity(prevXAxis2, prevYAxis2, bulletSpeed).y;

            //Set Attack Alarm 
            attackAlarm = attackTime;
        }

        //if (xAxis != prevXAxis) Debug.Log(xAxis + " " + prevXAxis);

        //Set Previous Axis Vars (Check For Flicking At Set Interval)
        if (prevUpdateAlarm > 0) prevUpdateAlarm -= Time.deltaTime;
        else
        {
            prevXAxis = xAxis;
            prevYAxis = yAxis;
            prevXAxis2 = xAxis2;
            prevYAxis2 = yAxis2;

            prevUpdateAlarm = prevUpdateTime;
        }


        //Horizontal Movement
        //if (right) po.hSpeed = moveSpeed;
        //else if (left) po.hSpeed = -moveSpeed;
        //else po.hSpeed = 0;

        //Controller Test Movement
        //po.hSpeed = moveSpeed * xAxis;


        //Manage (Debugging) Directional Reference
        DirectionalReference.GetComponent<Transform>().position = trans.position;
        DirectionalReference.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(yAxis, xAxis)/Mathf.PI)*180);
        DirectionalReference.GetComponent<SpriteRenderer>().sortingOrder = 1;

        DirectionalReference2.GetComponent<Transform>().position = trans.position;
        DirectionalReference2.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(yAxis2, xAxis2) / Mathf.PI) * 180);
        DirectionalReference2.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    void ManageTowerConstructionUI(bool Visible, int CurrentCount, bool Building)
    {
        //Initialize Variables
        if(!TowerUIInitialized)
        {
            //Define Tower UI Arrays
            for(int i = 0;i < TowerArrayMax;i++)
            {
                //Set Tower UI Sprites
                TowerUIArray[i].GetComponent<SpriteRenderer>().sprite = tr.TowerUISprites[0];
            }

            TowerUIInitialized = false;
        }

        //Manage UI Positions
        if(Visible)
        {
            //Determine Menu Selection Angle
            float SelectionAngle = ((Mathf.Atan2(yAxis, xAxis) / Mathf.PI) * 180); // + 180;
            SelectionAngle = (SelectionAngle + 360) % 360; //Use Modulo and OffSet to find only positive angles
            int SelectedInst = -1;
            float SelectDist = 360;

            //Manage All Active Instances
            for (int i = 0;i < CurrentCount;i++)
            {
                //Calculate Position (Relative To Player)
                float tvAngleOffSet = 90;
                float tvAngle = (i * (360 / CurrentCount) + tvAngleOffSet) % 360; //72 * i + tvAngleOffSet; 
                float UIX = TowerUIOffSetDist * Mathf.Cos((tvAngle/180) * Mathf.PI);
                float UIY = TowerUIOffSetDist * Mathf.Sin((tvAngle/180) * Mathf.PI);

                //Set Position
                TowerUIArray[i].GetComponent<Transform>().position = new Vector3(UIX + trans.position.x, 
                UIY + trans.position.y, TowerUIArray[i].GetComponent<Transform>().position.z);

                //Set Scale (Highlight Selected Circle)
                //if (Mathf.Abs(SelectionAngle - tvAngle) < (360 / CurrentCount) / 2
                //|| Mathf.Abs(SelectionAngle + (tvAngle-360)) < (360 / CurrentCount) / 2)
                //{
                //TowerUIArray[i].GetComponent<Transform>().localScale = new Vector3(2, 2, 2);
                //}
                //else TowerUIArray[i].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

                //Manage Scale 
                TowerUIArray[i].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

                //Manage Opacity
                SpriteRenderer tvSR = TowerUIArray[i].GetComponent<SpriteRenderer>();
                tvSR.color = new Color(tvSR.color.r, tvSR.color.g, tvSR.color.b, UIAlpha);

                //Find Selected Instance
                if (Mathf.Abs(SelectionAngle - tvAngle) < SelectDist)
                {
                    //Make Sure Input Exists
                    if (Mathf.Round(xAxis) != 0 || Mathf.Round(yAxis) != 0)
                    {
                        SelectedInst = i;
                        SelectDist = Mathf.Abs(SelectionAngle - tvAngle);
                    }
                }
            }

            //Determine Selected Instance (Manage Tower Reference)
            if(SelectedInst != -1)
            {
                TowerUIArray[SelectedInst].GetComponent<Transform>().localScale = new Vector3(2, 2, 2);
                
                //Dispose Of Inaccurate Reference
                if(ConstructingTower != null && ConstructingTower.GetComponent<SpriteRenderer>().sprite 
                != TowerReferences[SelectedInst].GetComponent<SpriteRenderer>().sprite)
                {
                    Destroy(ConstructingTower);
                }

                //Spawn Opaque Tower Reference
                if(ConstructingTower == null)
                {
                    ConstructingTower = Instantiate(TowerReferences[SelectedInst], trans.position, Quaternion.identity);
                    ConstructingTower.GetComponent<Tower>().Activated = false;
                }


                //Manage Active Tower Reference
                if(ConstructingTower != null)
                {
                    //Tower Reference Variables
                    Transform tvTrans = ConstructingTower.GetComponent<Transform>();
                    SpriteRenderer tvSR = ConstructingTower.GetComponent<SpriteRenderer>();
                    Tower tvTower = ConstructingTower.GetComponent<Tower>();

                    //Set Tower Position Relative To Ground
                    tvTrans.position = new Vector3(trans.position.x, trans.position.y - sr.bounds.size.y / 2 +
                    ConstructingTower.GetComponent<SpriteRenderer>().bounds.size.y / 2, tvTrans.position.z);

                    //Set Construction Color (Tell player if they are able to build the tower)
                    if (ConstructingTower.GetComponent<Collider>().PlaceMeeting(tvTrans.position.x, tvTrans.position.y, 3)
                    || ConstructingTower.GetComponent<Tower>().Cost > Cash)
                    {
                        tvTower.SetColor(new Color(BuildRed.r, BuildRed.g, BuildRed.b, tvSR.color.a));
                    }
                    else tvTower.SetColor(new Color(BuildGreen.r, BuildGreen.g, BuildGreen.b, tvSR.color.a));

                    //Construct Tower 
                    if (Building && !ConstructingTower.GetComponent<Collider>().PlaceMeeting(tvTrans.position.x, tvTrans.position.y, 3)
                    && Cash >= ConstructingTower.GetComponent<Tower>().Cost)
                    {
                        //Lock Axis 
                        LockAxis = true;
                        LockAxis2 = true;

                        //Construction Opacity (Transitioning Opacity and Transitiong Color (BuildGreen to White)
                        tvTower.SetColor(new Color(1 + (BuildGreen.r - 1) * (BuildAlarm) / BuildTime,
                        1 + (BuildGreen.g - 1) * (BuildAlarm) / BuildTime,
                        1 + (BuildGreen.b - 1) * (BuildAlarm) / BuildTime, 
                        ReferenceAlpha + (1-ReferenceAlpha) * (BuildTime-BuildAlarm) / BuildTime));

                        //Deduct Build Alarm
                        if (BuildAlarm-Time.deltaTime > 0)
                        {
                            BuildAlarm -= Time.deltaTime;
                        }
                        //Finish Constructing Tower
                        else
                        {
                            //Deduct Cash
                            Cash -= ConstructingTower.GetComponent<Tower>().Cost;

                            //Set Constructed Tower Variables
                            ConstructingTower.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                            ConstructingTower.GetComponent<Tower>().Activated = true;

                            //Detach Instance From Reference
                            ConstructingTower = null;
                           
                            //Reset Build Alarm
                            BuildAlarm = BuildTime;
                        }
                    }
                    //Default Values
                    else
                    {
                        //Default Opacity 
                        tvTower.SetColor(new Color(tvSR.color.r, tvSR.color.g, tvSR.color.b, ReferenceAlpha));

                        //Reset Construction Alarm
                        BuildAlarm = BuildTime;
                    }
                }

            }
            //Rid Of Unused Tower Reference
            else if(ConstructingTower != null)
            {
                Destroy(ConstructingTower);
            }
        }

        //Activated / Deactivated UI
        for(int i = 0;i < TowerArrayMax;i++)
        {
            //Activate Instances
            if (Visible && i < CurrentCount && !TowerUIArray[i].activeSelf)
            {
                TowerUIArray[i].SetActive(true);
            }
            //Deactivate Instance
            else if(!Visible && TowerUIArray[i].activeSelf ||
            i >= CurrentCount && TowerUIArray[i].activeSelf)
            {
                TowerUIArray[i].SetActive(false);
            }
        }

    }

    Vector2 CalcVelocity(float xPercent, float yPercent, float speed)
    {
        //This Equation Returns A Velocity With A Magnitude of "speed" based on the ratio
        //between xPercent and yPercent

        float returnX = speed * (xPercent / (Mathf.Abs(xPercent) + Mathf.Abs(yPercent)));
        float returnY = speed * (yPercent / (Mathf.Abs(xPercent) + Mathf.Abs(yPercent)));

        //Edgecase: Cannot find returnX/Y accurately to speed without finding the ratio
        //of return hypotenuse to speed (Don't entirely understand)
        float initialSpeed = Mathf.Sqrt(Mathf.Pow(returnX, 2) + Mathf.Pow(returnY, 2));
        returnX *= speed / initialSpeed;
        returnY *= speed / initialSpeed;

        //Avoid Returning Null
        if (double.IsNaN((double)returnX)) returnX = 0;
        if (double.IsNaN((double)returnY)) returnY = 0;

        //Debug.Log("1 " + speed + " 2 " + Mathf.Sqrt(Mathf.Pow(returnX, 2) + Mathf.Pow(returnY, 2)));

        return new Vector2(returnX, returnY);
    }
}
