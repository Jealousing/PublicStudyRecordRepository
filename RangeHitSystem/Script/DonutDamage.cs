using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DonutDamage : Damage
{
    int quality;
 
    // 메쉬, 메테리얼 설정
    protected override void SetMeshAndMaterial()
    {
        quality = (int)maxDistance * 10;
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

        baseMaterial = new Material(Shader.Find("Standard"));
        baseMaterial.color = new Color(110 / 255f, 0 / 255f, 0 / 255f, 0);

        fillMaterial = new Material(Shader.Find("Standard"));
        fillMaterial.color = new Color(200 / 255f, 0 / 255f, 0 / 255f, 1.0f);
    }

    // 기본그리기
    protected override void Draw()
    {
        if (base.preFillProgress == base.fillProgress)
        {
            Graphics.DrawMesh(baseMesh, Vector3.zero, Quaternion.identity, baseMaterial, 0);
            if (base.isFill) Graphics.DrawMesh(fillMesh, Vector3.zero, Quaternion.identity, fillMaterial, 0);
        }
        else
        {
            // 바라보는 각도
            float forwardAngle = Utils.GetForwardAngle(this.transform.forward);
            Vector3 pos = this.transform.position;
            float leftAngle = forwardAngle - base.limitAngle;
            float rightAngle = forwardAngle + base.limitAngle;
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

                baseVertices[a] = currMinPos;
                baseVertices[b] = currMaxPos;
                baseVertices[c] = nextMaxPos;
                baseVertices[d] = nextMinPos;

                if (base.isFill)
                {
                    fillVertices[a] = currMinPos + new Vector3(0, 0.001f, 0);
                    fillVertices[b] = Vector3.Lerp(currMinPos, currMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
                    fillVertices[c] = Vector3.Lerp(nextMinPos, nextMaxPos, fillProgress) + new Vector3(0, 0.001f, 0);
                    fillVertices[d] = nextMinPos + new Vector3(0, 0.001f, 0);
                }


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
            Graphics.DrawMesh(baseMesh, Vector3.zero, Quaternion.identity, baseMaterial, 0);

            if (base.isFill)
            {
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, base.targetLayer);
        List<GameObject> objects = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance > minDistance && 1<<collider.gameObject.layer == base.targetLayer)
            {
                if (limitAngle !=180)
                {
                    Vector3 direction = (collider.transform.position - this.transform.position).normalized;
                    float angle = Vector3.Angle(direction, this.transform.forward);
                    if(angle < limitAngle)
                    {
                        objects.Add(collider.gameObject);
                    }
                }
                else 
                {
                    objects.Add(collider.gameObject);
                }
            }
        }
        return objects;
    }
  
    

  
}
