using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OceanController : MonoBehaviour
{
    public int dimensions = 10;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = GenerateVerts();
        triangles = GenerateTries();
        // vertices = new Vector3[]
        // {
        //     new Vector3(0,0,0),
        //     new Vector3(0,0,1),
        //     new Vector3(1,0,0),
        //     new Vector3(1,0,1)
        // };

        // triangles = new int[]
        // {
        //     0, 1, 2,
        //     1, 3, 2
        // };
    }

    Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(dimensions) * (dimensions)];
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;
    }

    int[] GenerateTries()
    {
        var tries = new int[mesh.vertices.Length * 6];
        for (int x = 0; x < dimensions; x++)
        {
            for (int z = 0; z < dimensions; z++) {
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }

        return tries;
    }

    int index(int x, int z)
    {
        return x * (dimensions + 1) + z;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
