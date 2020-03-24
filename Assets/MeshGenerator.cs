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

    bool set_objects = true;

    List<Vector3> grassPosition;
    List<GameObject> grassObjects;
    bool grassInstantiated = false;

    List<Vector3> treePosition;
    List<GameObject> treeObjects;
    bool treeInstantiated = false;

    public int xPosition;   //posicion x del mundo. Se usa para mover el mesh a su posición.
    public int zPosition;   //posicion z del mundo

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

    float flat(float x,float z){
        float xp = x+xPosition;
        float zp = z+zPosition;
        float f1=.01f;//.02

        float a1 =10f;//10
        return (Mathf.PerlinNoise(xp*f1,zp*f1)*a1);
    }
    float hill(float x,float z){
        float xp = x+xPosition;
        float zp = z+zPosition;
        float f1=.01f;//.02
        float f2=.025f;//.05

        float a2=40f;//20
        return (Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2))*a2 + Mathf.PerlinNoise(xp*0.2f,zp*0.2f)*2.0f;
    }
    float mountain(float x, float z){
        float xp = x+xPosition;
        float zp = z+zPosition;
        float f1=.005f;//.02
        float f2=.012f;//.05
        float f3=.025f;//.1

        float a3=100f;//50
        return (Mathf.PerlinNoise(xp*f1,zp*f1)+Mathf.PerlinNoise(xp*f2,zp*f2)+Mathf.PerlinNoise(xp*f3,zp*f3))*a3+Mathf.PerlinNoise(xp*0.1f,zp*0.1f)*15.0f+Mathf.PerlinNoise(xp*0.2f,zp*0.2f)*10.0f;
    }

    public float asignar_altura(float x,float z){

        float xp = x+xPosition;
        float zp = z+zPosition;

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
        set_objects = gameObject.GetComponentInParent<MeshBrain>( ).set_objects;
        player = GameObject.Find("player");
        mesh = new Mesh();
        mesh_lod();

        GetComponent<MeshFilter>().mesh = mesh;
        meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        CreateShape();
        UpdateMesh();

        meshc.sharedMesh = mesh;

        grassPosition = new List<Vector3>();
        grassObjects = new List<GameObject>();

        treePosition = new List<Vector3>();
        treeObjects = new List<GameObject>();

        if(set_objects){
            generateGrassPosition();
            generateTreePosition();
        }
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
        float offset = 50;
        if(set_objects){
            if(!grassInstantiated){
                
                if((player.transform.position.x>=xTile*240 -offset)&&(player.transform.position.x <= xTile*240 +240 + offset)&&(player.transform.position.z>=zTile*240-offset)&&(player.transform.position.z <= zTile*240 +240 + offset)){
                    instantiateGrass();
                    grassInstantiated=true;
                }
            }
            else{
                if((player.transform.position.x>=xTile*240-50)&&(player.transform.position.x <= xTile*240 +290)&&(player.transform.position.z>=zTile*240-50)&&(player.transform.position.z <= zTile*240 +290)){
                    //instantiateGrass();
                    //grassInstantiated=true;
                }
                else{
                    destroyGrass();
                    grassInstantiated=false;
                }
            }
            float offset2 = 1000;

            if(!treeInstantiated){
                
                if((player.transform.position.x>=xTile*240 -offset2)&&(player.transform.position.x <= xTile*240 +240 + offset2)&&(player.transform.position.z>=zTile*240-offset2)&&(player.transform.position.z <= zTile*240 +240 + offset2)){
                    instantiateTree();
                    treeInstantiated=true;
                }
            }
            else{
                if((player.transform.position.x>=xTile*240 -offset2)&&(player.transform.position.x <= xTile*240 +240 + offset2)&&(player.transform.position.z>=zTile*240-offset2)&&(player.transform.position.z <= zTile*240 +240 + offset2)){
                    //instantiateGrass();
                    //grassInstantiated=true;
                }
                else{
                    destroyTree();
                    treeInstantiated=false;
                }
            }
        }
        if(use_lod){
            int last_lod =lod;
            mesh_lod();
            if((lod==1)&&(player_is_near())){   //si el lod es el de maximo detalle y el jugador se encuentra en ese mismo mesh, se cambia el collider
                meshc.sharedMesh = mesh;
            }

            if(last_lod!=lod){
                CreateShape();
                UpdateMesh();              
            }
        }
    }







    void generateGrassPosition(){
        for(int i=0;i<1000;i++){
            
        float x_random = Random.Range(0.0f, 240f-1f);
        float z_random = Random.Range(0.0f, 240f-1f);
        if(asignar_altura(x_random,z_random)<=10){
            Vector3 position = new Vector3(xPosition + x_random, asignar_altura(x_random,z_random)+.3f,zPosition + z_random);
            grassPosition.Add(position);
        }
        }
        
    }

    void instantiateGrass(){
        for(int i=0;i<grassPosition.Count;i++){
            //yield return new WaitForEndOfFrame(); 
            GameObject bruh = Instantiate(gameObject.GetComponentInParent<MeshBrain>( ).grass, grassPosition[i], Quaternion.Euler(0, Random.Range(0f, 180f), 0));
            bruh.transform.SetParent(this.transform);
            grassObjects.Add(bruh);
        }
    }

    

    void destroyGrass(){
        for(int i=0;i<grassObjects.Count;i++){
            Destroy(grassObjects[i]);
        }
    }


    void generateTreePosition(){
        for(int i=0;i<200;i++){
            
            float x_random = Random.Range(0.0f, 240f-1f);
            float z_random = Random.Range(0.0f, 240f-1f);
            var height = asignar_altura(x_random,z_random);
            if((height<=10)&&((Random.Range(0,10)==1))){
                Vector3 position = new Vector3(xPosition + x_random, asignar_altura(x_random,z_random)-1f,zPosition + z_random);
                treePosition.Add(position);
            }
            else{
                if((height>10)&&(height<=40)){
                    Vector3 position = new Vector3(xPosition + x_random, asignar_altura(x_random,z_random)-1f,zPosition + z_random);
                    treePosition.Add(position);
                }
                
            }
        }
        
    }

    void instantiateTree(){
        for(int i=0;i<treePosition.Count;i++){
            //yield return new WaitForEndOfFrame();
            if(treePosition[i].y<=10){
                
                GameObject bruh = Instantiate(gameObject.GetComponentInParent<MeshBrain>( ).arbol2, treePosition[i], Quaternion.Euler(0, Random.Range(0f, 180f), 0));
                bruh.transform.SetParent(this.transform);
                treeObjects.Add(bruh);
            }else{
                GameObject bruh = Instantiate(gameObject.GetComponentInParent<MeshBrain>( ).arbol, treePosition[i], Quaternion.Euler(0, Random.Range(0f, 180f), 0));
                bruh.transform.SetParent(this.transform);
                treeObjects.Add(bruh);
            }
            
        }
    }

    

    void destroyTree(){
        for(int i=0;i<treeObjects.Count;i++){
            Destroy(treeObjects[i]);
        }
    }
}
