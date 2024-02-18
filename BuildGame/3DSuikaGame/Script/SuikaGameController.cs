using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuikaGame3D
{
    // ������ ��Ʈ���� ����ϴ� �κ�
    public class SuikaGameController : Singleton<SuikaGameController>
    {
        // �÷��̾� �κа� ���Ϻκ� ����
        public GameObject player;
        public GameObject curFruit;

        // ���� ���׸��� ���� �� ���� ���Ͽ� ���� ���� �̹����� ����
        public Image[] fruitImage;
        public Image nextFruit;

        // ���� ��� ���� ������� �̰� �Ұ�����
        int minFruitNum = 0;
        int maxFruitNum = 5;

        // ���� ��⸦ ť�� ����
        private Queue<int> fruitQueue = new Queue<int>();

        // movement ����
        public float movementSpeed = 25f;
        public float minLimit;
        public float maxLimit;

        //camera ����
        public Transform target;
        public Transform cam;
        public float distance = 5f;
        public float sensitivity = 25f;

        private float azimuth = 0f;
        private float elevation = 0;

        // ������ ��Ÿ��
        private WaitForSeconds throwWaitSec;
        bool flag = false;

        // ������ ������ ��ġ�� ���� ǥ��
        private LineRenderer lineRenderer;

        // �̺�Ʈ�Լ�
        private void Start()
        {
            // ���η����� ����
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            // Ŀ�� ��� �� ������ ��Ÿ�� ����, �׼��̺�Ʈ�� ���� ���� ���۰� ���ᶧ ������ �Լ� ����
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            throwWaitSec = new WaitForSeconds(1f);
            SuikaGameManager.GetInstance.gameStartEvent += GameStart;
            SuikaGameManager.GetInstance.gameEndEvent += GameEnd;
            // ���ӽ���
            GameStart();
        }

        private void Update()
        {
            // �̵��� ī�޶� ���� ó�� �� Ŀ�� ����
            move();
            CameraController();
            CursorControl();
            ThrowPointLine();
            if (curFruit != null) curFruit.transform.localPosition = Vector3.zero;
        }

        // ���ӽ��۽� ȣ��
        private void GameStart()
        {
            // �÷��� ���� �� ����ť û�� �� �ű� ���� ����
            flag = false;
            fruitQueue.Clear();
            fruitQueue.Enqueue(Random.Range(minFruitNum, maxFruitNum));
            fruitQueue.Enqueue(Random.Range(minFruitNum, maxFruitNum));

            // ��Ʈ�� �ý��� ����
            StartCoroutine(ThrowFruitControl());
        }

        // ���� ����� ȣ��
        private void GameEnd()
        {
            // �߻� ���� ������ ������쿡 ���� ó��
            if (curFruit != null)
            {
                SuikaGameFruit temp =  curFruit.GetComponent<SuikaGameFruit>();
                temp.trigger = true;
                temp.Release(false);
                curFruit = null;
            }
            flag = true;
        }

        private void ThrowPointLine()
        {
            if (!lineRenderer.enabled) return;
            // ���η����� ������ġ
            lineRenderer.SetPosition(0, player.transform.position + new Vector3(0,-1f,0));

            // ���� ��ġ ã��
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 100f))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }

        // ���콺 Ŀ�� ����
        private void CursorControl()
        {
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // ī�޶� ��Ʈ�� ����
        private void CameraController()
        {
            if (Cursor.visible) return;
            float mouseX = Input.GetAxis("Mouse X")  * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y")  * sensitivity;

            azimuth += mouseX;
            azimuth = Mathf.Clamp(azimuth, -110f, 30f);

            elevation += mouseY;
            elevation = Mathf.Clamp(elevation, 1, 90);

            Vector3 offset = new Vector3
                (
                    distance * Mathf.Sin(elevation * Mathf.Deg2Rad) * Mathf.Cos(azimuth * Mathf.Deg2Rad),
                    distance * Mathf.Cos(elevation * Mathf.Deg2Rad),
                    distance * Mathf.Sin(elevation * Mathf.Deg2Rad) * Mathf.Sin(azimuth * Mathf.Deg2Rad)
                );
            cam.position = target.position + offset;
            cam.LookAt(target);
        }

        // ���� �����⿡ ���� ��Ʈ�Ѻκ�
        IEnumerator ThrowFruitControl()
        {
            yield return throwWaitSec;

            // ���� ����
            curFruit = SuikaGameManager.GetInstance.GetDictionaryPool(fruitQueue.Dequeue());
            curFruit.transform.parent = player.transform;

            // �������� �˷��ֱ�
            Image temp = fruitImage[fruitQueue.Peek()];
            nextFruit.color = temp.color;
            nextFruit.sprite = temp.sprite;

            // ť�� ����� ���� �߰�
            fruitQueue.Enqueue(Random.Range(minFruitNum, maxFruitNum));

            // ��ǥ ����
            curFruit.transform.localPosition = Vector3.zero;
            curFruit.transform.localRotation = Quaternion.identity;
            CorrectCoordinates();
            lineRenderer.enabled = true;
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                if (flag) yield break;
                yield return null;
            }
            lineRenderer.enabled = false;

            curFruit.GetComponent<SuikaGameFruit>().Shoot(); 
            curFruit.transform.SetParent(SuikaGameManager.GetInstance.transform.GetChild(0));
            curFruit = null;

            StartCoroutine(ThrowFruitControl());
        }

        // �����κ�
        private void move()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized;

            if (movement == Vector3.zero) return;
            player.transform.Translate(movement * movementSpeed * Time.deltaTime);
            CorrectCoordinates();
        }

        // ��ǥ����
        private void CorrectCoordinates()
        {
            float temp = curFruit != null ? (curFruit.gameObject.transform.localScale.x / 2)+0.05f : 0;
            Vector3 clampedPosition = new Vector3
                (
                    Mathf.Clamp(player.transform.position.x, minLimit + temp, maxLimit - temp),
                    player.transform.position.y,
                    Mathf.Clamp(player.transform.position.z, minLimit + temp, maxLimit - temp)
                );
            player.transform.position = clampedPosition;
        }
    }

}
