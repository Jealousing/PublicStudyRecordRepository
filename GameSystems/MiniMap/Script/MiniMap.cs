using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

// 미니맵에 표시할 마크 타입 정리
public  enum mapmarkType
{
    Quest_Available,
    Quest_Completed,
    Quest_Area,
    NPC,
    Enemy,
    Enemy_QuestTarget,

    None,
}

public class MiniMap : Singleton<MiniMap>, IPointerEnterHandler, IPointerExitHandler
{
    public Camera mapCamera;
    public BoxCollider cameraCollider;
    [SerializeField] Transform player;
    [SerializeField] RectTransform mapRect;
    [SerializeField] RectTransform  playerImage;
    [SerializeField] Slider slider;

    private float zoomSpeed = 100f;
    private float minZoom = 20f;
    private float maxZoom = 500f;
    private float currentZoom = 100f;
    private bool IsMouseOverMap = false;

    public Sprite[] markList;
    Dictionary<int, MapMarker> marker = new Dictionary<int, MapMarker>();
    IObjectPool<GameObject> markerPool;
    public GameObject markerPrefab;
    public GameObject poolParent;

    private void Start()
    {
        // 미니맵 scale에 따른 슬라이드바 조정
        slider.minValue = minZoom;
        slider.maxValue = maxZoom;
        slider.value = currentZoom;
    }

    private void Update()
    {
        HandleMiniMapZoom();
        UpdateMinimapBase(); 
        UpdateMarker();
    }
    private void Awake()
    {
        markerPool = new ObjectPool<GameObject>(CreateMarker, OnGetMarker, OnReleaseMarker, OnDestroyMarker, maxSize: 10);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOverMap = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOverMap = false;
    }

    public void ZoomIn()
    {
        Zoom(-1 * zoomSpeed);
    }

    public void ZoomOut()
    {
        Zoom(zoomSpeed);
    }
    private void Zoom(float zoomAmount)
    {
        currentZoom = mapCamera.orthographicSize;
        currentZoom -= zoomAmount;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        mapCamera.orthographicSize = currentZoom;
        slider.value=currentZoom;
        cameraCollider.size = new Vector3(currentZoom * 2, currentZoom * 2, mapCamera.farClipPlane);
        cameraCollider.center = new Vector3(0, 0, mapCamera.farClipPlane / 2);
    }
    private void HandleMiniMapZoom()
    {
        if (IsMouseOverMap)
        {
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0)
            {
                Zoom(zoomDelta * zoomSpeed);
            }
        }
    }

    // 플레이어를 기준으로 미니맵 배경과 플레이어가 보고있는 방향을 표시
    private void UpdateMinimapBase()
    {
        if (IsMouseOverMap) return;
        mapCamera.transform.position = player.position + new Vector3(0, 100f, 0);
        playerImage.transform.rotation =
            Quaternion.Euler(0f, 0f,-Quaternion.LookRotation(player.transform.forward, Vector3.up).eulerAngles.y);
    }


    public void AddMarkerData(MapMarker data)
    {
        if (!marker.ContainsKey(data.GetInstanceID()))
        {
            marker.Add(data.GetInstanceID(), data);
            data.marker = markerPool.Get();
            data.pool = markerPool;
            UpdateMarkType(data);                
        }
    }

    public void UpdateMarkType(MapMarker data)
    {
        if (data.marker.TryGetComponent(out Image image))
        {
            switch (data.markType)
            {
                case mapmarkType.NPC:
                    image.sprite = markList[0];
                    image.color = Color.green;
                    break;
                case mapmarkType.Quest_Available:
                    image.sprite = markList[1];
                    image.color = Color.yellow;
                    break;
                case mapmarkType.Quest_Completed:
                    image.sprite = markList[2];
                    image.color = Color.yellow;
                    break;
                case mapmarkType.Quest_Area:
                    image.sprite = markList[3];
                    image.color = Color.blue;
                    break;
                case mapmarkType.Enemy:
                    image.sprite = markList[0];
                    image.color = Color.red;
                    break;
                case mapmarkType.Enemy_QuestTarget:
                    image.sprite = markList[0];
                    image.color = new Color(1.0f, 0.65f, 0.0f);
                    break;
                default:
                    image.sprite = markList[0];
                    image.color = Color.black;
                    break;
            }
        }
    }


    public void RemoveMarkerData(int instanceID)
    {
        if (marker.ContainsKey(instanceID))
        {
            marker.Remove(instanceID);
        }
    }

    public MapMarker GetMapMarker(int instanceID)
    {
        MapMarker mapMarker = null;
        marker.TryGetValue(instanceID, out mapMarker);
        return mapMarker;
    }

    private GameObject CreateMarker()
    {
        GameObject mapMarker = Instantiate(markerPrefab);
        mapMarker.transform.SetParent(poolParent.transform, false);
        return mapMarker;
    }

    private void OnGetMarker(GameObject mapMarker)
    {
        mapMarker.gameObject.SetActive(true);
    }

    private void OnReleaseMarker(GameObject mapMarker)
    {
        mapMarker.gameObject.SetActive(false);
    }

    private void OnDestroyMarker(GameObject mapMarker)
    {
        Destroy(mapMarker);
    }
    private void UpdateMarker()
    {
        foreach (var pair in marker)
        {
            MapMarker mapMarker = pair.Value;
            if (mapMarker != null && mapMarker.gameObject.activeSelf)
            {
                Vector3 markerPosition = mapMarker.transform.position - player.position;
                markerPosition *= mapRect.rect.width / (2f * currentZoom);
                mapMarker.marker.transform.localPosition = new Vector3(markerPosition.x, markerPosition.z, 0);

                // 마커 변경점이있으면 변환
                if (mapMarker.CheckChangeType()) UpdateMarkType(mapMarker);
            }
        }
    }


}
