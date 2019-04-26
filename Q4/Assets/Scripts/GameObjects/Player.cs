using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    //Reference Variables
    Transform trans;
    SpriteRenderer sr;
    PhysicsObject po;
    Collider col;
    TowerRepository tr;
    CameraController playerCamera;
    public Sprite GranSprite;
    GameObject GranRef;
    public Sprite GranHandSprite;
    GameObject GranHand;
    public Sprite GranArmSprite;
    GameObject GranArm;
    public GameObject ProjectileReference;
    public GameObject DirectionalReference = null;
    public GameObject DirectionalReference2 = null;
    GameObject ConstructingTower;
    GameObject EditingTower;

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
    bool TowerUI;
    bool prevTowerUI;
    bool EditUI;
    bool prevEditUI;
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
    bool TowerUIInitialized;
    bool ResetUI;
    GameObject[] TowerUIArray;
    float TowerUIOffSetDist = 2;
    int TowerArrayMax = 10;
    int TowerArrayCount = 5;
    int SelectedInst = -1; //Set by ManageRadialUI() and used to determine the selected option
    int LastActive = 0;
    bool BuildActive;
    bool BuildPrev;
    bool OptionPossible; //(Used for tower editing) 
    float BuildAlarm = 0;
    float BuildTime = 2f;
    bool EditActive;
    bool EditPrev;
    float EditAlarm = 0;
    float EditTime = 2f;
    Color BuildRed = new Color(1f, 0f, 0f);
    Color BuildGreen = new Color(0f, 1f, 0f);
    float ReferenceAlpha = .5f;
    float UIAlpha = .5f;

    //Granny Animation Variables
    Vector2 GranOrigin = new Vector2(0, .5f);
    Vector2 GranHandPos = new Vector2(0, .4f);
    Vector2 GranArmPos1 = new Vector2(0, .4f); //(OffSet from PlayerPos)
    Vector2 GranArmPos2 = new Vector2(.4f, 0); //(OffSet from GranOrigin)
    float GranDist = .5f;
    float GranArmDir = 1f;
    float GranArmLength = 1f;

    //Define Reference Variables
    void OnEnable()
    {
        //Basic Reference Variables
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        po = GetComponent<PhysicsObject>();
        col = GetComponent<Collider>();
        tr = GetComponent<TowerRepository>();
        playerCamera = GameObject.Find("Player Camera")
        .GetComponent<CameraController>();

        //Tower Reference Variables
        TowerReferences = new GameObject[TowerArrayMax];
        TowerReferences[0] = tr.tr_PeaShooter;
        TowerReferences[1] = tr.tr_LaserRay;
        TowerReferences[2] = tr.tr_Wall;
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

        //Create Granny Object
        GranRef = new GameObject();
        GranRef.AddComponent<SpriteRenderer>();
        GranRef.GetComponent<SpriteRenderer>().sprite = GranSprite;
        GranRef.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder-1;
        //GranRef.GetComponent<SpriteRenderer>().color = new Color(1f, .25f, 1f, 1f);

        //Create Granny Hand
        GranHand = new GameObject();
        GranHand.AddComponent<SpriteRenderer>();
        GranHand.GetComponent<SpriteRenderer>().sprite = GranHandSprite;
        GranHand.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder;

        //Create Granny Arm
        GranArm = new GameObject();
        GranArm.AddComponent<SpriteRenderer>();
        GranArm.GetComponent<SpriteRenderer>().sprite = GranArmSprite;
        GranArm.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder-1;
        GranArmLength = GranArm.GetComponent<SpriteRenderer>().bounds.size.x;

        //Define Tower Array
        TowerUIArray = new GameObject[TowerArrayMax];
        for(int i = 0;i < TowerArrayMax; i++)
        {
            TowerUIArray[i] = new GameObject("Tower UI");
            TowerUIArray[i].AddComponent<SpriteRenderer>();
            TowerUIArray[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        
	}

    //DEBUGGING UI
    private void OnGUI()
    {
        GUIStyle testStyle = new GUIStyle();
        testStyle.fontSize = 50;
        GUI.Label(new Rect(100, 100, 200, 50), Cash.ToString(), testStyle);
    }

    // Update is called once per frame
    void Update () {

        //Input Variables
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);
        bool left = Input.GetKeyDown(KeyCode.LeftArrow);

        //DEVELOPMENT COMMANDS
        if(Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Credits");
        }

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
        prevTowerUI = TowerUI;
        prevEditUI = EditUI;
        TowerUI = Mathf.Round(Input.GetAxis("LeftTrigger")) > 0;
        EditUI = Mathf.Round(Input.GetAxis("RightTrigger")) > 0;
        bool BuildTower = Input.GetKey(KeyCode.Joystick1Button0);


        //TOWER UI (Testing)
        if (right && TowerArrayCount < 10) TowerArrayCount++;
        else if (left && TowerArrayCount > 0) TowerArrayCount--;
        //ManageTowerConstructionUI(true, TowerArrayCount);

        //TESTING PLAYER CURRENCY
        //Debug.Log(Cash);

        //Manage UI Housekeeping Variables (For Switching On And Off UI)
        if(BuildActive != BuildPrev || EditActive != EditPrev)
        {
            //Reset Input Alarms
            BuildAlarm = BuildTime;
            EditAlarm = EditTime;

            //Reset Editing Tower Visual Vars
            if(EditingTower != null)
            {
                //Reset Opacity
                EditingTower.GetComponent<Tower>().SetColor(new Color(
                EditingTower.GetComponent<SpriteRenderer>().color.r,
                EditingTower.GetComponent<SpriteRenderer>().color.g,
                EditingTower.GetComponent<SpriteRenderer>().color.b, 1f));

                //Reset Order In Layer 
                EditingTower.GetComponent<Tower>().SetSortingOrder(-5);

                //Detach Referenced Instance
                EditingTower = null;
            }

            //Destroy Unwanted Reference Tower
            if (ConstructingTower != null)
            {
                Destroy(ConstructingTower);
            }           

        }
        BuildPrev = BuildActive;
        EditPrev = EditActive;
        BuildActive = false;
        EditActive = false;

        //Force Reset UI (Sorry it's so messy)
        if(ResetUI)
        {
            //Last Active Values (Input)
            //NOTE: Used to temporarily turn of UI
            //and require the player to reinput 
            //0 - TowerUI
            //1 - EditUI 

            //Reset UI On New Input
            if(LastActive == 0 && TowerUI && !prevTowerUI
            || LastActive == 0 && EditUI && !prevEditUI
            || LastActive == 1 && EditUI && !prevEditUI
            || LastActive == 1 && TowerUI && !prevTowerUI)
            {
                ResetUI = false;
            }
        }

        //Active Tower Construction UI
        if (TowerUI && po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0) && !ResetUI)
        {
            ManageTowerConstructionUI(TowerArrayCount, BuildTower);
            Suspended = true;

            BuildActive = true;
            LastActive = 0;
        }
        //Active Tower Editing UI
        else if(EditUI && po.PlaceMeeting(trans.position.x, trans.position.y - PhysicsObject.minMove, 0) && !ResetUI)
        {
            ManageTowerEditingUI(TowerArrayCount, BuildTower);
            Suspended = true;

            EditActive = true;
            LastActive = 1;
        }
        //Deactivated Tower UI
        else
        {
            //Turn Off UI and Free Player
            ManageRadialUI(false, trans.position, 0);
            Suspended = false;            
        }

   
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

        //Manage Granny Animations 
        if (true)
        {
            //Control Granny Hand
            GranHand.GetComponent<Transform>().position = new Vector3(trans.position.x + GranHandPos.x, 
            trans.position.y + GranHandPos.y, GranHand.GetComponent<Transform>().position.z);

            //Control Granny Body
            Transform GranTrans = GranRef.GetComponent<Transform>();
            if (xAxis != 0 || yAxis != 0)
            {
                //Calculate Input Variables
                float zAxis = Mathf.Sqrt(Mathf.Pow(xAxis, 2) + Mathf.Pow(yAxis, 2)); //(Hypotenuse)
                float GranXDist = (GranDist / zAxis) * xAxis; // (xAxis / (Mathf.Abs(xAxis) + Mathf.Abs(yAxis)));
                float GranYDist = (GranDist / zAxis) * yAxis; // (yAxis / (Mathf.Abs(xAxis) + Mathf.Abs(yAxis)));

                //Set Grandmother Position               
                GranTrans.position = new Vector3(trans.position.x + GranOrigin.x + GranXDist,
                trans.position.y + GranOrigin.y + GranYDist, GranTrans.position.z);

                //Set Granny Direction
                if (xAxis < 0) //RIGHT
                {
                    GranRef.GetComponent<SpriteRenderer>().flipX = false;
                    GranHand.GetComponent<SpriteRenderer>().flipX = false;
                    GranArmDir = -1;
                }
                else if (xAxis > 0) //LEFT
                {
                    GranRef.GetComponent<SpriteRenderer>().flipX = true;
                    GranHand.GetComponent<SpriteRenderer>().flipX = true;
                    GranArmDir = 1;
                }
            }
            else
            {
                //Set Grandmother Position
                GranTrans.position = new Vector3(trans.position.x, trans.position.y, trans.position.z)
                + new Vector3(GranOrigin.x, GranOrigin.y);
            }

            //Control Granny Arm
            Transform ArmTrans = GranArm.GetComponent<Transform>();
            SpriteRenderer ArmSR = GranArm.GetComponent<SpriteRenderer>();
            Vector2 ArmPos1 = GranArmPos1 + new Vector2(trans.position.x, trans.position.y);
            Vector2 ArmPos2 = new Vector2(GranArmPos2.x*GranArmDir, GranArmPos2.y) + new Vector2(GranTrans.position.x, GranTrans.position.y);
            float ArmAngleOffSet = 90;
            float ArmAngle = (Mathf.Atan2((ArmPos2.x - ArmPos1.x), (ArmPos2.y - ArmPos1.y)) / Mathf.PI) * -180 + ArmAngleOffSet;
            float ArmLength = Mathf.Sqrt(Mathf.Pow(ArmPos2.x - ArmPos1.x, 2) + Mathf.Pow(ArmPos2.y - ArmPos1.y, 2));
            if (ArmLength == 0) ArmLength = PhysicsObject.minMove;
            ArmTrans.position = new Vector3(ArmPos1.x, ArmPos1.y, ArmTrans.position.z);
            ArmTrans.localScale = new Vector3(ArmLength / GranArmLength, ArmTrans.localScale.y, ArmTrans.localScale.z);
            ArmTrans.eulerAngles = new Vector3(0f, 0f, ArmAngle);
        }

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
        if (DirectionalReference != null)
        {
            DirectionalReference.GetComponent<Transform>().position = trans.position;
            DirectionalReference.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(yAxis, xAxis) / Mathf.PI) * 180);
            DirectionalReference.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        if (DirectionalReference2 != null)
        {
            DirectionalReference2.GetComponent<Transform>().position = trans.position;
            DirectionalReference2.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, (Mathf.Atan2(yAxis2, xAxis2) / Mathf.PI) * 180);
            DirectionalReference2.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    void ManageTowerConstructionUI(int CurrentCount, bool Building)
    {
        //Set Radial UI Sprites
        SetRadialUISprites(tr.TowerUISprites);
       
        //Manage Radial UI
        ManageRadialUI(true, trans.position, CurrentCount);

          
        //Determine Selected Instance (Manage Tower Reference)
        if(SelectedInst != -1)
        {                
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

                    //Create UI Selection Effect
                    RadialUISelectionEffect(SelectedInst, BuildAlarm, BuildTime);

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

                        //Reset UI
                        ResetUI = true;
                           
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
             
    }//MANAGE TOWER CONSTRUCTION UI

    void ManageTowerEditingUI(int CurrentCount, bool InputPositive)
    {
        //Reference Variables
        EditingTower = null;
        GameObject ReferenceTower = null;

        //Set Radial UI Sprites
        SetRadialUISprites(tr.EditingUISprites);

        //Check To See If Editing Tower Should Have Default Variables
        bool DefaultEditing = true; //(Use if you want to change the color of editing tower)

        //Determine Interactable Tower
        if (EditingTower == null && col.NearestCollider(trans.position.x, trans.position.y, 3) != null
        && col.NearestCollider(trans.position.x, trans.position.y, 3).GetComponent<Tower>().Activated)
        {
            EditingTower = col.NearestCollider(trans.position.x, trans.position.y, 3);
        }
     
        //Edit Interactable Tower
        if (EditingTower != null)
        {
            
            //Reference Variables
            Transform tvTrans = EditingTower.GetComponent<Transform>();
            SpriteRenderer tvSR = EditingTower.GetComponent<SpriteRenderer>();
            Tower tvTower = EditingTower.GetComponent<Tower>();
           
            //Display Radial Editing UI 
            ManageRadialUI(true, tvTrans.position, CurrentCount);

            //Focus Camera On Edited Tower
            playerCamera.CameraPosFollow(tvTrans.position);

            //Lock Axis On Positive Input
            if(InputPositive)
            {
                LockAxis = true;
                LockAxis2 = true;
            }

            //Select Option (INPUT)
            bool OptionCompleted = false;
            if (InputPositive && SelectedInst != -1 && OptionPossible)
            {
                //Deduct Alarm
                if (EditAlarm - Time.deltaTime > 0)
                {
                    EditAlarm -= Time.deltaTime;
                }
                else
                {
                    OptionCompleted = true;
                    EditAlarm = 0;
                }
                

                //Show Visual UI Change
                RadialUISelectionEffect(SelectedInst, EditAlarm, EditTime);
            }
            else EditAlarm = EditTime;


            //Input Options
            //0 - Delete
            //1 - Upgrade 
            OptionPossible = false;

            //Delete Tower
            if (SelectedInst == 0)
            {
                //Fade Out Tower
                tvTower.SetColor(new Color(1f, 1f, 1f,
                (EditAlarm / EditTime)));

                DefaultEditing = false;

                //Finish Operation
                if (OptionCompleted)
                {
                    Destroy(EditingTower);
                    ResetUI = true;
                }

                //Determine If Possible
                if(true)
                {
                    OptionPossible = true;
                }
            }

            //Upgrade Tower
            else if(SelectedInst == 1)
            {
                ReferenceTower = EditingTower.GetComponent<Tower>().UpgradeTower;
            
                //Finish Upgrading 
                if(OptionCompleted && Cash >= ReferenceTower.GetComponent<Tower>().Cost)
                {
                    //Destroy Tower and Prompt UIReset
                    Destroy(EditingTower);
                    ResetUI = true;

                    //Deduct Cash
                    Cash -= ReferenceTower.GetComponent<Tower>().Cost;

                    //Reset Color 
                    ConstructingTower.GetComponent<Tower>().SetColor(new Color(1f, 1f, 1f, 1f));

                    //Disconnect Instance From Reference
                    ConstructingTower.GetComponent<Tower>().Activated = true;
                    ConstructingTower = null;
                }

                //Determine If Possible
                if(EditingTower.GetComponent<Tower>() != null)
                {
                    if(EditingTower.GetComponent<Tower>().UpgradeTower != null
                    && Cash >= ReferenceTower.GetComponent<Tower>().Cost)
                    {
                        OptionPossible = true;
                    }
                }
            }

            //Complete Option
            if(OptionCompleted)
            {
                EditAlarm = EditTime;
            }

            //Create Reference Tower
            if (ConstructingTower == null && ReferenceTower != null && !OptionCompleted)
            {
                ConstructingTower = Instantiate(ReferenceTower);
                ConstructingTower.GetComponent<Tower>().Activated = false;
            }

            //Manage Reference Tower
            if (ConstructingTower != null && ReferenceTower != null)
            {
                //Set Position
                ConstructingTower.GetComponent<Transform>().position = new Vector3(tvTrans.position.x, (tvTrans.position.y - tvSR.bounds.size.y / 2)
                + ConstructingTower.GetComponent<SpriteRenderer>().bounds.size.y / 2, ConstructingTower.GetComponent<Transform>().position.z);

                //Set Order In Layer
                EditingTower.GetComponent<Tower>().SetSortingOrder(-6);

                //Set Color (Show whether can be built or not)
                if(Cash >= ReferenceTower.GetComponent<Tower>().Cost)
                {
                    //Set Color Green
                    ConstructingTower.GetComponent<Tower>().SetColor(new Color(1f + (BuildGreen.r-1f)*(EditAlarm/EditTime),
                    1f + (BuildGreen.g - 1f) * (EditAlarm / EditTime), 1f + (BuildGreen.b - 1f) * (EditAlarm / EditTime), 
                    ConstructingTower.GetComponent<SpriteRenderer>().color.a));
                }
                else
                {
                    //Set Color Red
                    ConstructingTower.GetComponent<Tower>().SetColor(new Color(BuildRed.r,
                    BuildRed.g, BuildRed.b, ConstructingTower.GetComponent<SpriteRenderer>().color.a));
                }

                //Make Editing Tower Opaque
                tvSR.GetComponent<Tower>().SetAlpha(EditAlarm/EditTime);

                //Make Constructing Tower Opaque
                ConstructingTower.GetComponent<Tower>().SetAlpha(.7f + .3f * ((EditTime - EditAlarm) / EditTime));
            }
            
        }

        //Default Editing Tower Variables
        if(ConstructingTower == null && EditingTower != null && DefaultEditing)
        {
            //Make Editing Tower Visible
            SpriteRenderer tvColor = EditingTower.GetComponent<SpriteRenderer>();
            tvColor.GetComponent<Tower>().SetColor(new Color(tvColor.color.r, tvColor.color.g, tvColor.color.b, 1f));
        }

        //Destroy Unecessary Tower Reference
        if (ConstructingTower != null)
        {
            if(ReferenceTower == null)
            {
                Destroy(ConstructingTower);
            }
            else if (ConstructingTower.GetComponent<SpriteRenderer>().sprite != ReferenceTower.GetComponent<SpriteRenderer>().sprite)
            { 
                Destroy(ConstructingTower);
            }
        }

    }//MANAGE TOWER EDITING UI

    void ManageRadialUI(bool Visible, Vector3 RadialPos, int DisplayCount)
    {
   
        //Determine Menu Selection Angle
        float SelectionAngle = ((Mathf.Atan2(yAxis, xAxis) / Mathf.PI) * 180); // + 180;
        SelectionAngle = (SelectionAngle + 360) % 360; //Use Modulo and OffSet to find only positive angles
        SelectedInst = -1;
        float SelectDist = 360;

        //Manage All Active Instances
        if (Visible)
        {
            for (int i = 0; i < DisplayCount; i++)
            {
                //Calculate Position (Input Position)
                float tvAngleOffSet = 90;
                float tvAngle = (i * (360 / DisplayCount) + tvAngleOffSet) % 360; //72 * i + tvAngleOffSet; 
                float UIX = TowerUIOffSetDist * Mathf.Cos((tvAngle / 180) * Mathf.PI);
                float UIY = TowerUIOffSetDist * Mathf.Sin((tvAngle / 180) * Mathf.PI);

                //Set Position
                TowerUIArray[i].GetComponent<Transform>().position = new Vector3(UIX + RadialPos.x,
                UIY + RadialPos.y, TowerUIArray[i].GetComponent<Transform>().position.z);

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
        }

        //Highlight Selected Instance
        if(SelectedInst != -1)
        {
            //Make Larger
            TowerUIArray[SelectedInst].GetComponent<Transform>().localScale = new Vector3(2, 2, 2);

            //Change Color
        }

        //Activated / Deactivated UI
        for (int i = 0; i < TowerArrayMax; i++)
        {
            //Activate Instances
            if (Visible && i < DisplayCount && !TowerUIArray[i].activeSelf)
            {
                TowerUIArray[i].SetActive(true);
            }
            //Deactivate Instance
            else if (!Visible && TowerUIArray[i].activeSelf ||
            i >= DisplayCount && TowerUIArray[i].activeSelf)
            {
                TowerUIArray[i].SetActive(false);
            }
        }
    }

    void SetRadialUISprites(Sprite[] InputSprites)
    {
        //Initialize Variables
        int tvPos = 0;

        //Define Tower UI Sprites
        while(tvPos < TowerUIArray.Length)
        {
            //Set UI Sprite (If Sprite Exists
            if (TowerUIArray[tvPos].GetComponent<SpriteRenderer>().sprite != InputSprites[tvPos])
            {
                TowerUIArray[tvPos].GetComponent<SpriteRenderer>().sprite = InputSprites[tvPos];
            }

            //Increment TvPos
            tvPos++;
        }
    }

    void RadialUISelectionEffect(int index, float Numerator, float Denominator)
    {
        //Calculate Smooth Ratio 
        float SmoothRatio = 0;
        float dPos = 0;
        float nPos = 0;
        for(int i = 0;i < Mathf.Round(Denominator*100); i++)
        { dPos += i; }
        for(int i = 0;i < Mathf.Round(Numerator*100); i++)
        { nPos += i; }
        SmoothRatio = nPos / dPos;

        //Reference Variables
        Transform tvTrans = TowerUIArray[index].GetComponent<Transform>();

        //Set Size (Shrinking)
        float StartSize = 1;
        float EndSize = 2;
        float InputSize = StartSize + (EndSize - StartSize) * (SmoothRatio);
        tvTrans.localScale = new Vector3(InputSize, InputSize, tvTrans.localScale.z);
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
