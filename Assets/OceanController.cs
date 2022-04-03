using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OceanController : MonoBehaviour
{
    public int dimensions = 200;
    public Octave[] Octaves;
    public float UVScale;
    public float oceanScale = 4;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        mesh.uv = GenerateUVs();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = GenerateVerts();
        triangles = GenerateTries();
    }

    Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(dimensions + 1) * (dimensions + 1)];
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                verts[index(x, z)] = new Vector3(x * oceanScale, 0, z * oceanScale);
            }
        }

        return verts;
    }

    int[] GenerateTries()
    {
        var tries = new int[vertices.Length * 6];
        for (int x = 0; x < dimensions; x++)
        {
            for (int z = 0; z < dimensions; z++)
            {
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

    Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[mesh.vertices.Length];

        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    void UpdateVerts()
    {
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var y = 0f;
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x) / dimensions, (z * Octaves[o].scale.y) / dimensions) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + Octaves[o].speed.magnitude * Time.time) * Octaves[o].height;
                    }
                }
                vertices[index(x, z)] = new Vector3(x * oceanScale, y, z * oceanScale);
            }
        }
    }

    void Update()
    {
        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        UpdateVerts();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
