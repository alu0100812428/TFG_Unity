using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    int lod=1;          //Nivel de detalla del mesh

    MeshCollider meshc;

    public GameObject player;

    public int xPosition;   //posicion x del mundo. Se usa para mover el mesh a su posición.
    public int zPosition;   //posicion z del mundo

    public bool collision;

    public int xTile;   //Coordenada x que va de 0 hasta el numero de meshes que se hayan creado
    public int zTile;   //Coordenada z que va de 0 hasta el numero de meshes que se hayan creado

    int xMapSize;       //Numero total de meshes en el eje x
    int zMapSize;       //Numero total de meshes en el eje z
    float porcentaje_borde; //valor entre 0.0 y 1.0 para definir cuanto borde se quiere aplicar al mapa
    float map_seed=0;   //Semilla que se usará para crear el mapa

    public int xSize = 240;     //Tamaño en el eje x del mesh
    public int zSize = 240;     //Tamaño en el eje z del mesh

    bool use_lod = true;

    void mesh_lod(){
        if(use_lod){
            if((player.transform.position.x>xTile*240-240)&&(player.transform.position.x < xTile*240 +480)&&(player.transform.position.z>zTile*240-240)&&(player.transform.position.z < zTile*240+480)){
                lod = 1;
            }
            else{
                if((player.transform.position.x>xTile*240-480)&&(player.transform.position.x < xTile*240 +720)&&(player.transform.position.z>zTile*240-480)&&(player.transform.position.z < zTile*240+720)){
                    lod = 2;
                }
                else{
                    lod= 4;
                }
            }
        }
        
    }

    bool player_is_near(){
        if((player.transform.position.x>=xTile*240)&&(player.transform.position.x <= xTile*240 +240)&&(player.transform.position.z>=zTile*240)&&(player.transform.position.z <= zTile*240 +240)){
            return true;
        }
        else{
            return false;
        }
    }

    public void asignar_posicion(int xpos,int zpos){
        xPosition = xpos*xSize;
        zPosition = zpos*zSize;
        transform.position = new Vector3(xPosition, transform.position.y, zPosition);
    }
    public void asignar_borde(float borde){
        porcentaje_borde = borde;
    }
    public void asignar_tile(int x,int z,int xmap,int zmap){
        xTile=x;
        zTile=z;
        xMapSize = xmap;
        zMapSize = zmap;
    }

    public void set_seed(float nSeed){
        map_seed = nSeed;
    }

    float flat(int x,int z){
        int xp = x+xPosition;
        int zp = z+zPosition;
        float f1=.01f;//.02

        float a1 =10f;//10
        return (Mathf.PerlinNoise(xp*f1,zp*f1)*a1);
    }
    float hill(int x,int z){
        int xp = x+xPosition;
        int zp = z+zPosition;
        float f1=.01f;//.02
        float f2=.025f;//.05

        float a1 =10f;//10
        float a2=40f;//20
        return (Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2))*a2 + Mathf.PerlinNoise(xp*0.2f,zp*0.2f)*2.0f;
    }
    float mountain(int x, int z){
        int xp = x+xPosition;
        int zp = z+zPosition;
        float f1=.005f;//.02
        float f2=.012f;//.05
        float f3=.025f;//.1

        float a1 =10f;//10
        float a2=40f;//20
        float a3=100f;//50
        return (Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2)+Mathf.PerlinNoise(xp*f3,zp*f3))*a3+Mathf.PerlinNoise(xp*0.1f,zp*0.1f)*15.0f+Mathf.PerlinNoise(xp*0.2f,zp*0.2f)*10.0f;
    }

    public float asignar_altura(int x,int z){

        int xp = x+xPosition;
        int zp = z+zPosition;

        float frec = .002f; //.005
        float mapa = Mathf.PerlinNoise((xp+map_seed)*frec,(zp+map_seed)*frec)*1f;

        //---------------------------Bordes del mapa-----------------------------//
        if(xp <= 240*xMapSize*porcentaje_borde){
            float rango=(240*xMapSize*porcentaje_borde -xp)/(240*xMapSize*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,rango);
        }
        if(xp>=(240*xMapSize)-(240*xMapSize*porcentaje_borde)){
            float rango=(240*xMapSize-xp)/(240*xMapSize*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,1-rango);
        }
        if(zp <= 240*zMapSize*porcentaje_borde){
            float rango=(240*zMapSize*porcentaje_borde -zp)/(240*zMapSize*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,rango);
        }
        if(zp>=(240*zMapSize)-(240*zMapSize*porcentaje_borde)){
            float rango=(240*zMapSize-zp)/(240*zMapSize*porcentaje_borde);
            mapa = Mathf.Lerp(mapa,0.85f,1-rango);
        }

        //---------------------------------------------------------------------------//

        if(mapa<0.4){
            return flat(x,z);
        }

        if((mapa>=.4)&&(mapa<0.6)){
            float rango = (mapa -.4f)/(.6f-.4f);
            return Mathf.Lerp(flat(x,z),hill(x,z),rango);
        }
        
        if((mapa>=0.6)&&(mapa<0.7)){
            return hill(x,z);
        }
        if((mapa>=0.7)&&(mapa<0.85)){
            float rango = (mapa -.7f)/(.85f-.7f);
            return Mathf.Lerp(hill(x,z),mountain(x,z),rango);
        }
        if(mapa>=0.85){
            return mountain(x,z);
        }
        return 100f;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        mesh = new Mesh();
        mesh_lod();

        GetComponent<MeshFilter>().mesh = mesh;
        meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        CreateShape();
        UpdateMesh();

        meshc.sharedMesh = mesh;
        
    }
    
    void CreateShape(){
        vertices = new Vector3[((xSize/lod) + 1)*((zSize/lod) + 1)];
        uvs = new Vector2[vertices.Length];
        triangles = new int[xSize/lod * zSize/lod * 6];

        int vert= 0;
        int tris = 0;
        
        for(int i=0,z = 0; z <= zSize/lod; z++)
        {
            for(int x = 0;x<=xSize/lod;x++){

                float y= asignar_altura(x*lod,z*lod);

                vertices[i]=new Vector3(x*lod,y,z*lod);
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                if(( x==xSize/lod)||(z==zSize/lod)){
                }
                else{
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + xSize/lod + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + xSize/lod +1;
                    triangles[tris + 5] = vert + xSize/lod +2;

                    vert++;
                    tris +=6;
                }
                i++;
            }
            if(z!=zSize/lod){
                vert++;
            }
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
        if(use_lod){
            int last_lod =lod;
            mesh_lod();
            if(last_lod!=lod){
                CreateShape();
                UpdateMesh();
                if((lod==1)&&(player_is_near())){
                    meshc.sharedMesh = mesh;
                }

            }
        }
    }
}
