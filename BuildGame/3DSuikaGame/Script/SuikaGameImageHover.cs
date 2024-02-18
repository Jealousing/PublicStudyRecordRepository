using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SuikaGame3D
{
    // 색깔에 대한 과일 이름을 알려주기 위한 스크립트
    public class SuikaGameImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TextMeshProUGUI Text;
        private Image image;
        int correctionValue = 22;
        private void Start()
        {
            image = GetComponent<Image>();
        }

        // 마우스 포인터가 이미지 오브젝트위에 있을경우 부르는 이벤트 함수로 과일 종류에 대한 설명 출력
        public void OnPointerEnter(PointerEventData eventData)
        {
            Text.gameObject.SetActive(true);
            Text.text = gameObject.name;
            Text.transform.position = this.transform.position + transform.right * 150f + transform.up * 80f + new Vector3(0,0,-5);
            Text.color = FruitColor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Text.gameObject.SetActive(false);
        }

        Color FruitColor()
        {
            Color temp = Color.clear;

            if (image.sprite && image.sprite.texture.isReadable)
            {
                int centerX = image.sprite.texture.width / 2 + correctionValue;
                int centerY = image.sprite.texture.height / 2 + correctionValue;
                temp = image.sprite.texture.GetPixel(centerX, centerY);
            }

            if (temp == Color.clear) temp = image.color;

            return temp;
        }
    }
}