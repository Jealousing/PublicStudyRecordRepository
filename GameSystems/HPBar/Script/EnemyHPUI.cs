using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;

public class EnemyHPUI : MonoBehaviour
{
    [SerializeField] private EnemyInfo info;
    IObjectPool<EnemyHPUI> poolData;
    EnemyHPUIManager manager;
    Camera cam;
    float destroyAngle;
    [SerializeField] private Slider sliderHP;
    // uitext의 단점은 보완해서 나옴
    [SerializeField] private TextMeshProUGUI textHP;


    float minDistance = 2.0f;
    float maxDistance = 10.0f;

    float minScale = 0.5f;
    float maxScale = 1.5f;

    // HPBar 풀링에 대한 셋팅
    public void Setting(EnemyInfo infodata,Camera camera,  EnemyHPUIManager manager ,float destroyAngle)
    {
        info = infodata;
        this.manager = manager;
        this.destroyAngle = destroyAngle;
        cam = camera;
        HPBarPosUpdate();
        HPBarScaleUpdate();
    }

    // 오브젝트풀에 연결
    public void SetPool(IObjectPool<EnemyHPUI> pool)
    {
        poolData = pool;
    }

    // 오브젝트풀 반환
    public void Destroy()
    {
        info.HPUI = null;
        poolData.Release(this);
    }

    void Update()
    {
        if(HPBarCheck())
        {
            HPBarUpdate();
        }
    }

    private void LateUpdate()
    {
        HPBarPosUpdate();
        HPBarScaleUpdate();
    }

    // HPbar의 hp정보를 업데이트
    void HPBarUpdate()
    {
        if (sliderHP != null) sliderHP.value = Utils.Percent(ref info.lerpHP, info.HP, info.maxHP);
        if (textHP != null)
        {
            //textHP.text = $"{info.HP:F0}/{info.maxHP:F0}";
            float healthPercentage = (float)info.HP / info.maxHP * 100f;
            textHP.text = $"{healthPercentage:F0}%";
        }
    }

    // 사라지는 조건을 확인하여 충족한다면 반환
    bool HPBarCheck()
    {
        if (!manager.checkRaycast(info.transform.position, destroyAngle))
        {
            Destroy();
            return false;
        }
        return true;
    }

    // 위치업데이트
    void HPBarPosUpdate()
    {
        this.transform.position = cam.WorldToScreenPoint(
                info.transform.position + new Vector3(0, info.collider.bounds.extents.y+0.5f, 0));
    }

    // 크기 업데이트
    void HPBarScaleUpdate()
    {
        float distance = Vector3.Distance(info.transform.position, manager.playerTr.position);
        float normalizedDistance = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
        float scale = Mathf.Lerp(maxScale, minScale, normalizedDistance); 

        sliderHP.transform.localScale = new Vector3(scale, scale, 1.0f);
    }

}
