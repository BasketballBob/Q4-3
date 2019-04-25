using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    //Reference Variables
    Transform CamTrans;
    public Sprite Fence, Tree, City;
    float ScreenWidth = 14;

    //Parallax Scrolling Variables
    List<Layer> Layers;
    public struct Layer
    {
        public float XPos;
        public float YPos;
        public float OffSetMultiplier;
        public float ObjectSpread;
        public int LayerOrder;
        public Vector2 ImageScale;
        public Color ImageColor;
        public Sprite LayerImage;
        public List<GameObject> ObjectList;

        public Layer(float yPos, float offSetMultiplier, float objectSpread, Sprite layerImage, int layerOrder)
        {
            XPos = 0;
            YPos = yPos;
            OffSetMultiplier = offSetMultiplier;
            ObjectSpread = objectSpread;
            LayerImage = layerImage;
            LayerOrder = layerOrder;
            ImageScale = new Vector2(1, 1);
            ImageColor = new Color(1f, 1f, 1f, 1f);
            ObjectList = new List<GameObject>();
        }

        public Layer(float yPos, float offSetMultiplier, float objectSpread, Sprite layerImage, int layerOrder, Vector2 imageScale, Color imageColor)
        {
            XPos = 0;
            YPos = yPos;
            OffSetMultiplier = offSetMultiplier;
            ObjectSpread = objectSpread;
            LayerImage = layerImage;
            LayerOrder = layerOrder;
            ImageScale = imageScale;
            ImageColor = imageColor;
            ObjectList = new List<GameObject>();
        }
    }

    //Define Reference Variables
    private void OnEnable()
    {
        CamTrans = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Add Layers To Layer List
        Layers = new List<Layer>();
        //Layers.Add(new Layer(1, .25f, 0, City, -7));
        //Layers.Add(new Layer(0, .5f, 1, Fence, -6));
        Layers.Add(new Layer(1, 1f, 3f, Tree, -5));
        //Layers.Add(new Layer())
        Layers.Add(new Layer(0, .5f, -1f, Tree, -6, new Vector2(.75f, .75f), new Color(.8f, .8f, .8f)));
        Layers.Add(new Layer(-1, .25f, -1f, Tree, -7, new Vector2(.5f, .5f), new Color(.6f, .6f, .6f)));

        //Initialize Layers
        foreach (Layer element in Layers)
        {
            //Determine Length Of Sprite
            GameObject testObj = new GameObject();
            testObj.AddComponent<SpriteRenderer>();
            testObj.GetComponent<SpriteRenderer>().sprite = element.LayerImage;
            testObj.GetComponent<Transform>().localScale = new Vector3(element.ImageScale.x, element.ImageScale.y, 1);
            float ImageWidth = testObj.GetComponent<SpriteRenderer>().bounds.size.x;
            Destroy(testObj);

            //Create Necessary Amount Of Objects
            for(float i = 0;i < ScreenWidth+ImageWidth*2+element.ObjectSpread*2;i += ImageWidth+element.ObjectSpread)
            {
                GameObject tvGO = new GameObject();
                tvGO.AddComponent<SpriteRenderer>();
                tvGO.GetComponent<SpriteRenderer>().sprite = element.LayerImage;
                tvGO.GetComponent<SpriteRenderer>().sortingOrder = element.LayerOrder;
                tvGO.GetComponent<Transform>().localScale = new Vector3(element.ImageScale.x, element.ImageScale.y, 1);
                tvGO.GetComponent<SpriteRenderer>().color = element.ImageColor;
                element.ObjectList.Add(tvGO);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Display All Layers
        foreach(Layer element in Layers)
        {
            //Set Position Of First Object
            float ImageWidth = element.ObjectList[0].GetComponent<SpriteRenderer>().bounds.size.x;
            Debug.Log(ImageWidth);
            Debug.Log(element.ObjectSpread);
            float InitialX = CamTrans.position.x-ScreenWidth/2 - (CamTrans.position.x*element.OffSetMultiplier) % (ImageWidth+element.ObjectSpread);
            element.ObjectList[0].transform.position = new Vector3(InitialX,
            element.YPos, element.ObjectList[0].GetComponent<Transform>().position.z);

            //Set Child Object Position
            for(int i = 0; i < element.ObjectList.Count-1; i++)
            {
                //Reference Variables
                Transform tvTrans = element.ObjectList[i + 1].GetComponent<Transform>();        

                //Set Position
                tvTrans.position = new Vector3(InitialX + (i + 1) * (ImageWidth+element.ObjectSpread), element.YPos, tvTrans.position.z);
            }
        }
    }
}
