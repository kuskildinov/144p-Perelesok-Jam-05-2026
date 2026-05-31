using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WallOutlineMeshBuilder : MonoBehaviour
{
    public float cellSize = 1f;

    // ВАЖНО: подними выше кубов (под стандартный Unity Cube)
    public float yOffset = 0.51f;

    private MeshFilter mf;

    void Awake()
    {
        mf = GetComponent<MeshFilter>();
    }

    [ContextMenu("Build Outline")]
    void Start()
    {
        Build();
    }

    public void Build()
    {
        // 1. собираем все кубы в сетку
        Transform[] all = FindObjectsOfType<Transform>();

        HashSet<Vector2Int> cells = new HashSet<Vector2Int>();

        foreach (var myTransform in all)
        {
            if (myTransform == transform) continue;

            Vector3 p = myTransform.position;

            // более стабильное приведение к сетке
            int x = Mathf.FloorToInt((p.x + cellSize * 0.5f) / cellSize);
            int z = Mathf.FloorToInt((p.z + cellSize * 0.5f) / cellSize);

            cells.Add(new Vector2Int(x, z));
        }

        Mesh mesh = new Mesh();

        List<Vector3> v = new List<Vector3>();
        List<int> t = new List<int>();

        // 2. строим контур
        foreach (var c in cells)
        {
            int x = c.x;
            int z = c.y;

            Vector3 origin = new Vector3(x * cellSize, yOffset, z * cellSize);

            Vector3 right = Vector3.right * cellSize;
            Vector3 forward = Vector3.forward * cellSize;

            bool left = cells.Contains(new Vector2Int(x - 1, z));
            bool rightN = cells.Contains(new Vector2Int(x + 1, z));
            bool up = cells.Contains(new Vector2Int(x, z + 1));
            bool down = cells.Contains(new Vector2Int(x, z - 1));

            // LEFT EDGE
            if (!left)
                AddQuad(v, t,
                    origin,
                    origin + forward);

            // RIGHT EDGE
            if (!rightN)
                AddQuad(v, t,
                    origin + right,
                    origin + right + forward);

            // DOWN EDGE
            if (!down)
                AddQuad(v, t,
                    origin,
                    origin + right);

            // UP EDGE
            if (!up)
                AddQuad(v, t,
                    origin + forward,
                    origin + forward + right);
        }

        mesh.SetVertices(v);
        mesh.SetTriangles(t, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        mf.mesh = mesh;
    }

    void AddQuad(List<Vector3> v, List<int> t, Vector3 a, Vector3 b)
    {
        float thickness = 0.05f;

        Vector3 dir = (b - a).normalized;
        Vector3 right = Vector3.Cross(dir, Vector3.up) * thickness;

        int i = v.Count;

        v.Add(a - right);
        v.Add(a + right);
        v.Add(b + right);
        v.Add(b - right);

        // правильная ориентация (чтобы не было "перевёрнуто")
        t.Add(i + 0);
        t.Add(i + 1);
        t.Add(i + 2);

        t.Add(i + 0);
        t.Add(i + 2);
        t.Add(i + 3);
    }
}