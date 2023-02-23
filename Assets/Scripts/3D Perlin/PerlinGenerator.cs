using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject block;

    [SerializeField]
    private float noiseScale = 0.1f;

    public float sizeX = 50;
    public float sizeY = 50;
    public float sizeZ = 50;

    public float cutOff = 0.5f;

    public Material material;

    private List<Mesh> meshes = new List<Mesh>(); //the actual list of meshes (since they have a limit) 

    void Start()
    {
        float startTime = Time.realtimeSinceStartup;
        //Generating initial meshes and structure...

        List<CombineInstance> blockData = new List<CombineInstance>(); //the list of meshes to be combined. this includes ALL the blockData...
        MeshFilter blockMesh = Instantiate(block, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>(); //this just gets the block mesh, and also is the inputed block

        for (float x = 0; x < sizeX; x++)
        {
            for(float y = 0; y < sizeY; y++)
            {
                for(float z = 0; z < sizeZ; z++)
                {
                    float xFactor = x * noiseScale;
                    float yFactor = y * noiseScale;
                    float zFactor = z * noiseScale;
                    float value = perlin3D(xFactor, yFactor, zFactor);
                    if (value >= cutOff)
                    {
                        blockMesh.transform.position = new Vector3(x, y, z); //move the unit cube to the intended position
                        CombineInstance ci = new CombineInstance
                        {
                            mesh = blockMesh.sharedMesh,
                            transform = blockMesh.transform.localToWorldMatrix,
                        };
                        blockData.Add(ci);
                        //Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
        Destroy(blockMesh.gameObject);//original unit cube is no longer needed. we copied all the data we need to the block list.

        //Creating proper mesh lists...

        List<List<CombineInstance>> blockDataLists = new List<List<CombineInstance>>(); //storing meshes in a list of lists, sublist contains the data for one mesh, like the blockData.
        int vertexCount = 0;
        blockDataLists.Add(new List<CombineInstance>());
        for(int i = 0; i < blockData.Count; i++)
        {
            vertexCount += blockData[i].mesh.vertexCount;
            if(vertexCount > 65536) //max amount in a unity mesh
            {
                vertexCount = 0;
                blockDataLists.Add(new List<CombineInstance>());
                i--;
            } else
            {
                blockDataLists.Last().Add(blockData[i]);
            }
        }

        //creating the mesh itself

        Transform container = new GameObject("unitMesh").transform;
        foreach(List<CombineInstance> data in blockDataLists)
        {
            GameObject g = new GameObject("unitMesh");
            g.transform.parent = container;
            MeshFilter mf = g.AddComponent<MeshFilter>();
            MeshRenderer mr = g.AddComponent<MeshRenderer>();
            mr.material = material;
            mf.mesh.CombineMeshes(data.ToArray());
            meshes.Add(mf.mesh);
        }
        Debug.Log("Loaded in " + (Time.realtimeSinceStartup - startTime) + " Seconds.");
    }

    public static float perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6f;
    }
}
