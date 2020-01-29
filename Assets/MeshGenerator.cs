using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

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

        float offsetx = Random.Range(0,100);
        float offsetz = Random.Range(0,100);
        
        for(int i=0,z = 0; z <= zSize; z++)
        {
            for(int x = 0;x<=xSize;x++){
                //float y= Mathf.PerlinNoise(x*.1f ,z*.1f) * 10f;
                //float y= Mathf.PerlinNoise(x*.1f + offsetx,z*.1f+offsetz) * 10f;
                //float y= (Mathf.PerlinNoise(x*.1f ,z*.1f) * 5f)+(Mathf.PerlinNoise(x*.2f ,z*.2f) * 2.5f);
                //float y= (Mathf.PerlinNoise(x*.1f ,z*.1f) * 10f)+(Mathf.PerlinNoise(x*.2f ,z*.2f) * 5f)+(Mathf.PerlinNoise(x*.4f ,z*.4f) * 2.5f);
                //float y= (Mathf.PerlinNoise(x*.1f ,z*.1f) * 6f)+(Mathf.PerlinNoise(x*.2f ,z*.2f) * 5f)+(Mathf.PerlinNoise(x*.4f ,z*.4f) * 2.5f);

                float y= (Mathf.PerlinNoise(x*.03f ,z*.03f) * 50f)+(Mathf.PerlinNoise(x*.2f ,z*.2f) * 2f)+(Mathf.PerlinNoise(x*.4f ,z*.4f) * 1f);

                vertices[i]=new Vector3(x,y,z);
                i++;
            }
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

        mesh.RecalculateNormals();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
