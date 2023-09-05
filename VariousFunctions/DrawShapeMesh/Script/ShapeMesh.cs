using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ShapeMesh : MonoBehaviour
{
    [NonSerialized] public Mesh baseMesh;
    [NonSerialized] public Mesh fillMesh;
    [NonSerialized] public Material baseMaterial;
    [NonSerialized] public Material fillMaterial;
    [NonSerialized] public float fillProgress = 0;

    protected abstract void SetMesh();
    protected abstract void SetMaterial();

    public abstract void DrawShape(Vector3 pos, Vector3 dir);
}
