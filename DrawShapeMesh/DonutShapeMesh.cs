using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fetch :
/// https://gamedev.stackexchange.com/questions/31170/drawing-a-dynamic-indicator-for-a-field-of-view
/// </summary>shape
public class DonutShapeMesh : ShapeMesh
{
    // 삼각형 갯수 
    [Range(1, 100)] public int quality = 15;
    // 도넛의 각도
    [Range(1.0f, 180.0f)] public float shapeAngle = 40;
    public float height = 2.0f; // 메시의 높이 값을 지정합니다.
    // 안쪽 원의 거리 바깥원의 거리
    public float minDistance = 5.0f;
    public float maxDistance = 15.0f;

    // 기본 설정
    private void Start()
    {
        SetMesh();
        SetMaterial();
    }

    // 메쉬 생성 및 설정
    protected override void SetMesh()
    {
        baseMesh = new Mesh();
        fillMesh = new Mesh();

        baseMesh.vertices = new Vector3[4 * quality];
        baseMesh.triangles = new int[3 * 2 * quality];
        fillMesh.vertices = new Vector3[4 * quality];
        fillMesh.triangles = new int[3 * 2 * quality];

        Vector3[] normals = new Vector3[4 * quality];
        Vector2[] uv = new Vector2[4 * quality];

        for (int i = 0; i < uv.Length; i++)
            uv[i] = new Vector2(0, 0);
        for (int i = 0; i < normals.Length; i++)
            normals[i] = new Vector3(0, 1, 0);

        baseMesh.uv = uv;
        baseMesh.normals = normals;
        fillMesh.uv = uv;
        fillMesh.normals = normals;
    }

    // 메테리얼 설정
    protected override void SetMaterial()
    {
        baseMaterial = new Material(Shader.Find("Standard"));
        baseMaterial.color = new Color(110 / 255f, 0 / 255f, 0 / 255f, 0);

        fillMaterial = new Material(Shader.Find("Standard"));
        fillMaterial.color = new Color(200 / 255f, 0 / 255f, 0 / 255f, 1.0f);
    }

    /// <summary>
    /// 도형 그리기
    /// </summary>
    /// <param name="standard"> 그리는 위치 </param>
    public override void DrawShape(Vector3 pos, Vector3 dir)
    {
        // 바라보는 각도
        float forwardAngle = GetForwardAngle(dir);
        float leftAngle = forwardAngle - shapeAngle;
        float rightAngle = forwardAngle + shapeAngle;
        float betweenAngle = (rightAngle - leftAngle) / quality;

        float currAngle = leftAngle;
        float nextAngle = leftAngle + betweenAngle;


        Vector3[] baseVertices = new Vector3[4 * quality];
        Vector3[] fillVertices = new Vector3[4 * quality];

        int[] triangles = new int[3 * 2 * quality];

        for (int i = 0; i < quality; i++)
        {
            Vector3 currSphere = new Vector3(
            Mathf.Sin(Mathf.Deg2Rad * (currAngle)), 0,   // Left handed CW
            Mathf.Cos(Mathf.Deg2Rad * (currAngle)));

            Vector3 nextSphere = new Vector3(
            Mathf.Sin(Mathf.Deg2Rad * (nextAngle)), 0,
            Mathf.Cos(Mathf.Deg2Rad * (nextAngle)));

            Vector3 currMinPos = pos + currSphere * minDistance;
            Vector3 currMaxPos = pos + currSphere * maxDistance;


            Vector3 nextMinPos = pos + nextSphere * minDistance;
            Vector3 nextMaxPos = pos + nextSphere * maxDistance;

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;


            /*
              삼각형 1: abc
                        2: cda
            b ----- c 
            ㅣ        /ㅣ
            ㅣ     /   ㅣ
            ㅣ /       ㅣ
            a ----- d
             */

            baseVertices[a] = currMinPos;
            baseVertices[b] = currMaxPos;
            baseVertices[c] = nextMaxPos;
            baseVertices[d] = nextMinPos;

            fillVertices[a] = currMinPos + new Vector3(0, 0.001f, 0);
            fillVertices[b] = Vector3.Lerp(currMinPos, currMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
            fillVertices[c] = Vector3.Lerp(nextMinPos, nextMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
            fillVertices[d] = nextMinPos + new Vector3(0, 0.001f, 0);

            triangles[6 * i] = a;
            triangles[6 * i + 1] = b;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = d;
            triangles[6 * i + 5] = a;

            currAngle += betweenAngle;
            nextAngle += betweenAngle;

        }

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


    // 높이를 제외한 바라보는 각도
    protected float GetForwardAngle(Vector3 dir)
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(dir.z, dir.x);
    }

}
