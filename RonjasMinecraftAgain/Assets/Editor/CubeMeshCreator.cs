using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestInterfaces;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class CubeMeshCreator : ScriptableObject, IMeshCreator
{

    
private enum BlockShape {
    DefaultCube,
    TopSideBottomCube,
    Liquid,
}

    [SerializeField]
    private string assetDictionary = "Assets/Art/Models";
    [SerializeField]
    private BlockShape shape = BlockShape.DefaultCube;
    [SerializeField]
    private bool bakeMesh = false;
    [SerializeField]
    private Mesh bakedMesh = default;

    protected void OnValidate() {
        if (bakeMesh) {
            bakeMesh = false;

            var assetPath = $"{assetDictionary}/{shape}.asset";

            bakedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
            if (!bakedMesh) {
                bakedMesh = new Mesh();
                AssetDatabase.CreateAsset(bakedMesh, assetPath);
            }

            RecreateMesh(bakedMesh);

            AssetDatabase.SaveAssets();
        }
    }

    private static readonly Vector3Int[] directions = new[] {
        Vector3Int.up,
        Vector3Int.back,
        Vector3Int.right,
        Vector3Int.forward,
        Vector3Int.left,
        Vector3Int.down,
    };
    public void RecreateMesh(Mesh mesh) {

        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var normals = new List<Vector3>();

        foreach (var direction in directions) {
            var rotation = Quaternion.LookRotation(direction);

            vertices.Add(rotation * new Vector3(-0.5f, -0.5f, 0.5f));
            uvs.Add(new Vector2(1, 0));
            normals.Add(direction);

            vertices.Add(rotation * new Vector3(0.5f, -0.5f, 0.5f));
            uvs.Add(new Vector2(0, 0));
            normals.Add(direction);

            vertices.Add(rotation * new Vector3(0.5f, 0.5f, 0.5f));
            uvs.Add(new Vector2(0, 1));
            normals.Add(direction);

            vertices.Add(rotation * new Vector3(-0.5f, 0.5f, 0.5f));
            uvs.Add(new Vector2(1, 1));
            normals.Add(direction);

        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        switch (shape) {

            case BlockShape.DefaultCube:
                mesh.subMeshCount = 1;
                mesh.SetIndices(Enumerable.Range(0, 24).ToList(), MeshTopology.Quads, 0);
                break;

            case BlockShape.TopSideBottomCube:
                mesh.subMeshCount = 3;
                mesh.SetIndices(Enumerable.Range(0, 4).ToList(), MeshTopology.Quads, 0);
                mesh.SetIndices(Enumerable.Range(4, 16).ToList(), MeshTopology.Quads, 1);
                mesh.SetIndices(Enumerable.Range(20, 4).ToList(), MeshTopology.Quads, 2);
                break;

            case BlockShape.Liquid:
                mesh.vertices = mesh.vertices.Select(v => v + Vector3.down / 16).ToArray();
                mesh.subMeshCount = 1;
                mesh.SetIndices(Enumerable.Range(0, 4).ToList(), MeshTopology.Quads, 0);
                break;
        }
        mesh.Optimize();
    }


}
