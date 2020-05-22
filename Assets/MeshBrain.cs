using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshBrain : MonoBehaviour
{
    MeshGenerator[,] meshes;
    public GameObject arbol;
    public GameObject arbol2;
    public GameObject[] grass;
    public GameObject[] rock;
    public GameObject particles;

    public int xSize = 2;
    public int zSize = 2;
    public float borde = 0.05f;
    int nTrees=100;
    int nGrass = 2000;
    int nRocks = 10;
    int nParticlesSpawns= 100;

    public bool set_seed=true;
    public bool set_objects = true;

    void spawnTree(){
        float x_random = Random.Range(0.0f, xSize*240f-1f);
        float z_random = Random.Range(0.0f, zSize*240f-1f);
        int x = (int) x_random/240;
        int z = (int) z_random/240;
        int xpos= (int)(x_random -x*240);
        int zpos= (int)(z_random -z*240);
        if(((meshes[x,z].asignar_altura(xpos,zpos)>=10)&&(meshes[x,z].asignar_altura(xpos,zpos)<=40))){
            Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos)-1f,z_random);
            Instantiate(arbol, position, Quaternion.Euler(0, Random.Range(0f, 180f), 0));
        }
         
        if((meshes[x,z].asignar_altura(xpos,zpos)<10)&&(Random.Range(0,10)==1)){
            Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos)-1f,z_random);
            Instantiate(arbol2, position, Quaternion.Euler(0, Random.Range(0f, 180f), 0));
        }
    }

    void spawnGrass(){
        float x_random = Random.Range(0.0f, xSize*240f-1f);
        float z_random = Random.Range(0.0f, zSize*240f-1f);
        int x = (int) x_random/240;
        int z = (int) z_random/240;
        int xpos= (int)(x_random -x*240);
        int zpos= (int)(z_random -z*240);
        if(meshes[x,z].asignar_altura(xpos,zpos)<=10){
            Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos)+.3f,z_random);
            Instantiate(grass[0], position, Quaternion.Euler(0, Random.Range(0f, 180f), 0));
        }
        
    }
    void spawnRocks(){
        float x_random = Random.Range(0.0f, xSize*240f-1f);
        float z_random = Random.Range(0.0f, zSize*240f-1f);
        int x = (int) x_random/240;
        int z = (int) z_random/240;
        int xpos= (int)(x_random -x*240);
        int zpos= (int)(z_random -z*240);
        if(meshes[x,z].asignar_altura(xpos,zpos)<=50){
            Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos),z_random);
            Instantiate(rock[Random.Range(0,rock.Length)], position, Quaternion.Euler(0, Random.Range(0f, 180f), 0));
        }
    }
    void spawnParticlesPoint(){
        float x_random = Random.Range(0.0f, xSize*240f-1f);
        float z_random = Random.Range(0.0f, zSize*240f-1f);
        int x = (int) x_random/240;
        int z = (int) z_random/240;
        int xpos= (int)(x_random -x*240);
        int zpos= (int)(z_random -z*240);
        if((meshes[x,z].asignar_altura(xpos,zpos)>=10) && (meshes[x,z].asignar_altura(xpos,zpos)<=40)){
            Vector3 position = new Vector3(x_random, meshes[x,z].asignar_altura(xpos,zpos),z_random);
            Instantiate(particles, position, Quaternion.Euler(0, Random.Range(0f, 180f), 0));
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        meshes = new MeshGenerator[xSize,zSize];
        Material material = Resources.Load("materialbasico", typeof(Material)) as Material;

        float seed = Random.Range(0.0f, 10000000f);

        nTrees = xSize*zSize*nTrees;
        nGrass = xSize*zSize*nGrass;
        nRocks = xSize*zSize*nRocks;
         
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
                meshes[i,j].asignar_tile(i,j,xSize,zSize);
                if(set_seed){
                    meshes[i,j].set_seed(seed);
                }

                go.transform.SetParent(this.transform);


            }
        }
        
        if(set_objects){
            for(int i=0;i<nRocks;i++){
                spawnRocks();
            }

            for(int i = 0;i<nParticlesSpawns;i++){
                spawnParticlesPoint();
            }
            /* 
            for(int i=0;i<nTrees;i++){
                //spawnTree();
            }
            for(int i=0;i<nGrass;i++){
                //spawnGrass();
            }
            */
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
