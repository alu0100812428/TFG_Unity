using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshBrain : MonoBehaviour
{
    MeshGenerator[,] meshes;

    public int xSize = 2;
    public int zSize = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        meshes = new MeshGenerator[xSize,zSize];
        Material material = Resources.Load("materialbasico", typeof(Material)) as Material;
        //Debug.Log(material);
         
        for(int i=0;i<xSize;i++){
            for(int j=0;j<zSize;j++){

                GameObject go = new GameObject();
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.AddComponent<MeshGenerator>();
                go.GetComponent<MeshRenderer>().material = material;
                go.layer = 9;
                meshes[i,j] = go.GetComponent<MeshGenerator>();
                meshes[i,j].asignar_posicion(i,j);

                go.transform.SetParent(this.transform);

               // Debug.Log(go.transform.parent);

            }
        } 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
