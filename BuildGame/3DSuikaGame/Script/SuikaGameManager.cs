using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System;
using UnityEditor;

namespace SuikaGame3D
{
    // 과일 종류를 enum으로 관리
    public enum Fruit
    {
        Cherry = 0,
        Strawberry,
        Grapes,
        Lemon,
        Orange,
        Apple,
        Pear,
        Peach,
        Pineapple,
        Melon,
        Watermelon
    }

#if UNITY_EDITOR
    // 테스트하면서 최고기록을 지우기 위한 에딧
    [CustomEditor(typeof(SuikaGameManager))]
    public class SuikaGameManagerEditor : Editor
    {
        SuikaGameManager editScript;
        private void OnEnable()
        {
            editScript = (SuikaGameManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset BestScore"))
            {
                if (EditorUtility.DisplayDialog("Reset Best Score", "베스트 스코어 초기화가 맞습니까?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteKey("Best Score");
                    editScript.LoadScore();
                }
            }
        }
    }
#endif

    // 전체적으로 수박게임을 관리하는 스크립트
    public class SuikaGameManager : Singleton<SuikaGameManager>
    {
        // 과일을 오브젝트 풀링으로 관리
        public List<GameObject> fruitList = new List<GameObject>();
        private Dictionary<int, IObjectPool<GameObject>> fruitPool;

        // 과일생성 번호
        int numbering=0;

        // 점수 관리
        public TextMeshProUGUI curScoreText;
        public TextMeshProUGUI bestScoreText;
        int curScore = 0;
        [NonSerialized] public int bestScore = 0;

        // 과일 합치기 효과음
        private const string FruitSfx = "fruitSfx";
        [SerializeField] private AudioClip fruitClap;

        // 게임 시작 및 종료에 대한 액션 이벤트
        public Action gameStartEvent;
        public Action gameEndEvent;

        // 수박 갯수 등록
        public int watermelonCnt = 0;

        // 이벤트 함수
        private void Awake()
        {
            // 풀생성 및 최고기록 불러오기
            fruitPool = new Dictionary<int, IObjectPool<GameObject>>();
            LoadScore();
        }

        #region pool
        public GameObject GetDictionaryPool(int count)
        {
            if (!fruitPool.ContainsKey(count))
            {
                AddDictionary(count);
            }
            return fruitPool[count].Get();
        }

        // 딕셔너리 생성
        private void AddDictionary(int count)
        {
            fruitPool.Add(count, new ObjectPool<GameObject>(() => CreateFruit(count),
                                                               OnGetFruit, OnReleaseFruit, OnDestroyFruit, maxSize: 10));
        }

        // 오브젝트 풀링 함수
        private GameObject CreateFruit(int count)
        {
            return Instantiate(fruitList[count]);
        }

        private void OnGetFruit(GameObject fruit)
        {
            // 과일 정보 설정 및 수박인지 확인
            SuikaGameFruit suikaGameFruit = fruit.GetComponent<SuikaGameFruit>();
            suikaGameFruit.Set(fruitPool[(int)suikaGameFruit.fruit], numbering++);
            if(suikaGameFruit.fruit==SuikaGame3D.Fruit.Watermelon) watermelonCnt++;
            fruit.SetActive(true);
        }

        private void OnReleaseFruit(GameObject fruit)
        {
            fruit.SetActive(false);
        }

        private void OnDestroyFruit(GameObject fruit)
        {
            Destroy(fruit);
        }
        #endregion

       // 점수 추가
        public void AddScore(Fruit fruit)
        {
            SoundManager.GetInstance.SFXPlay(FruitSfx, fruitClap);
            curScoreText.text = (curScore += ((int)fruit + 1)).ToString();

            // 스코어 갱신하면서 진행상황확인하기 ( 수박이 2개이상이라면 맵 초기화 )
            if(watermelonCnt>=2)
            {
                watermelonCnt = 0;
                StartCoroutine(CleanMapCoroutine());
            }
        }

        // 게임 종료
        public void GameOver()
        {
            StartCoroutine(GameOverCoroutine());
        }

        // 맵에있는 과일 지우기
        private IEnumerator CleanMapCoroutine()
        {
            // 과일정보
            Transform parentTransform = transform.GetChild(0);

            // 움직임 멈추기위해
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                if (parentTransform.GetChild(i).TryGetComponent(out SuikaGameFruit fruit) && fruit.gameObject.activeSelf)
                {
                    fruit.rigid.isKinematic = true;
                    fruit.rigid.useGravity = false;
                    fruit.colliderInfo.isTrigger = true;
                }
            }

            // 남은 과일 처리
            float tickTime = 0.25f;
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                if (parentTransform.GetChild(i).TryGetComponent(out SuikaGameFruit fruit) && fruit.gameObject.activeSelf)
                {
                    fruit.Release(true);
                    yield return new WaitForSeconds(tickTime);
                    if (tickTime >= 0.10f)
                    {
                        tickTime -= 0.05f;
                    }
                }
            }
        }

        // 게임 종료에 대한 코루틴
        private IEnumerator GameOverCoroutine()
        {
            // 게임 멈추기
            gameEndEvent();

            // 맵청소
            yield return CleanMapCoroutine();

            // 베스트 점수 갱신
            if (bestScore<curScore)
            {
                SaveScore();
            }

            // 스페이스바 대기
            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

            // 게임 재시작
            curScore = 0;
            curScoreText.text = curScore.ToString();
            gameStartEvent();
        }

        // 점수 저장 및 불러오기
        public void SaveScore()
        {
            bestScore = curScore;
            bestScoreText.text = bestScore.ToString();
            PlayerPrefs.SetInt("Best Score", bestScore);
        }
        public void LoadScore()
        {
            bestScore = PlayerPrefs.GetInt("Best Score", 0);
            bestScoreText.text = bestScore.ToString();
        }
    }
}
