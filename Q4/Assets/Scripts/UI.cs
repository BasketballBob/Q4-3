 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    
    // Defining all the varibles for text
    public float cx, cy, cw, ch;
    public float wx, wy, ww, wh;
    float HealthCap = GameManager.HealthCap;

    
   
    
    //varibles for font and font size
    public Font Defultfont, WaveFont;
    public int fontSize;

    //vectors varibles for game object that will be spawned at the start of the game.
    Vector3 BaseHealthPos, TowerPos, WaveProgressBarPos;
    public Vector3 Toffest,Hoffset,Poffset,HMoffset,PMoffset, BHoffset, PBoffset, Coinoffset;
    public GameObject HealthMask, ProgressMask;
    public Sprite BaseHealthBar, Tower, WaveProgressBar, HealthBackground, ProgressBackground, Coin; 
    GameObject HealthRef, TowerRef, ProgressRef, ProgressMaskRef, HealthMaskRef, HealthBackgroundRef, ProgressBackgroundRef, CoinRef;

    Transform ts;
    //curency, health, and wave varibles
    public static int currency;
    public static int health;
    public static int wave;

    
    private void OnGUI()
    {
        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.font = GUI.skin.button.font = GUI.skin.box.font = Defultfont;
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = fontSize;

        //Displays Curency text at the given corintaes
        GUI.Label(new Rect(cx, cy, cw, ch), currency.ToString());

        //Displays the wave counter text at the given corinates
        GUI.skin.label.normal.textColor = Color.black;
        GUI.skin.label.font = GUI.skin.button.font = GUI.skin.box.font = WaveFont;
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = fontSize;
        GUI.Label(new Rect(wx, wy, ww, wh), wave.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        ts=GetComponent<Transform>();
       

        //Create The GameObjects (Julien)
        //Health Bar full
        HealthRef = new GameObject();
        HealthRef.AddComponent<SpriteRenderer>();
        //HealthRef.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        HealthRef.GetComponent<SpriteRenderer>().sprite = BaseHealthBar;
        //Tower
        TowerRef = new GameObject();
        TowerRef.AddComponent<SpriteRenderer>();
        TowerRef.GetComponent<SpriteRenderer>().sprite = Tower;
        //Wave Progress bar full
        ProgressRef = new GameObject();
        ProgressRef.AddComponent<SpriteRenderer>();
        ProgressRef.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        ProgressRef.GetComponent<SpriteRenderer>().sprite = WaveProgressBar;
       
        ProgressBackgroundRef = new GameObject();
        ProgressBackgroundRef.AddComponent<SpriteRenderer>();
        ProgressBackgroundRef.GetComponent<SpriteRenderer>().sprite = ProgressBackground;

       //Health background this is what palyers will see if the base takes damage.
        HealthBackgroundRef = new GameObject();
        HealthBackgroundRef.AddComponent<SpriteRenderer>();
        HealthBackgroundRef.GetComponent<SpriteRenderer>().sprite = HealthBackground;
        //Coin 
        CoinRef = new GameObject();
        CoinRef.AddComponent<SpriteRenderer>();
        CoinRef.GetComponent<SpriteRenderer>().sprite = Coin;

        ProgressMaskRef = Instantiate(ProgressMask);
        HealthMaskRef = Instantiate(HealthMask);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // set the vector to move the objects reltive to the camrea
        Vector3 TowerRefCam = new Vector3(ts.position.x, ts.position.y, TowerRef.transform.position.z);
        Vector3 HealthRefCam = new Vector3(ts.position.x, ts.position.y, HealthRef.transform.position.z);
        Vector3 ProgressRefCam = new Vector3(ts.position.x, ts.position.y, ProgressRef.transform.position.z);

        Vector3 ProgressMaskRefCam = new Vector3(ts.position.x, ts.position.y, ProgressMaskRef.transform.position.z);
        Vector3 HealthMaskRefCam = new Vector3(ts.position.x, ts.position.y, HealthMaskRef.transform.position.z);

        Vector3 BackgroundHealthRefCam = new Vector3(ts.position.x, ts.position.y, HealthBackgroundRef.transform.position.z);
        Vector3 BackgroundProgressRefCam = new Vector3(ts.position.x, ts.position.y, ProgressBackgroundRef.transform.position.z);
        Vector3 CoinCam = new Vector3(ts.position.x, ts.position.y, CoinRef.transform.position.z);

        //Move the game objects reltive to the camrea
        TowerRef.transform.position=(TowerRefCam+Toffest);

        ProgressRef.transform.position=(ProgressRefCam+Poffset);

        HealthRef.transform.position=(HealthRefCam+Hoffset);

        ProgressMaskRef.transform.position = (ProgressMaskRefCam + PMoffset);
        HealthMaskRef.transform.position = (HealthMaskRefCam + HMoffset);

        HealthBackgroundRef.transform.position = (HealthMaskRefCam + BHoffset);
        ProgressBackgroundRef.transform.position = (HealthMaskRefCam + PBoffset);
        CoinRef.transform.position = (CoinCam + Coinoffset);

        // move the health sprite mask
        currency++;
        wave++;
        float maskX = HealthRef.transform.localScale.x;
        float Health = GameManager.Health;

        Vector3 HealthBarPos = HealthRef.transform.position;
        float HealthBarWidth = HealthRef.GetComponent<SpriteRenderer>().bounds.size.x;

        float HealthRatio = Health / HealthCap;

        //Debug.Log("Base Health " +GameManager.Health);
        Vector3 MaskPos = new Vector3(HealthBarPos.x - HealthRatio * HealthBarWidth, HealthBarPos.y, HealthBarPos.z);
        HealthMaskRef.transform.position = MaskPos;
        //move the progress sprite mask

    }
}
