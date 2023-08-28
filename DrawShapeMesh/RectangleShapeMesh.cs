using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleShapeMesh : ShapeMesh
{

    [Range(1.0f, 20.0f)] public float shapeLength =1.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 20.0f;

    void Start()
    {
        SetMesh();
        SetMaterial();
    }


    protected override void SetMesh()
    {
        baseMesh = new Mesh();
        fillMesh = new Mesh();

        baseMesh.vertices = new Vector3[4 ];
        baseMesh.triangles = new int[3 * 2 ];

        fillMesh.vertices = new Vector3[4 ];
        fillMesh.triangles = new int[3 * 2 ];

        Vector3[] normals = new Vector3[4 ];
        Vector2[] uv = new Vector2[4 ];

        for (int i = 0; i < uv.Length; i++)
            uv[i] = new Vector2(0, 0);
        for (int i = 0; i < normals.Length; i++)
            normals[i] = new Vector3(0, 1, 0);

        baseMesh.uv = uv;
        baseMesh.normals = normals;
        fillMesh.uv = uv;
        fillMesh.normals = normals;
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
        Vector3[] baseVertices = new Vector3[4];
        Vector3[] fillVertices = new Vector3[4];
        int[] triangles = new int[6] 
        { 0,1,2,
          2,3,0 };

        Vector3 leftMinPos = pos  - transform.right * shapeLength + dir* minDistance;
        Vector3 leftMaxPos = pos - transform.right * shapeLength + dir * maxDistance ;

        Vector3 rightMinPos = pos  + transform.right * shapeLength + dir * minDistance ;
        Vector3 rightMaxPos = pos  + transform.right * shapeLength + dir * maxDistance ;

        baseVertices[0] = leftMinPos;
        baseVertices[1] = leftMaxPos;
        baseVertices[2] = rightMaxPos;
        baseVertices[3] = rightMinPos;

        fillVertices[0] = leftMinPos + new Vector3(0, 0.001f, 0);
        fillVertices[1] = Vector3.Lerp(leftMinPos, leftMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
        fillVertices[2] = Vector3.Lerp(rightMinPos, rightMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
        fillVertices[3] = rightMinPos + new Vector3(0, 0.001f, 0);

        baseMesh.Clear();
        baseMesh.vertices = baseVertices;
        baseMesh.triangles = triangles;
        baseMesh.RecalculateNormals();

        fillMesh.Clear();
        fillMesh.vertices = fillVertices;
        fillMesh.triangles = triangles;
        fillMesh.RecalculateNormals();

        Graphics.DrawMesh(baseMesh, Vector3.zero, Quaternion.identity, baseMaterial, 0);
        Graphics.DrawMesh(fillMesh, Vector3.zero, Quaternion.identity, fillMaterial, 0);
    }
   
}

