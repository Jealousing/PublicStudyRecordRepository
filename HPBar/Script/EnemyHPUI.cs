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
    // uitext�� ������ �����ؼ� ����
    [SerializeField] private TextMeshProUGUI textHP;


    float minDistance = 2.0f;
    float maxDistance = 10.0f;

    float minScale = 0.5f;
    float maxScale = 1.5f;

    // HPBar Ǯ���� ���� ����
    public void Setting(EnemyInfo infodata,Camera camera,  EnemyHPUIManager manager ,float destroyAngle)
    {
        info = infodata;
        this.manager = manager;
        this.destroyAngle = destroyAngle;
        cam = camera;
        HPBarPosUpdate();
        HPBarScaleUpdate();
    }

    // ������ƮǮ�� ����
    public void SetPool(IObjectPool<EnemyHPUI> pool)
    {
        poolData = pool;
    }

    // ������ƮǮ ��ȯ
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

    // HPbar�� hp������ ������Ʈ
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

    // ������� ������ Ȯ���Ͽ� �����Ѵٸ� ��ȯ
    bool HPBarCheck()
    {
        if (!manager.checkRaycast(info.transform.position, destroyAngle))
        {
            Destroy();
            return false;
        }
        return true;
    }

    // ��ġ������Ʈ
    void HPBarPosUpdate()
    {
        this.transform.position = cam.WorldToScreenPoint(
                info.transform.position + new Vector3(0, info.collider.bounds.extents.y+0.5f, 0));
    }

    // ũ�� ������Ʈ
    void HPBarScaleUpdate()
    {
        float distance = Vector3.Distance(info.transform.position, manager.playerTr.position);
        float normalizedDistance = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
        float scale = Mathf.Lerp(maxScale, minScale, normalizedDistance); 

        sliderHP.transform.localScale = new Vector3(scale, scale, 1.0f);
    }

}
