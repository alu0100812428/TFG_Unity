using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshBrain : MonoBehaviour
{
    MeshGenerator[,] meshes;
    public GameObject arbol;
    public GameObject grass;

    public int xSize = 2;
    public int zSize = 2;
    public float borde = 0.05f;
    int nTrees=1000;
    int nGrass = 100000;

    void spawnTree(){
        float x_random = Random.Range(0.0f, xSize*240f);
        float z_random = Random.Range(0.0f, zSize*240f);
        int x = (int) x_random/240;
        int z = (int) z_random/240;
        int xpos= (int)(x_random -x*240);
        int zpos= (int)(z_random -z*240);
        while(!((meshes[x,z].asignar_altura(xpos,zpos)>=10)&&(meshes[x,z].asignar_altura(xpos,zpos)<=20))){
            x_random = Random.Range(0.0f, xSize*240f);
            z_random = Random.Range(0.0f, zSize*240f);
            x = (int) x_random/240;
            z = (int) z_random/240;
            xpos= (int)(x_random -x*240);
            zpos= (int)(z_random -z*240);
        }

        Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos)-1f,z_random);
        Instantiate(arbol, position, Quaternion.identity);
    }

    void spawnGrass(){
        float x_random = Random.Range(0.0f, xSize*240f);
        float z_random = Random.Range(0.0f, zSize*240f);
        int x = (int) x_random/240;
        int z = (int) z_random/240;
        int xpos= (int)(x_random -x*240);
        int zpos= (int)(z_random -z*240);
        while(meshes[x,z].asignar_altura(xpos,zpos)>=10){
            x_random = Random.Range(0.0f, xSize*240f);
            z_random = Random.Range(0.0f, zSize*240f);
            x = (int) x_random/240;
            z = (int) z_random/240;
            xpos= (int)(x_random -x*240);
            zpos= (int)(z_random -z*240);
        }

        Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos)+.3f,z_random);
        Instantiate(grass, position, Quaternion.Euler(90, Random.Range(0f, 360f), 0));
    }
    
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
                meshes[i,j].asignar_borde(borde);
                meshes[i,j].asignar_tile(xSize,zSize);

                go.transform.SetParent(this.transform);

               // Debug.Log(go.transform.parent);

            }
        }
        for(int i=0;i<nTrees;i++){
            spawnTree();
        }
        for(int i=0;i<nGrass;i++){
            spawnGrass();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
