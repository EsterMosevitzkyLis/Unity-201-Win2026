using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircularQuad : MonoBehaviour
{
    [SerializeField] private int _segments = 32;
    [SerializeField] private float _radius = 0.5f;

    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "CircularQuad";

        int vertCount = _segments + 2;

        Vector3[] vertices = new Vector3[vertCount];
        Vector2[] uv = new Vector2[vertCount];
        int[] triangles = new int[_segments * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        float angleStep = Mathf.PI * 2f / _segments;

        for (int i = 0; i <= _segments; i++)
        {
            float angle = i * angleStep;

            float x = Mathf.Cos(angle) * _radius;
            float y = Mathf.Sin(angle) * _radius;

            vertices[i + 1] = new Vector3(x, y, 0);

            uv[i + 1] = new Vector2(
                (x / (_radius * 2f)) + 0.5f,
                (y / (_radius * 2f)) + 0.5f
            );
        }

        int triIndex = 0;

        for (int i = 1; i <= _segments; i++)
        {
            triangles[triIndex++] = 0;
            triangles[triIndex++] = i;
            triangles[triIndex++] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

#if UNITY_EDITOR
    [ContextMenu("Generate And Save Circular Quad")]
    void GenerateAndSave()
    {
        Mesh mesh = GenerateMesh();

        string path = EditorUtility.SaveFilePanelInProject(
            "Save Circular Quad Mesh",
            "CircularQuad",
            "asset",
            "Choose location for the mesh asset"
        );

        if (string.IsNullOrEmpty(path))
            return;

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        Debug.Log("Circular quad mesh saved at: " + path);
    }
#endif
}