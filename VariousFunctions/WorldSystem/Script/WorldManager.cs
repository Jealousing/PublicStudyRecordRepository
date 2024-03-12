using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

// ûũ ��ǥ
public class ChunkPos
{
    public int x, z;

    public ChunkPos()
    {
        x = 0;
        z = 0;
    }

    public ChunkPos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public ChunkPos(Vector3 pos)
    {
        this.x = Mathf.FloorToInt(pos.x) / WorldData.chunkSize;
        this.z = Mathf.FloorToInt(pos.z) / WorldData.chunkSize;
    }

    public bool Equals(ChunkPos other)
    {
        if (other == null) return false;
        else if (other.x == this.x && other.z == this.z) return true;
        else return false;
    }
}

// ���尪
public static class WorldData
{
    // ûũ ũ�� ����
    public static readonly int chunkSize = 1000;

    // �õ尪
    public static int seed=21434312;
}

[System.Serializable]
public class WorldSettings
{
    [Header("����")]
    public int viewDistance = 6;
    public int loadDistance = 12;
}

public class WorldManager : Singleton<WorldManager>
{
    public WorldSettings set;
    private Transform player;
    private List<WorldChunk> activeChunkList = new List<WorldChunk>();
    private List<ChunkPos> createChunkList = new List<ChunkPos>();
    private List<WorldChunk> deactiveChunkList= new List<WorldChunk>();
    private ChunkPos playerPreviousChunk;
    private ChunkPos playerCurrentChunk;

    // ������Ʈ Ǯ������ ����
    #region objectpool

    // ���� Ÿ�Կ����� ���� �� �� �ֵ���
    private Dictionary<worldType, IObjectPool<WorldChunk>> worldChunkPool;
    public GameObject[] worldTypeList;
    double[] worldTypeProbabilities = { 0.95f, 0.025f, 0.025f };

    private enum worldType
    {
        ocean = 0,
        worldType2,
        worldType3,
        worldType4,
        worldType5,
        worldType6,
    }
  
    private WorldChunk CreateChunk(worldType type)
    {
        WorldChunk chunk = Instantiate(worldTypeList[(int)type].GetComponent<WorldChunk>());
        chunk.Init();
        chunk.Set(worldChunkPool[type]);
        chunk.transform.parent = this.transform;
        return chunk;
    }

    private void OnGetChunk(WorldChunk chunk)
    {
        chunk.gameObject.SetActive(true);
    }

    private void OnReleaseChunk(WorldChunk chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    private void OnDestroyChunk(WorldChunk chunk)
    {
        Destroy(chunk.gameObject);
    }
    private void AddDictionary(worldType type)
    {
        worldChunkPool.Add(type, new ObjectPool<WorldChunk>(() => CreateChunk(type),
                                                           OnGetChunk, OnReleaseChunk, OnDestroyChunk, maxSize: Mathf.FloorToInt(Mathf.Pow(set.viewDistance * 2, 2)) + set.viewDistance*4));
    }

    // ��ųʸ����� Ǯã�Ƽ� ��ȯ
    private WorldChunk GetDictionaryPool(worldType type)
    {
        // Dictionary����  �ش� type���� �̷������ �ִ���Ȯ�� -> ������ ����
        if (!worldChunkPool.ContainsKey(type))
        {
            AddDictionary(type);
        }

        return worldChunkPool[type].Get();
    }
    #endregion

    private void Awake()
    {
        worldChunkPool = new Dictionary<worldType, IObjectPool<WorldChunk>>();
    }

    void Start()
    {
        //�õ尪���� �����Լ� �ʱ�ȭ����
        UnityEngine.Random.InitState(WorldData.seed);
        player = PlayerInfo.GetInstance.transform;
        StartCoroutine(ChunkUpdateCoroutine());
    }

    private IEnumerator ChunkUpdateCoroutine()
    {
        while (true)
        {
            if (deactiveChunkList.Count > 0)
            {
                DeactiveChunks();
            }
            if (createChunkList.Count > 0)
            {
                ChunkPos newChunkPos = createChunkList[0];
                createChunkList.RemoveAt(0);

                // �񵿱�� ûũ ���� ó��
                yield return StartCoroutine(CreateChunkAsync(newChunkPos));
            }
            yield return null;
        }
    }

    int GenerateSeed(int chunkX, int chunkZ)
    {
        string seedStr = $"{chunkX}-{chunkZ}";
        int seed = seedStr.GetHashCode();
        return seed;
    }

    worldType GetChunkWorldType(int chunkX, int chunkZ)
    {
        int seed = GenerateSeed(chunkX, chunkZ);
        System.Random random = new System.Random(seed);

        double randVal = random.NextDouble();
        double cumulativeProb = 0.0;

        for (int i = 0; i < worldTypeProbabilities.Length; i++)
        {
            cumulativeProb += worldTypeProbabilities[i];
            if (randVal < cumulativeProb)
            {
                return (worldType)i;
            }
        }

        return worldType.ocean;
    }

    private WorldChunk GenerateWorldChunkBasedOnSeed(ChunkPos chunkPos)
    {
        // �ֺ� ûũ ������ ����
        int nearbyRange = 2;

        // �ֺ� ûũ �� �ϳ��� ������ ���θ� ��Ÿ���� ����
        bool isNearbyIsland = false;

        // ûũ �ֺ��� ���� ���� Ȯ��
        for (int xOffset = chunkPos.x - nearbyRange; xOffset <= chunkPos.x + nearbyRange; xOffset++)
        {
            for (int zOffset = chunkPos.z - nearbyRange; zOffset <= chunkPos.z + nearbyRange; zOffset++)
            {
                if (xOffset == chunkPos.x && zOffset == chunkPos.z)
                    continue; // ���� ûũ�� ������ ûũ�� ����

                worldType nearbyChunk = GetChunkWorldType(xOffset, zOffset);

                // �ֺ� ûũ �� �ϳ��� ���� ��� �÷��� ����
                if (nearbyChunk != worldType.ocean)
                {
                    isNearbyIsland = true;
                    break;
                }
            }
            if (isNearbyIsland)
                break; 
        }

        worldType selectedType;

        if (isNearbyIsland)
        {
            selectedType = worldType.ocean; // �� �ֺ����� ������ �ٴ� ����
        }
        else
        {
            selectedType = GetChunkWorldType(chunkPos.x, chunkPos.z);
        }

        // ���õ� ���� ������ ���� �ش� WorldChunk�� �����Ͽ� ��ȯ
        return GetDictionaryPool(selectedType);
    }

    private IEnumerator CreateChunkAsync(ChunkPos pos)
    {
        // �̹� ������ ûũ�� �ߺ����� �ʴ��� Ȯ��
        bool isDuplicate = false;
        foreach (WorldChunk chunk in activeChunkList)
        {
            if (chunk.chunkPos.Equals(pos))
            {
                isDuplicate = true;
                break;
            }
        }

        if (!isDuplicate)
        {
            WorldChunk temp = GenerateWorldChunkBasedOnSeed(pos);
            temp.Set(pos);

            activeChunkList.Add(temp);
        }

        yield return null; // �񵿱� �۾� �Ϸ� ��, ���� �����ӱ��� ���
    }


    void DeactiveChunks()
    {
        if (deactiveChunkList.Count > 0)
        {
            WorldChunk chunkToDeactivate = deactiveChunkList[0];
            deactiveChunkList.RemoveAt(0);

            chunkToDeactivate.Release();
        }
    }


    void Update()
    {
        // �÷��̾ ��ġ�� ûũ��ȣ Ȯ�� 
        playerCurrentChunk = GetChunkFromVector3(player.position);

        // if ������ �ִ� ûũ�� �ٸ� ûũ���� Ȯ�� -> �ٸ� ��� ����
        if (!playerCurrentChunk.Equals(playerPreviousChunk)) CheckViewDistanceChunk();

    }

    ChunkPos GetChunkFromVector3(Vector3 pos)
    {
        return new ChunkPos(pos);
    }

    void CheckViewDistanceChunk()
    {
        // �÷��̾� ûũ ����
        playerPreviousChunk = playerCurrentChunk;

        // �÷��̾ ���ִ°��� ûũ ��ǥ
        ChunkPos pos = GetChunkFromVector3(player.position);

        // Ȱ��ȭ �Ǿ��ִ� ûũ ����Ʈ
        List<WorldChunk> previousActiveChunkList = new List<WorldChunk>(activeChunkList);
        activeChunkList.Clear();
        createChunkList.Clear();
        deactiveChunkList.Clear();

        // ������ ȸ�� �������� üũ
        int x = 0;
        int z = 0;
        int dx = 0;
        int dz = -1;
        int currentLayer = 1;
        int maxLayer = set.viewDistance;

        for (int i = 0; i < maxLayer * maxLayer; i++)
        {
            if (-maxLayer / 2 <= x && x <= maxLayer / 2 && -maxLayer / 2 <= z && z <= maxLayer / 2)
            {
                // Ȱ��ȭ�� �Ǿ��ִ��� Ȯ��
                ChunkPos checkChunk = new ChunkPos(pos.x + x, pos.z + z);
                bool check = false;
                for (int j = 0; j < previousActiveChunkList.Count; j++)
                {
                    if (previousActiveChunkList[j].chunkPos.Equals(checkChunk))
                    {
                        activeChunkList.Add(previousActiveChunkList[j]);
                        previousActiveChunkList.RemoveAt(j);
                        break;
                    }
                }

                // Ȱ��ȭ�� �ȵǾ��ִ� ûũ�� ��� ����
                if (!check && !createChunkList.Contains(checkChunk))
                {
                    createChunkList.Add(checkChunk);
                }
            }

            if (x == z || (x < 0 && x == -z) || (x > 0 && x == 1 - z))
            {
                int temp = dx;
                dx = -dz;
                dz = temp;
            }

            x += dx;
            z += dz;

            if (i == currentLayer * currentLayer - 1)
            {
                currentLayer++;
                if (currentLayer % 2 == 0)
                {
                    maxLayer++;
                }
            }
        }

        // ������ �Ⱥ��̴� ûũ�̴� ��Ȱ��ȭ
        foreach (WorldChunk chunk in previousActiveChunkList)
        {
            deactiveChunkList.Add(chunk);
        }
    }
}
