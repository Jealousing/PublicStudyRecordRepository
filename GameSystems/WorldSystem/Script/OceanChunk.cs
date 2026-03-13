using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanChunk : WorldChunk
{
    public override void Init()
    {
        groundRenderer = ground.GetComponent<Renderer>();
        groundMaterial = new Material(groundRenderer.material);
        groundRenderer.material = groundMaterial;
    }

    public override void Set(ChunkPos pos)
    {
        chunkPos = pos;
        this.transform.position = new Vector3(pos.x * WorldData.chunkSize, 0, pos.z * WorldData.chunkSize);
        groundMaterial.SetVector("_NoiseOffset", new Vector4(pos.x * WorldData.chunkSize / 100, 0, pos.z * WorldData.chunkSize / 100, 0));
        released = false;
    }
}
