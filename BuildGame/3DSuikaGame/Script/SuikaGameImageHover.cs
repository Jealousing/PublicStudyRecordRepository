using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SuikaGame3D
{
    // ���� ���� ���� �̸��� �˷��ֱ� ���� ��ũ��Ʈ
    public class SuikaGameImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TextMeshProUGUI Text;
        private Image image;
        int correctionValue = 22;
        private void Start()
        {
            image = GetComponent<Image>();
        }

        // ���콺 �����Ͱ� �̹��� ������Ʈ���� ������� �θ��� �̺�Ʈ �Լ��� ���� ������ ���� ���� ���
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