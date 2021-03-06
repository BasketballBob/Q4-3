﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRepository : MonoBehaviour
{
    //NOTE: This script is used as a simple workaround
    //for storing all the tower object references in the
    //players inspector.

    //Tower Array Variables 
    // (Sync Array Size With Player Values) \/ \/ \/
    int TowerUISpriteCount = 10;
    public Sprite[] TowerUISprites = new Sprite[10];
    public Sprite[] EditingUISprites = new Sprite[10];

    //Pea Shooter
    public GameObject tr_PeaShooter;
    public Sprite tr_PeaShooter_Sprite;

    //Laser Ray
    public GameObject tr_LaserRay;
    public Sprite tr_LaserRay_Sprite;

    //Wall 
    public GameObject tr_Wall;
    public Sprite tr_Wall_Sprite;

    private void Start()
    {

        //Packup Tower UI Sprite Array
        TowerUISprites[0] = tr_PeaShooter_Sprite;
        TowerUISprites[1] = tr_LaserRay_Sprite;
        TowerUISprites[2] = tr_Wall_Sprite;

        //Packup Editing UI Sprite Array
        for (int i = 0; i < 10; i++)
        {
            EditingUISprites[i] = tr_PeaShooter_Sprite;
        }
    }

}
