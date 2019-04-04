using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    //Reference Variables
    [System.NonSerialized] public Transform trans;
    [System.NonSerialized] public SpriteRenderer sr;
    [System.NonSerialized] public Collider col;
    [System.NonSerialized] public GameObject target;
    [System.NonSerialized] public List<GameObject> Children = new List<GameObject>(); //For Color Inheritance
    public GameObject projectile;

    //Tower Variables
    public int Cost = 30;
    public bool Activated = true;
    public Vector3 ShotOffset = new Vector3(0, 2.5f);
    public float Range = 15;
    public float AttackTime = 1f;
    [System.NonSerialized] public float AttackAlarm;
    public float BulletSpeed = 20f;

    //Define Reference Variables
    public virtual void OnEnable()
    {
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        col = gameObject.AddComponent<Collider>();
        col.CollisionType = 3;
    }

    //Initialize Variables
    public virtual void Start()
    {
        //Set Draw Order
        sr.sortingOrder = -5;
    }

    // Update is called once per frame
    public virtual void Update()
    {

        //Only Operate If Active
        if (!Activated) return;

        //Deduct Attack Alarm
        if (AttackAlarm > 0)
        {
            AttackAlarm -= Time.deltaTime;
        }

        //Detect Closest Target
        target = col.NearestCollider(trans.position.x, trans.position.y, 2);

	}

    public void SetColor(Color SetColor)
    {
        //Color Self
        GetComponent<SpriteRenderer>().color = SetColor;

        //Set Color Of Children Objects
        foreach(GameObject element in Children)
        {
            element.GetComponent<SpriteRenderer>().color = SetColor;
        }
    }
}
