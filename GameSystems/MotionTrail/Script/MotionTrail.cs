using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshTrailData
{
    public GameObject body { get; private set; }
    public MeshFilter[] meshFilters { get; private set; }
    public Mesh[] meshes { get; private set; }

    public MeshTrailData(GameObject container, GameObject partPrefab, int rendererCount, SkinnedMeshRenderer[] renderers)
    {
        body = container;
        meshFilters = new MeshFilter[rendererCount];
        meshes = new Mesh[rendererCount];

        for (int i = 0; i < rendererCount; i++)
        {
            GameObject part = Object.Instantiate(partPrefab, container.transform);
            meshFilters[i] = part.GetComponent<MeshFilter>();
            meshes[i] = new Mesh();
            renderers[i].BakeMesh(meshes[i]);
            meshFilters[i].mesh = meshes[i];
        }
    }

    public void ApplyColor(float alpha)
    {
        Color colorWithAlpha = new Color(0, 0, 0, alpha);
        foreach (var filter in meshFilters)
        {
            if (filter.TryGetComponent(out MeshRenderer renderer))
            {
                renderer.material.SetColor("_Color", colorWithAlpha);
            }
        }
    }
}

public class MotionTrail : MonoBehaviour
{
    private const float ColorAlphaFactor = 0.5f;

    [SerializeField] private GameObject armature;
    [SerializeField] private Transform trailParent;
    [SerializeField] private GameObject partPrefab;
    [SerializeField, Range(1,30)] private int trailCount; 
    [SerializeField, Range(0.001f, 1f)] private float trailGap = 0.2f;

    private List<MeshTrailData> meshTrailDataList = new List<MeshTrailData>();  
    private List<Vector3> positionMemory = new List<Vector3>();
    private List<Quaternion> rotationMemory = new List<Quaternion>();

    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private int curTrailCount;

    void Start()
    {  
        InitializeTrails();
        //trailParent.gameObject.SetActive(false);
        StartCoroutine(UpdateTrailCoroutine());
    }

    private void InitializeTrails()
    {
        skinnedMeshRenderers = armature.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < trailCount; i++)
        {
            CreateTrail(i);
            InitializeMemory();
        }
        curTrailCount = trailCount;
    }

    private void InitializeMemory()
    {
        positionMemory.Add(Vector3.zero);
        rotationMemory.Add(Quaternion.identity);
    }

    private void CreateTrail(int index)
    {
        GameObject trailObject = new GameObject($"MotionTrail {index}");
        trailObject.transform.parent = trailParent;

        var trailData = new MeshTrailData(trailObject, partPrefab, skinnedMeshRenderers.Length, skinnedMeshRenderers);
        trailData.ApplyColor(CalculateAlpha(index));
        meshTrailDataList.Add(trailData);
    }
    private float CalculateAlpha(int index) =>  Mathf.Clamp01((1f - (float)index / trailCount) * ColorAlphaFactor);
    public void ActivateTrail() => trailParent.gameObject.SetActive(true);
    public void DeactivateTrail() => trailParent.gameObject.SetActive(false);

    private IEnumerator UpdateTrailCoroutine()
    {
        while (true)
        {
            UpdateMeshData();
            UpdateMemory();
            UpdateTrailCount();

            yield return new WaitForSecondsRealtime(trailGap);
        }
    }

    private void UpdateMeshData()
    {
        for (int i = meshTrailDataList.Count - 2; i >= 0; i--)
        {
            CopyMeshData(meshTrailDataList[i], meshTrailDataList[i + 1]);
        }

        for (int j = 0; j < skinnedMeshRenderers.Length; j++)
        {
            skinnedMeshRenderers[j].BakeMesh(meshTrailDataList[0].meshes[j]);
        }
    }

    private void CopyMeshData(MeshTrailData source, MeshTrailData destination)
    {
        for (int i = 0; i < source.meshes.Length; i++)
        {
            destination.meshes[i].vertices = source.meshes[i].vertices;
            destination.meshes[i].triangles = source.meshes[i].triangles;
        }
    }

    private void UpdateMemory()
    {
        positionMemory.Insert(0, transform.position);
        rotationMemory.Insert(0, Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

        if (positionMemory.Count > trailCount) positionMemory.RemoveAt(positionMemory.Count - 1);
        if (rotationMemory.Count > trailCount) rotationMemory.RemoveAt(rotationMemory.Count - 1);

        for (int i = 0; i < Mathf.Min(meshTrailDataList.Count, positionMemory.Count); i++)
        {
            meshTrailDataList[i].body.transform.position = positionMemory[i];
            meshTrailDataList[i].body.transform.rotation = rotationMemory[i];
        }
    }

    private void UpdateTrailCount()
    {
        if (curTrailCount == trailCount) return;

        if (curTrailCount > trailCount)
        {
            for (int i = curTrailCount - 1; i >= trailCount; i--)
            {
                meshTrailDataList[i].body.SetActive(false);
            }
        }
        else if (meshTrailDataList.Count < trailCount)
        {
            for (int i = meshTrailDataList.Count; i < trailCount; i++)
            {
                CreateTrail(i);
                InitializeMemory();
            }
        }

        for (int i = 0; i < trailCount; i++)
        {
            if (!meshTrailDataList[i].body.activeSelf)
            {
                meshTrailDataList[i].body.SetActive(true);
            }
            meshTrailDataList[i].ApplyColor(CalculateAlpha(i));
        }

        curTrailCount = trailCount;
    }
}
 
