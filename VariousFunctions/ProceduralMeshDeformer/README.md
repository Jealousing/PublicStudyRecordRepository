
# ProceduralMeshDeformer  
Unity에서 리본이 허벅지를 눌러서 변형 효과를 주는 실시간 메시 변형 시스템입니다. 이 시스템은 캐릭터 애니메이션에서 허벅지 부위에 리본이 눌리는 듯한 효과를 자연스럽게 구현할 수 있도록 돕습니다.
  
## 소개
이 프로젝트는 Unity에서 허벅지 중심을 기준으로 메쉬를 압축하고, 그에 따라 리본 형태의 오브젝트가 자연스럽게 눌리는 효과를 구현하는 스크립트입니다.

기존에는 셰이더에서 처리해야 했던 기능을 c# 스크립트로 구현하였으며, 메시 분할(Subdivision), 중심점 계산, 방향 벡터 그룹화, 보간 압축 등의 다양한 함수를 포함하고 있습니다. 이를 통해 실시간으로 메쉬를 변형하고, 압축된 영역을 시각적으로 구현할 수 있습니다.

## 유튜브 영상
  [![Video Label](http://img.youtube.com/vi/UEzHz86ewXs/0.jpg)](https://youtu.be/UEzHz86ewXs)

## 주요 기능
* 다리 중심 기준 압축(Squeeze): 허벅지와 리본의 중심을 기준으로 메쉬가 압축되어 눌리는 효과를 생성합니다.
* 실시간 메시 분할 (Subdivision): 실시간으로 메시의 세분화 및 분할을 처리합니다.
* Coroutine 기반 비동기 메시 처리: **Coroutine**을 이용한 비동기 처리를 통해 프레임 드랍을 최소화하고, 실시간 변형이 부드럽게 이루어지도록 합니다.
* **OnValidate**를 통한 에디터에서 실시간 조정 가능: 유니티 에디터에서 실시간으로 변형을 확인하며 조정할 수 있습니다. 
 

## 인스펙터 설정 항목
| 변수 | 설명 |
|------|------|
| `squeezeFactor` | 압축 강도 (1에 가까울수록 약함) |
| `bulgeFactor` | 눌림에 의하여 튀어나오는 살이 튀어나오는 정도 |
| `subdivisionLevel` | 메시 분할 레벨 |
| `outerBlendHeight` | 눌림 외부 블렌딩 범위높이 |
| `innerBlendHeight` | 눌림 내부 블렌딩 범위높이 |
| `bowKnotHeight` | 리본 밴드가 위치한 Y 좌표 |
| `squeezeHeight` | 실제로 눌림의 영향 범위 | 

## 주요 코드 설명
   
1. 메쉬 분할 (Subdivision)
메쉬의 영향을 받는 부분을 SubdivideMeshAsync 함수로 실시간으로 분할합니다. 이를 통해 더 세밀한 변형 효과를 줄 수 있습니다.

```C#
 void SubdivideTriangle(Vector3 v0, Vector3 v1, Vector3 v2, int level, List<Vector3> vertices, List<int> triangles)
    {
        if (level == 0)
        {
            triangles.Add(AddVertex(v0, vertices));
            triangles.Add(AddVertex(v1, vertices));
            triangles.Add(AddVertex(v2, vertices));
            return;
        }

        int i0 = AddVertex(v0, vertices);
        int i1 = AddVertex(v1, vertices);
        int i2 = AddVertex(v2, vertices);
        // 중간점 계산 (중복 방지 위해 캐시 사용)
        int im01 = GetOrCreateMidpoint(v0, v1, vertices);
        int im12 = GetOrCreateMidpoint(v1, v2, vertices);
        int im20 = GetOrCreateMidpoint(v2, v0, vertices);
        // 분할된 삼각형 재귀 처리
        SubdivideTriangle(vertices[i0], vertices[im01], vertices[im20], level - 1, vertices, triangles);
        SubdivideTriangle(vertices[im01], vertices[i1], vertices[im12], level - 1, vertices, triangles);
        SubdivideTriangle(vertices[im12], vertices[i2], vertices[im20], level - 1, vertices, triangles);
        SubdivideTriangle(vertices[im01], vertices[im12], vertices[im20], level - 1, vertices, triangles);
    }

    int GetOrCreateMidpoint(Vector3 a, Vector3 b, List<Vector3> vertices)
    {
        Edge edge = new Edge(a, b);
        if (edgeMidpointCache.TryGetValue(edge, out int index))
            return index;

        Vector3 mid = (a + b) * 0.5f;
        index = AddVertex(mid, vertices);
        edgeMidpointCache[edge] = index;
        return index;
    }

    private int AddVertex(Vector3 vertex, List<Vector3> vertices)
    {
        Vector3Int key = new Vector3Int(
            Mathf.RoundToInt(vertex.x / precision),
            Mathf.RoundToInt(vertex.y / precision),
            Mathf.RoundToInt(vertex.z / precision)
        );

        if (legVertexCache.TryGetValue(key, out int index))
            return index;

        index = vertices.Count;
        legVertexCache[key] = index;
        vertices.Add(vertex);
        return index;
    }
```

2. 중심 계산 (ComputeLegHeightCenters)
다리의 중심을 계산하여 눌림 효과가 적용될 범위와 위치를 추적합니다. 이 중심을 기준으로 다리의 메쉬를 압축합니다.

```C#
        Dictionary<float, List<Vector3>> heightGroups = new();
        // 각 정점을 월드 좌표로 변환하고, Y 기준으로 그룹화
        foreach (var vertex in legOriginalVertices)
        {
            Vector3 worldVertex = localToWorld.MultiplyPoint3x4(vertex);
            float height = Mathf.Floor(worldVertex.y * heightRoundingFactor) / heightRoundingFactor;
            // 높이값을 키로 정점 그룹 생성
            if (!heightGroups.ContainsKey(height))
                heightGroups[height] = new List<Vector3>();

            heightGroups[height].Add(worldVertex);
        }
        // 각 그룹의 중심점 계산 (XZ 평균)
        foreach (var pair in heightGroups)
        {
            Vector3 avgCenter = Vector3.zero;
            foreach (var pos in pair.Value)
                avgCenter += new Vector3(pos.x, 0, pos.z);

            avgCenter /= pair.Value.Count;
            legHeightToCenter[pair.Key] = avgCenter;
            legSortedHeights.Add(pair.Key);
        }
``` 

3. 다리 압축 (ApplyLegCompressionAsync)
다리 중심을 기준으로 압축이 적용되며, ApplyLegCompressionAsync 함수는 이 작업을 비동기적으로 처리합니다.

```C#
            if (height < minHeight - outerBlendHeight || height > maxHeight + outerBlendHeight)
            {
                legDeformedVertices[i] = legOriginalVertices[i];
            }
            else
            {
                // 중심으로부터 거리 계산
                Vector3 center = InterpolateHeightCenter(height);
                Vector3 offset = new Vector3(worldVertex.x - center.x, 0, worldVertex.z - center.z);
                // 방향과 거리 계산 후 압축 적용
                Vector3 direction = offset.normalized;
                float distance = offset.magnitude;
                float compressedDistance = distance * GetSqueezeFactor(height);
                
                Vector3 squeezedVertex = center + direction * compressedDistance;
                squeezedVertex.y = worldVertex.y;
                // 로컬 좌표로 변환 후 저장
                legDeformedVertices[i] = worldToLocal.MultiplyPoint3x4(squeezedVertex);
            }
``` 

4. 리본 끈 정점 방향 그룹화 (GroupByDirectionFromCenter)
리본 형태의 메시 정점들을 그룹화하여 부드럽게 눌림 효과를 구현합니다.

```C# 
            Vector3 direction = new Vector3(worldVertex.x - heightCenter.x, 0, worldVertex.z - heightCenter.z).normalized;
 
            bool foundGroup = false;
            float maxDotProduct = -1f;  
            int bestDirectionKey = 0;
 
            foreach (var group in bowKnotDirectionGroups)
            {
                // 각 방향 벡터의 첫 번째 벡터와 비교하여 얼마나 유사한지 계산 (Dot Product 사용)
                Vector3 worldGroup = localToWorld.MultiplyPoint3x4(group.Value[0]);
                Vector3 groupDirection = new Vector3(worldGroup.x - heightCenter.x, 0, worldGroup.z - heightCenter.z).normalized;
                float dotProduct = Vector3.Dot(direction, groupDirection);

                // dotProduct가 최대일 경우 해당 그룹에 속하게 됨
                if (dotProduct > maxDotProduct)
                {
                    maxDotProduct = dotProduct;
                    bestDirectionKey = group.Key;
                    foundGroup = true;
                }
            }
 
            // 비슷한 그룹이 없다면 새로운 그룹을 만들고, 있다면 기존 그룹에 추가
            if (foundGroup && maxDotProduct >= 0.999f)
            {
                bowKnotDirectionGroups[bestDirectionKey].Add(vertex);
            }
            else
            { 
                int newDirectionKey = Mathf.RoundToInt(direction.x * 1000) * 1000 + Mathf.RoundToInt(direction.z * 1000);
                bowKnotDirectionGroups[newDirectionKey] = new List<Vector3> { vertex };
            }
``` 

5. 리본 끈 압축 효과 (ApplyInterpolatedSqueezeEffectAsync)
정점들의 방향을 그룹화한 후, 압축 효과를 부드럽게 적용하여 자연스러운 리본 변형을 생성합니다.

```C#
            // 그룹 내 중심점
            Vector3 centroid = new Vector3(vertices.Average(v => v.x),
                                           vertices.Average(v => v.y),
                                           vertices.Average(v => v.z));


            // 그룹내 중심점과 상대적인 위치를 저장합니다.
            Dictionary<Vector3, Vector3> relativePositions = new Dictionary<Vector3, Vector3>();
            foreach (var vertex in vertices)
            {
                relativePositions[vertex] = vertex - centroid;
            }
            // 추가 계산을 통해 중심점을 이동 시킴(리드미에 없음)
            centroid += directionToMove.normalized * moveDistance;
            // 다른 정점들의 상대적 위치를 유지하며 이동시킵니다. 
            foreach (var vertex in vertices)
            {
                if (vertexIndexMap.TryGetValue(vertex, out List<int> indices))
                {
                    foreach (var index in indices)
                    {
                        if (vertex == centroid) bowKnotDeformedVertices[index] = centroid;
                        else bowKnotDeformedVertices[index] = centroid + relativePositions[vertex];
                    }
                }
            }
``` 

6. 리본 본체 위치 및 방향 보정 (MoveBowKnotBody)
리본 본체의 위치와 방향을 허벅지에 맞게 보정하여 최종적으로 압축된 리본 모양을 생성합니다.
 

### 주요 처리 방식
- 높이를 기준으로 정점를 그룹화하고, XZ 중심을 추출
- 다리 중심점으로부터의 거리 및 방향 벡터를 기반으로 메시를 압축
- 리본 끈 정점들의 방향을 그룹화하여 부드러운 압축 효과 적용

## 특징
- **정점 캐싱** 및 **에지 해시 기반 중간점 캐싱**으로 중복 계산 최소화
- `Matrix4x4`를 통한 월드 ↔ 로컬 좌표 간 변환
- `Coroutine` 기반 정점 배치 처리로 부드러운 실행

## 어려움과 해결책
### 메시의 정점이 적어 발생한 부자연스러움
다리 메시의 정점과 삼각형 수가 부족해 압축 시 부자연스러운 결과가 발생했습니다. 이를 해결하기 위해 메시를 분할하여 자연스러운 변형이 가능하도록 개선하였습니다.
### 리본 끈의 정점들을 어떻게 이동시킬 것인가?
허벅지가 완벽한 원형이 아니기 때문에 리본 끈들의 정점들을 중점 기준으로 일정하게 이동한다면 이상하게 보이기 때문에 리본 끈들의 정점들을 방향에 따라 그룹화하여 그룹의 중심점을 이동하고 그 중심점의 상대적 위치를 통해 정점들을 이동하도록 처리하여 살에 밀착하여 살을 압축하는듯한 효과를 주었습니다.
### 메시 찢어짐 문제
메시 분할 과정에서 메시를 나누어 분할한 부분과 안한 부분의 찢어짐 문제가 발생하여 경계부분을 체크하여 경계부분도 분할을 통해 찢어짐이 발생하지 않도록 수정.

## 느낀 점
  [![Video Label](http://img.youtube.com/vi/DvQzY2maJl8/0.jpg)](https://youtu.be/DvQzY2maJl8)
  위의 영상을 보면서 이 기능은 어떻게 구현했을까 고민하다가 만약 c# 코드로 구현한다면 어떻게 구현을 할 수 있을까?에 대한 의문점으로 시작한 코드입니다.

셰이더로 구현했다면 더 간단했을 수도 있지만, C# 스크립트로 직접 구현함으로써 메시 분할 및 구조에 대한 이해를 깊이 있게 다질 수 있었던 점이 큰 수확이었다고 생각합니다.