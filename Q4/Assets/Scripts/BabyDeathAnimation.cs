using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDeathAnimation : MonoBehaviour
{
    public static bool IsDead=false;
    public Animation BabyDeathAnimation1;
    // Start is called before the first frame update
    void Start()
    {
        BabyDeathAnimation1 = GetComponent<Animation>();
        IsDead = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead==true)
        {
            BabyDeathAnimation1.Play();
        }
    }
}
