using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuikaGame3D
{
    // 게임의 컨트롤을 담당하는 부분
    public class SuikaGameController : Singleton<SuikaGameController>
    {
        // 플레이어 부분과 과일부분 정보
        public GameObject player;
        public GameObject curFruit;

        // 과일 메테리얼 정보 및 다음 과일에 대한 정보 이미지로 전달
        public Image[] fruitImage;
        public Image nextFruit;

        // 과일 몇번 부터 몇번까지 뽑게 할것인지
        int minFruitNum = 0;
        int maxFruitNum = 5;

        // 과일 대기를 큐로 관리
        private Queue<int> fruitQueue = new Queue<int>();

        // movement 정보
        public float movementSpeed = 25f;
        public float minLimit;
        public float maxLimit;

        //camera 정보
        public Transform target;
        public Transform cam;
        public float distance = 5f;
        public float sensitivity = 25f;

        private float azimuth = 0f;
        private float elevation = 0;

        // 던지기 쿨타임
        private WaitForSeconds throwWaitSec;
        bool flag = false;

        // 과일이 떨어질 위치에 대한 표시
        private LineRenderer lineRenderer;

        // 이벤트함수
        private void Start()
        {
            // 라인렌더러 설정
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            // 커서 잠금 및 던지기 쿨타임 설정, 액션이벤트를 통해 게임 시작과 종료때 실행할 함수 전달
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            throwWaitSec = new WaitForSeconds(1f);
            SuikaGameManager.GetInstance.gameStartEvent += GameStart;
            SuikaGameManager.GetInstance.gameEndEvent += GameEnd;
            // 게임시작
            GameStart();
        }

        private void Update()
        {
            // 이동과 카메라에 대한 처리 및 커서 관리
            move();
            CameraController();
            CursorControl();
            ThrowPointLine();
            if (curFruit != null) curFruit.transform.localPosition = Vector3.zero;
        }

        // 게임시작시 호출
        private void GameStart()
        {
            // 플레그 종료 및 과일큐 청소 및 신규 과일 전달
            flag = false;
            fruitQueue.Clear();
            fruitQueue.Enqueue(Random.Range(minFruitNum, maxFruitNum));
            fruitQueue.Enqueue(Random.Range(minFruitNum, maxFruitNum));

            // 컨트롤 시스템 시작
            StartCoroutine(ThrowFruitControl());
        }

        // 게임 종료시 호출
        private void GameEnd()
        {
            // 발사 안한 과일이 있을경우에 대한 처리
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
            // 라인렌더러 시작위치
            lineRenderer.SetPosition(0, player.transform.position + new Vector3(0,-1f,0));

            // 종료 위치 찾기
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 100f))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }

        // 마우스 커서 관리
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

        // 카메라 컨트롤 관리
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

        // 과일 던지기에 대한 컨트롤부분
        IEnumerator ThrowFruitControl()
        {
            yield return throwWaitSec;

            // 과일 생성
            curFruit = SuikaGameManager.GetInstance.GetDictionaryPool(fruitQueue.Dequeue());
            curFruit.transform.parent = player.transform;

            // 다음과일 알려주기
            Image temp = fruitImage[fruitQueue.Peek()];
            nextFruit.color = temp.color;
            nextFruit.sprite = temp.sprite;

            // 큐에 대기할 과일 추가
            fruitQueue.Enqueue(Random.Range(minFruitNum, maxFruitNum));

            // 좌표 보정
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

        // 조종부분
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

        // 좌표보정
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
