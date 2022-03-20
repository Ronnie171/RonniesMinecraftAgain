using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class ExampleCubeCreator : ScriptableObject {
    [SerializeField]
    private string assetPath = "Assets/Art/Models/ExampleCube.asset";
    [SerializeField]
    private bool bakeMesh = false;
    [SerializeField]
    private Mesh bakedMesh = default;

    // OnValidate is called whenever this object is changed in the Inspector
    protected void OnValidate() {
        if (bakeMesh) {
            bakeMesh = false;

            // if the mesh already exists, load it
            bakedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

            if (!bakedMesh) {
                // if not, create it
                bakedMesh = new Mesh();
                AssetDatabase.CreateAsset(bakedMesh, assetPath);
            }

            // recreate the mesh
            ModifyMesh(bakedMesh);

            // save the changed mesh to disk
            AssetDatabase.SaveAssets();
        }
    }

    private void ModifyMesh(Mesh mesh) {
        // first, create the vertex information

        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        // Vertex #0
        positions.Add(new Vector3(0, 0, 0));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0, 1));

        // Vertex #1
        positions.Add(new Vector3(0, 0, 1));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0, 0));

        // Vertex #2
        positions.Add(new Vector3(0, 1, 0));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0, 1));

        // Vertex #3
        positions.Add(new Vector3(0, 1, 1));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0, 1));

        // Vertex #4
        positions.Add(new Vector3(1, 0, 0));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(1, 0));

        // Vertex #5
        positions.Add(new Vector3(1, 0, 1));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(1, 0));

        // Vertex #6
        positions.Add(new Vector3(1, 1, 0));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(1, 1));

        // Vertex #7
        positions.Add(new Vector3(1, 1, 1));
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(1, 1));

        mesh.Clear();
        mesh.SetVertices(positions);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        // next, assign quads 

        var quads = new List<int> {
            // Quad 1
            0,
            1,
            3,
            2,

            // Quad 2
            6,
            7,
            5,
            4,

            // Quad 3
            4,
            5,
            1,
            0,

            // Quad 4
            2,
            3,
            7,
            6,

            // Quad 5
            0,
            2,
            6,
            4,

            // Quad 6
            5,
            7,
            3,
            1
        };

        // assign all quads to submesh #0
        mesh.SetIndices(quads, MeshTopology.Quads, 0);

        // Unity likes to do some optimizations with the finished mesh
        mesh.Optimize();
    }
}
