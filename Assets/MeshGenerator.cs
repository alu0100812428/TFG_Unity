using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    public int xPosition;
    public int zPosition;

    public int xSize = 240;
    public int zSize = 240;

    public void asignar_posicion(int xpos,int zpos){
        xPosition = xpos*xSize;
        zPosition = zpos*zSize;
        transform.position = new Vector3(xPosition, transform.position.y, zPosition);
    }

    public float asignar_altura(int x,int z){
        // y = altura del mapa de colores. Valores entre 0 y 1
        /*
        float mapa = Mathf.PerlinNoise((x+xPosition)*.005f,(z+zPosition)*.005f)*1f;

        if(mapa<0.5){
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*10f);
            //return 0;
        }
        if((mapa>=0.5)&&(mapa<0.6)){
            float rango = (mapa -.5f)/(.6f-.5f);
            return ((Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)+
            (Mathf.PerlinNoise((x+xPosition)*.05f,(z+zPosition)*.05f))))*Mathf.Lerp(0f,20f,rango);
        }
        if((mapa>=0.6)&&(mapa<0.7)){
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)+
            Mathf.PerlinNoise((x+xPosition)*.05f,(z+zPosition)*.05f))*20f;
        }
        if((mapa>0.7)&&(mapa<0.85)){
            float rango = (mapa -.7f)/(.85f-.7f);
            return ((Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)+
            (Mathf.PerlinNoise((x+xPosition)*.05f,(z+zPosition)*.05f))+
            (Mathf.PerlinNoise((x+xPosition)*.1f,(z+zPosition)*.1f))))*Mathf.Lerp(15f,50f,rango);
        }
        if(mapa>=.85){
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)+
            Mathf.PerlinNoise((x+xPosition)*.05f,(z+zPosition)*.05f)+
            Mathf.PerlinNoise((x+xPosition)*.1f,(z+zPosition)*.1f))*50f;
        }
        return mapa;
        */
        float y = Mathf.PerlinNoise((x+xPosition)*.005f,(z+zPosition)*.005f)*1f;
         
        if(y<=.4f){ // terreno mas plano
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*10f);
        }
        if((y>0.4)&&(y<=0.5)){
            float rango = (y -.4f)/(.5f-.4f);
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*(Mathf.Lerp(10f,50f,rango)));
        }
        if((y>0.5)&&(y<=0.6)){ //terreno poco montañoso
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*50f);
        }
        if((y>0.6)&&(y<=0.7)){
            float rango = (y -.6f)/(.7f-.6f);
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*(Mathf.Lerp(50f,100f,rango)));
        }
        if(y>0.7){ // terreno montañoso
            return (Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*100f);
            //return ((Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f))+(Mathf.PerlinNoise((x+xPosition)*.02f,(z+zPosition)*.02f)*.1f))*100f;
        }
        else{
            return 100f;
        }
        
        /* 
        return (Mathf.Pow((Mathf.PerlinNoise((x+xPosition)*.01f ,(z+zPosition)*.01f) *1f)+
            (Mathf.PerlinNoise((x+xPosition)*.02f ,(z+zPosition)*.02f) *.5f)+
            (Mathf.PerlinNoise((x+xPosition)*.04f ,(z+zPosition)*.04f) * 0.25f),2f))*30f;
        */
    }
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        
        GetComponent<MeshFilter>().mesh = mesh;
        MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        CreateShape();
        UpdateMesh();
        meshc.sharedMesh = mesh;
    }
    
    void CreateShape(){
        vertices = new Vector3[(xSize + 1)*(zSize + 1)];
        uvs = new Vector2[vertices.Length];
        
        for(int i=0,z = 0; z <= zSize; z++)
        {
            for(int x = 0;x<=xSize;x++){

                float y= asignar_altura(x,z);

                vertices[i]=new Vector3(x,y,z);
                i++;
            }
        }
         
        for (int i = 0; i <vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }    
        triangles = new int[xSize * zSize * 6];

        int vert= 0;
        int tris = 0;
        for(int z=0;z<zSize;z++){
            for(int x=0;x<xSize;x++){
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize +1;
                triangles[tris + 5] = vert + xSize +2;

                vert++;
                tris +=6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();

    }
    // Update is called once per frame
    void Update()
    {

    }
}
