using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour
{
    public mapmarkType markType;
    private mapmarkType preMarkType = mapmarkType.None;
    public bool IsMarker = false;

    public IObjectPool<GameObject> pool;
    public GameObject marker;

    public bool CheckChangeType()
    { 
        if(markType !=preMarkType)
        {
            preMarkType = markType;
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsMarker) return;

        if (ReferenceEquals(other.gameObject,MiniMap.GetInstance.mapCamera.gameObject))
        {
            IsMarker = true;
            MiniMap.GetInstance.AddMarkerData(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsMarker) return;
        if (ReferenceEquals(other.gameObject, MiniMap.GetInstance.mapCamera.gameObject))
        {
            IsMarker = false;
            pool.Release(marker);
            MiniMap.GetInstance.RemoveMarkerData(this.GetInstanceID());
        }
    }

    private void OnDisable()
    {
        if (!IsMarker || pool == null) return; // pool�� null�̸� ���⼭ �ٷ� ����
        IsMarker = false;
        if (marker != null) // marker�� null�� �ƴ� ��쿡�� release ����
            pool.Release(marker);

        // MiniMap �ν��Ͻ��� null�� �ƴ� ��쿡�� ȣ��
        if (MiniMap.GetInstance != null)
            MiniMap.GetInstance.RemoveMarkerData(this.GetInstanceID());
    }

}
