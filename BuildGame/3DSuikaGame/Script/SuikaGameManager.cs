using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System;
using UnityEditor;

namespace SuikaGame3D
{
    // ���� ������ enum���� ����
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
    // �׽�Ʈ�ϸ鼭 �ְ����� ����� ���� ����
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
                if (EditorUtility.DisplayDialog("Reset Best Score", "����Ʈ ���ھ� �ʱ�ȭ�� �½��ϱ�?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteKey("Best Score");
                    editScript.LoadScore();
                }
            }
        }
    }
#endif

    // ��ü������ ���ڰ����� �����ϴ� ��ũ��Ʈ
    public class SuikaGameManager : Singleton<SuikaGameManager>
    {
        // ������ ������Ʈ Ǯ������ ����
        public List<GameObject> fruitList = new List<GameObject>();
        private Dictionary<int, IObjectPool<GameObject>> fruitPool;

        // ���ϻ��� ��ȣ
        int numbering=0;

        // ���� ����
        public TextMeshProUGUI curScoreText;
        public TextMeshProUGUI bestScoreText;
        int curScore = 0;
        [NonSerialized] public int bestScore = 0;

        // ���� ��ġ�� ȿ����
        private const string FruitSfx = "fruitSfx";
        [SerializeField] private AudioClip fruitClap;

        // ���� ���� �� ���ῡ ���� �׼� �̺�Ʈ
        public Action gameStartEvent;
        public Action gameEndEvent;

        // ���� ���� ���
        public int watermelonCnt = 0;

        // �̺�Ʈ �Լ�
        private void Awake()
        {
            // Ǯ���� �� �ְ��� �ҷ�����
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

        // ��ųʸ� ����
        private void AddDictionary(int count)
        {
            fruitPool.Add(count, new ObjectPool<GameObject>(() => CreateFruit(count),
                                                               OnGetFruit, OnReleaseFruit, OnDestroyFruit, maxSize: 10));
        }

        // ������Ʈ Ǯ�� �Լ�
        private GameObject CreateFruit(int count)
        {
            return Instantiate(fruitList[count]);
        }

        private void OnGetFruit(GameObject fruit)
        {
            // ���� ���� ���� �� �������� Ȯ��
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

       // ���� �߰�
        public void AddScore(Fruit fruit)
        {
            SoundManager.GetInstance.SFXPlay(FruitSfx, fruitClap);
            curScoreText.text = (curScore += ((int)fruit + 1)).ToString();

            // ���ھ� �����ϸ鼭 �����ȲȮ���ϱ� ( ������ 2���̻��̶�� �� �ʱ�ȭ )
            if(watermelonCnt>=2)
            {
                watermelonCnt = 0;
                StartCoroutine(CleanMapCoroutine());
            }
        }

        // ���� ����
        public void GameOver()
        {
            StartCoroutine(GameOverCoroutine());
        }

        // �ʿ��ִ� ���� �����
        private IEnumerator CleanMapCoroutine()
        {
            // ��������
            Transform parentTransform = transform.GetChild(0);

            // ������ ���߱�����
            for (int i = parentTransform.childCount - 1; i >= 0; i--)
            {
                if (parentTransform.GetChild(i).TryGetComponent(out SuikaGameFruit fruit) && fruit.gameObject.activeSelf)
                {
                    fruit.rigid.isKinematic = true;
                    fruit.rigid.useGravity = false;
                    fruit.colliderInfo.isTrigger = true;
                }
            }

            // ���� ���� ó��
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

        // ���� ���ῡ ���� �ڷ�ƾ
        private IEnumerator GameOverCoroutine()
        {
            // ���� ���߱�
            gameEndEvent();

            // ��û��
            yield return CleanMapCoroutine();

            // ����Ʈ ���� ����
            if (bestScore<curScore)
            {
                SaveScore();
            }

            // �����̽��� ���
            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

            // ���� �����
            curScore = 0;
            curScoreText.text = curScore.ToString();
            gameStartEvent();
        }

        // ���� ���� �� �ҷ�����
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
