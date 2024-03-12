using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

// 청크 좌표
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

// 월드값
public static class WorldData
{
    // 청크 크기 설정
    public static readonly int chunkSize = 1000;

    // 시드값
    public static int seed=21434312;
}

[System.Serializable]
public class WorldSettings
{
    [Header("성능")]
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

    // 오브젝트 풀링으로 관리
    #region objectpool

    // 월드 타입에따라 재사용 할 수 있도록
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

    // 딕셔너리에서 풀찾아서 반환
    private WorldChunk GetDictionaryPool(worldType type)
    {
        // Dictionary에서  해당 type으로 이루어진게 있는지확인 -> 없으면 생성
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
        //시드값으로 랜덤함수 초기화설정
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

                // 비동기로 청크 생성 처리
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
        // 주변 청크 범위를 설정
        int nearbyRange = 2;

        // 주변 청크 중 하나라도 섬인지 여부를 나타내는 변수
        bool isNearbyIsland = false;

        // 청크 주변의 월드 유형 확인
        for (int xOffset = chunkPos.x - nearbyRange; xOffset <= chunkPos.x + nearbyRange; xOffset++)
        {
            for (int zOffset = chunkPos.z - nearbyRange; zOffset <= chunkPos.z + nearbyRange; zOffset++)
            {
                if (xOffset == chunkPos.x && zOffset == chunkPos.z)
                    continue; // 현재 청크와 동일한 청크는 무시

                worldType nearbyChunk = GetChunkWorldType(xOffset, zOffset);

                // 주변 청크 중 하나라도 섬인 경우 플래그 설정
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
            selectedType = worldType.ocean; // 섬 주변에는 무조건 바다 생성
        }
        else
        {
            selectedType = GetChunkWorldType(chunkPos.x, chunkPos.z);
        }

        // 선택된 월드 유형에 따라 해당 WorldChunk를 생성하여 반환
        return GetDictionaryPool(selectedType);
    }

    private IEnumerator CreateChunkAsync(ChunkPos pos)
    {
        // 이미 생성된 청크와 중복되지 않는지 확인
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

        yield return null; // 비동기 작업 완료 시, 다음 프레임까지 대기
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
        // 플레이어가 위치한 청크번호 확인 
        playerCurrentChunk = GetChunkFromVector3(player.position);

        // if 이전에 있던 청크와 다른 청크인지 확인 -> 다를 경우 갱신
        if (!playerCurrentChunk.Equals(playerPreviousChunk)) CheckViewDistanceChunk();

    }

    ChunkPos GetChunkFromVector3(Vector3 pos)
    {
        return new ChunkPos(pos);
    }

    void CheckViewDistanceChunk()
    {
        // 플레이어 청크 갱신
        playerPreviousChunk = playerCurrentChunk;

        // 플레이어가 서있는곳의 청크 좌표
        ChunkPos pos = GetChunkFromVector3(player.position);

        // 활성화 되어있던 청크 리스트
        List<WorldChunk> previousActiveChunkList = new List<WorldChunk>(activeChunkList);
        activeChunkList.Clear();
        createChunkList.Clear();
        deactiveChunkList.Clear();

        // 스네일 회전 패턴으로 체크
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
                // 활성화가 되어있는지 확인
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

                // 활성화가 안되어있는 청크일 경우 생성
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

        // 남은건 안보이는 청크이니 비활성화
        foreach (WorldChunk chunk in previousActiveChunkList)
        {
            deactiveChunkList.Add(chunk);
        }
    }
}
