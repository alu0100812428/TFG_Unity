using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard_tree : MonoBehaviour
{
    private Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        //this.transform.LookAt(camera);
        InvokeRepeating("faceCamera", 0.0f, 3f); //Se llama repetidamente a la función faceCamera cada 3 segundos, empieza el primero a los 0.5segundos    
    }

    void faceCamera(){
        if((transform.position.x<2000)&&(transform.position.y<2000)&&(transform.position.z<2000))
        {
            this.transform.LookAt(camera);
        }
    }

    // Update is called once per frame
    void Update()
    {
          
        
        //this.transform.LookAt(camera);
    }
}
