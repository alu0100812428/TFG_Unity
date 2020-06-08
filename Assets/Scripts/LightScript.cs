using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightScript : MonoBehaviour
{
    float x=50;
    public float speed = .001f;

    public Light lt;
    Color colorBase;
    Color atardecer;

    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
        colorBase = lt.color;
        atardecer = new Color(70f/255f, 79f/255f, 55f/255f); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(x, transform.rotation.y, transform.rotation.z);
        if((transform.eulerAngles.x > 0)&&(transform.eulerAngles.x < 90)){
            float rango = transform.eulerAngles.x/90;
            //lt.color = new Color(233f/255f, 79f/255f, 55f/255f);
            //lt.color = Color.Lerp(atardecer, colorBase,rango);
            lt.intensity = Mathf.Lerp(.1f,1f,rango);
        }
        if((transform.eulerAngles.x >= 90)&&(transform.eulerAngles.x <= 180)){
            float rango = transform.eulerAngles.x-90/90;
            //lt.color = new Color(233f/255f, 79f/255f, 55f/255f);
            //lt.color = Color.Lerp(colorBase,atardecer,rango);
            lt.intensity = Mathf.Lerp(.1f,1f,1-rango);
        }
        /* 
        if(transform.eulerAngles.x >= 180){
            float rango = transform.eulerAngles.x-180/180;
            //lt.color = Color.Lerp(atardecer,new Color(10f/255f, 10f/255f, 10f/255f),rango);
            //lt.color = new Color(10f/255f, 10f/255f, 10f/255f);
        }
        */
        x=x+speed;
    }
}
