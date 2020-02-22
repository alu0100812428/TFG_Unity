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

    int xTile;
    int zTile;
    float porcentaje_borde;
    float map_seed=0;

    public int xSize = 240;
    public int zSize = 240;

    public void asignar_posicion(int xpos,int zpos){
        xPosition = xpos*xSize;
        zPosition = zpos*zSize;
        transform.position = new Vector3(xPosition, transform.position.y, zPosition);
    }
    public void asignar_borde(float borde){
        porcentaje_borde = borde;
    }
    public void asignar_tile(int x,int z){
        xTile=x;
        zTile=z;
    }

    public void set_seed(float nSeed){
        map_seed = nSeed;
    }

    public float asignar_altura(int x,int z){

        int xp = x+xPosition;
        int zp = z+zPosition;
        float frec = .004f; //.005
        // y = altura del mapa de colores. Valores entre 0 y 1
        float mapa = Mathf.PerlinNoise((xp+map_seed)*frec,(zp+map_seed)*frec)*1f;
        /* 
        if((xp <= 240*xTile*porcentaje_borde)||(xp>=(240*xTile)-(240*xTile*porcentaje_borde))||(zp<= 240*zTile*porcentaje_borde)||(zp>=(240*zTile)-(240*zTile*porcentaje_borde))){
            mapa = 0.8f;      
        }*/
        if(xp <= 240*xTile*porcentaje_borde){
            float rango=(240*xTile*porcentaje_borde -xp)/(240*xTile*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,rango);
        }
        if(xp>=(240*xTile)-(240*xTile*porcentaje_borde)){
            float rango=(240*xTile-xp)/(240*xTile*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,1-rango);
        }
        if(zp <= 240*zTile*porcentaje_borde){
            float rango=(240*zTile*porcentaje_borde -zp)/(240*zTile*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,rango);
        }
        if(zp>=(240*zTile)-(240*zTile*porcentaje_borde)){
            float rango=(240*zTile-zp)/(240*zTile*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,1-rango);
        }
        float f1=.01f;//.02
        float f2=.025f;//.05
        float f3=.05f;//.1

        float a1 =10f;//10
        float a2=20f;//20
        float a3=100f;//50

        if(mapa<0.4){
            return (Mathf.PerlinNoise(xp*f1,zp*f1)*a1);
        }

        if((mapa>=.4)&&(mapa<0.6)){
            float rango = (mapa -.4f)/(.6f-.4f);
            return Mathf.Lerp(Mathf.PerlinNoise(xp*f1,zp*f1)*a1,(Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2))*a2,rango);
        }
        
        if((mapa>=0.6)&&(mapa<0.7)){
            return (Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2))*a2;
        }
        if((mapa>=0.7)&&(mapa<0.85)){
            float rango = (mapa -.7f)/(.85f-.7f);
            return Mathf.Lerp((Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2))*a2,(Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2)+Mathf.PerlinNoise(xp*f3,zp*f3))*a3,rango);
        }
        if(mapa>=0.85){
            return (Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2)+Mathf.PerlinNoise(xp*f3,zp*f3))*a3;
        }
        return 100f;
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
