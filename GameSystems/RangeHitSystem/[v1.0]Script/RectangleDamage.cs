using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleDamage : Damage
{

    protected override void SetMeshAndMaterial()
    {
        baseMesh = new Mesh();
        fillMesh = new Mesh();

        baseMesh.vertices = new Vector3[4];
        baseMesh.triangles = new int[3 * 2];

        fillMesh.vertices = new Vector3[4];
        fillMesh.triangles = new int[3 * 2];

        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        for (int i = 0; i < uv.Length; i++)
            uv[i] = new Vector2(0, 0);
        for (int i = 0; i < normals.Length; i++)
            normals[i] = new Vector3(0, 1, 0);

        baseMesh.uv = uv;
        baseMesh.normals = normals;
        fillMesh.uv = uv;
        fillMesh.normals = normals;

        baseMaterial = new Material(Shader.Find("Standard"));
        baseMaterial.color = new Color(110 / 255f, 0 / 255f, 0 / 255f, 0);

        fillMaterial = new Material(Shader.Find("Standard"));
        fillMaterial.color = new Color(200 / 255f, 0 / 255f, 0 / 255f, 1.0f);
    }

    protected override void Draw()
    {
        if (base.preFillProgress == base.fillProgress)
        {
            Graphics.DrawMesh(baseMesh, Vector3.zero, Quaternion.identity, baseMaterial, 0);
            if (base.isFill) Graphics.DrawMesh(fillMesh, Vector3.zero, Quaternion.identity, fillMaterial, 0);
        }
        else
        {
            Vector3[] baseVertices = new Vector3[4];
            Vector3[] fillVertices = new Vector3[4];
            int[] triangles = new int[6]
            { 0,1,2,
            2,3,0 };
            Vector3 pos = this.transform.position;
            Vector3 dir = this.transform.forward;

            Vector3 leftMinPos = pos - transform.right * width + dir * minDistance;
            Vector3 leftMaxPos = pos - transform.right * width + dir * maxDistance;

            Vector3 rightMinPos = pos + transform.right * width + dir * minDistance;
            Vector3 rightMaxPos = pos + transform.right * width + dir * maxDistance;

            baseVertices[0] = leftMinPos;
            baseVertices[1] = leftMaxPos;
            baseVertices[2] = rightMaxPos;
            baseVertices[3] = rightMinPos;

            baseMesh.Clear();
            baseMesh.vertices = baseVertices;
            baseMesh.triangles = triangles;
            baseMesh.RecalculateNormals();

            Graphics.DrawMesh(baseMesh, Vector3.zero, Quaternion.identity, baseMaterial, 0);
            if (base.isFill)
            {
                fillVertices[0] = leftMinPos + new Vector3(0, 0.001f, 0);
                fillVertices[1] = Vector3.Lerp(leftMinPos, leftMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
                fillVertices[2] = Vector3.Lerp(rightMinPos, rightMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
                fillVertices[3] = rightMinPos + new Vector3(0, 0.001f, 0);

                fillMesh.Clear();
                fillMesh.vertices = fillVertices;
                fillMesh.triangles = triangles;
                fillMesh.RecalculateNormals();

                Graphics.DrawMesh(fillMesh, Vector3.zero, Quaternion.identity, fillMaterial, 0);
                base.preFillProgress = base.fillProgress;
            }
        }
    }

    public override void StartDraw()
    {
        if (fillTime > 0)
        {
            StartCoroutine(DrawShape());
        }
        else
        {
            isDraw = true;
        }
    }

    public override void StopDraw()
    {
        isDraw = false;
        isFill = false;
        fillProgress = 0;
        preFillProgress = -0.1f;
        fillTime = 0;
        pool.Release(this);
    }

    protected override IEnumerator DrawShape()
    {
        yield return null;

        if (isDraw) yield break;
        isDraw = true;


        float tickCount = fillTime / tick;
        float add = 1 / tickCount;

        for (int count = 0; count < tickCount + 1; count++)
        {
            fillProgress += add;
            yield return tickTimer;
        }
        yield return null;

        if (isDamage)
        {
            List<GameObject> objects = CheckObject();
            if (objects.Count > 0)
            {
                for (int count = 0; count < objects.Count; count++)
                {
                    objects[count].GetComponent<BasicInfo>().TakeDamage(damage);
                }
            }
            isDamage = false;
        }
        StopDraw();
    }

    public override void StartDamage()
    {
        StartCoroutine(CheckObjectDamage());
    }

    protected override IEnumerator CheckObjectDamage()
    {
        yield return null;
        float tickCount = fillTime / tick;
        for (int count = 0; count < tickCount + 1; count++)
        {
            yield return tickTimer;
        }

        List<GameObject> objects = CheckObject();
        if (objects.Count > 0)
        {
            for (int count = 0; count < objects.Count; count++)
            {
                objects[count].GetComponent<BasicInfo>().TakeDamage(damage);
            }
        }
        isDamage = false;
        StopDraw();
    }

    public override List<GameObject> CheckObject()
    {
        Vector3 pos = this.transform.position;
        Vector3 dir = this.transform.forward;

        float cubeWidth = width * 2;
        float cubeDepth = maxDistance - minDistance;
        float cubeHeight = 5.0f;

        Collider[] colliders = Physics.OverlapBox(pos + dir * (maxDistance + minDistance) * 0.5f, 
            new Vector3(cubeWidth, cubeHeight, cubeDepth) * 0.5f, transform.rotation
            , base.targetLayer);

        List<GameObject> objects = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            Vector3 closestPoint = collider.ClosestPoint(pos);
            float distance = Vector3.Distance(closestPoint, pos);
            if (distance < maxDistance && distance > minDistance)
            {
                objects.Add(collider.gameObject);
            }
        }
        return objects;
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = this.transform.position;
        Vector3 dir = this.transform.forward;

        float cubeWidth = width*2;
        float cubeDepth = maxDistance - minDistance;
        float cubeHeight = 5.0f;

        Vector3 cubePos = pos + dir * ((minDistance + maxDistance) * 0.5f);
        Quaternion cubeRot = Quaternion.LookRotation(dir, this.transform.up);
        Vector3 cubeSize = new Vector3(cubeWidth, cubeHeight, cubeDepth);

        Gizmos.color = Color.white;
        Gizmos.matrix = Matrix4x4.TRS(cubePos, cubeRot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, cubeSize);
    }




}
