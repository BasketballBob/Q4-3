using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    //Reference Variables
    Sprite[] CreditsSheet;
    public Sprite Texture1;
    public Sprite Texture2;
    public Sprite Texture3;
    GameObject CreditObject;
    SpriteRenderer CreditRenderer;

    //Credits Variables
    int CreditPos = 0;
    int CreditCount = 3;

    //Define Reference Variables
    private void OnEnable()
    {
        CreditsSheet = new Sprite[CreditCount];
        CreditsSheet[0] = Texture1;
        CreditsSheet[1] = Texture2;
        CreditsSheet[2] = Texture3;

        CreditObject = new GameObject();
        CreditRenderer = CreditObject.AddComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Player Input 
        bool Forward = Input.GetKeyDown(KeyCode.RightArrow);
        bool Backward = Input.GetKeyDown(KeyCode.LeftArrow);

        //Set Sprite Position
        if(Forward)
        {
            if (CreditPos < CreditCount-1)
            {
                CreditPos++;
            }
            else CreditPos = 0;
        }
        else if(Backward)
        {
            if (CreditPos > 0)
            {
                CreditPos--;
            }
            else CreditPos = CreditCount-1;
        }

        //Set Sprite Value
        CreditRenderer.sprite = CreditsSheet[CreditPos];
    }
}
