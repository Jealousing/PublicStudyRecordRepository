using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonShapeMesh : ShapeMesh
{
    public float radius;
    public int pointNum;
    public int distance;
    [Range(0.0f, 360.0f)] public float rotate;

    void Start()
    {
        SetMesh();
        SetMaterial();
    }
    protected override void SetMesh()
    {

    }
    protected override void SetMaterial()
    {
        baseMaterial = new Material(Shader.Find("Standard"));
        baseMaterial.color = new Color(110 / 255f, 0 / 255f, 0 / 255f, 0);

        fillMaterial = new Material(Shader.Find("Standard"));
        fillMaterial.color = new Color(200 / 255f, 0 / 255f, 0 / 255f, 1.0f);
    }

    public override void DrawShape(Vector3 pos, Vector3 dir)
    {
        baseMesh = new();
        fillMesh=new();

        //verticies
        List<Vector3> baseVerticiesList = new() { };
        List<Vector3> fillVerticiesList = new() { };
        float x;
        float z;
        for (int i = 0; i < pointNum; i++)
        {
            x =  Mathf.Sin((2 * Mathf.PI * i) / pointNum);
            z =  Mathf.Cos((2 * Mathf.PI * i) / pointNum);

            baseVerticiesList.Add(pos + dir * distance + Quaternion.Euler(0, rotate, 0) * new Vector3(x * radius, pos.y, z * radius) );
            fillVerticiesList.Add(pos + dir* distance + Quaternion.Euler(0, rotate, 0) * new Vector3(x* Mathf.Lerp(0, radius,fillProgress), pos.y+0.001f, z* Mathf.Lerp(0, radius, fillProgress)));
        }
        Vector3[] baseVertices = baseVerticiesList.ToArray();
        Vector3[] fillVertices = fillVerticiesList.ToArray();

        //triangles
        List<int> trianglesList = new List<int> { };
        for (int i = 0; i < (pointNum - 2); i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }
        int[] triangles = trianglesList.ToArray();

        //normals
        List<Vector3> normalsList = new List<Vector3> { };
        for (int i = 0; i < baseVertices.Length; i++)
        {
            normalsList.Add(-Vector3.forward);
        }
        Vector3[] normals = normalsList.ToArray();


        baseMesh.Clear();
        baseMesh.vertices = baseVertices;
        baseMesh.triangles = triangles;
        baseMesh.normals = normals;
        baseMesh.RecalculateNormals();

        fillMesh.Clear();
        fillMesh.vertices = fillVertices;
        fillMesh.triangles = triangles;
        fillMesh.normals = normals;
        fillMesh.RecalculateNormals();

        Graphics.DrawMesh(baseMesh, Vector3.zero, Quaternion.identity, baseMaterial, 0);
        Graphics.DrawMesh(fillMesh, Vector3.zero, Quaternion.identity, fillMaterial, 0);
    }

}
