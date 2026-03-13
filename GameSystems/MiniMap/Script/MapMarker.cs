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
        if (!IsMarker || pool == null) return; // pool이 null이면 여기서 바로 리턴
        IsMarker = false;
        if (marker != null) // marker가 null이 아닌 경우에만 release 수행
            pool.Release(marker);

        // MiniMap 인스턴스가 null이 아닌 경우에만 호출
        if (MiniMap.GetInstance != null)
            MiniMap.GetInstance.RemoveMarkerData(this.GetInstanceID());
    }

}
