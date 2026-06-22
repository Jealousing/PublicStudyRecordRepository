# Snow (적설)

이 환경 시스템의 핵심입니다. 지형 위에 눈이 시간에 따라 쌓이고, 발자국이 찍히며, 천장 아래에는 눈이 닿지 않습니다. 계단·이동하는 천장 같은 구조물에도 자연스럽게 대응합니다.

[상위 README](../../README.md)에서 요약한 **mesh → RT 전환**의 상세와, 그 위에서 동작하는 적설 기능 전체를 이 문서에 정리합니다.

<!-- 눈쌓임 과정 전체 gif -->

---

## 목차

- [mesh에서 RT로](#mesh에서-rt로) — 접근 방식 전환 (핵심)
- [적설 누적](#적설-누적) — 시간에 따라 쌓이는 눈
- [Sky Occlusion](#sky-occlusion) — 천장 아래엔 눈이 닿지 않게
- [동적 천장](#동적-천장) — 움직이는 천장 대응
- [발자국 RT 파이프라인](#발자국-rt-파이프라인) — stamp / fade / shift
- [계단 적설](#계단-적설) — 계단 측면·윗면 처리
- [표면 셰이딩](#표면-셰이딩) — 능선 음영 · SSS · 발자국 음영
- [테셀레이션과 T-junction](#테셀레이션과-t-junction) — mesh 방식의 과제
- [관련 코드](#관련-코드)

---

## mesh에서 RT로

> 전환의 **배경(CPU 병목)과 성능 수치**는 [상위 README](../../README.md#최적화를-위해-걸었던-길--mesh에서-rt로)에 있습니다. 여기서는 적설 관점에서 **무엇이 어떻게 바뀌었는지**를 봅니다.

### mesh 방식 — 정점을 CPU에서 깎다

처음에는 지형 메시를 런타임에 **세분화**하고, 각 정점을 눈 높이만큼 끌어올려 적설을 표현했습니다. 발자국은 해당 정점을 눌러 내리는 식이었습니다.

문제는 이 계산이 전부 **CPU**에서 일어난다는 점이었습니다. 적설 범위가 넓어질수록 세분화할 정점이 늘고 메인 스레드가 병목이 되었으며, 멀티코어로 분산해도 프레임 저하가 해소되지 않았습니다. (정량적 근거는 [성능 비교](../../Benchmark/README.md) 참고.) 여기에 세분화 경계의 **T-junction 균열**, 청크 경계의 **이음새** 같은 기하 문제도 직접 막아야 했습니다.

### RT 방식 — 데이터를 텍스처에, 변위를 GPU에

그래서 "정점을 직접 만지는" 대신, **적설 상태를 RenderTexture에 그리고 셰이더가 그 텍스처를 읽어 GPU에서 정점을 변위**시키도록 바꿨습니다.

```mermaid
flowchart LR
    INPUT["입력<br/>발자국 · 적설량 · occlusion"] --> RT["RenderTexture<br/>(SnowRenderTextureManager)"]
    RT --> SHADER["눈 표면 셰이더<br/>(SnowSurface + Tessellation)"]
    SHADER --> GPU["GPU에서 정점 변위 · 음영"]
```

- CPU는 더 이상 메시를 깎지 않고, RT를 갱신하는 가벼운 작업만 합니다.
- 정점 변위는 GPU **하드웨어 테셀레이션**이 담당하므로, mesh 방식에서 손으로 막던 T-junction 문제가 사라집니다.
- 발자국·occlusion도 모두 RT라는 하나의 데이터 통로로 합쳐집니다.

**RT에 담기는 것** — occlusion 아틀라스는 `RGFloat` 텍스처로, **R 채널에 적설 계수(snowFactor), G 채널에 천장 깊이(ceilDepth)** 를 담습니다. 발자국은 별도 RT에 누적됩니다. 표면 셰이더는 정점(도메인) 단계에서 이 값들을 읽어, 적설량만큼 올리고 발자국·천장 가림만큼 깎아 최종 변위량을 정합니다.

| | mesh | RT |
|---|---|---|
| 적설 데이터 | 메시 정점 | RenderTexture |
| 변위 주체 | CPU | GPU (하드웨어 테셀레이션) |
| T-junction | 직접 방지해야 함 | 하드웨어가 처리 |
| 발자국 | 정점 변형 | RT 누적 |

> mesh 구현도 코드에 남겨, 시연 영상에서 두 방식을 직접 비교합니다.

---

## 적설 누적

눈은 한 번에 최대치로 차지 않고, **시간에 따라 서서히 쌓입니다.** 현재 날씨 강도가 목표 적설량을 정하고, 실제 적설량이 그 목표를 향해 매 프레임 **일정 속도로 접근**합니다.

쌓이는 **속도**는 전략(`IAccumulationStrategy`)으로 분리했습니다. 기본 구현은 "게임 시간 기준 N시간 동안 최대치에 도달"하도록 속도를 계산하며, 전략을 교체하면 축적 곡선을 바꿀 수 있습니다.

```csharp
public float CalculateSpeed(float minValue, float maxValue)
{
    if (hoursToMax <= 0f) return 0f;
    float gameTimeInSeconds = fullDayDuration * (hoursToMax / 24f);
    return (maxValue - minValue) / gameTimeInSeconds; // 초당 증가 속도
}
```

이 구조 덕분에 적설 컨트롤러(mesh·RT 양쪽)는 "얼마나 빨리 쌓일지"를 직접 알 필요 없이, 공통 베이스(`SnowAccumulatorBase`)에서 동일하게 누적 로직을 공유합니다.

---

## Sky Occlusion

실내나 다리 밑처럼 **하늘이 가려진 곳**에는 눈이 쌓이면 안 됩니다. 각 지점이 "하늘에 얼마나 노출됐는가"를 구해 셰이더에 넘기고, 셰이더는 가려진 곳의 적설을 잘라냅니다.

<!-- occlusion on/off 비교 gif -->

**접근** — 위에서 레이캐스트해 천장에 가려진 셀(occluded)을 찾고, 그 경계로부터 **바깥으로 거리장(distance field)을 BFS로 전파**합니다. 천장 경계에서 멀어질수록 적설량이 0→1로 차오르게 해, 천장 밑에서 노출 영역으로 넘어갈 때 눈이 자연스럽게 경사지며 생깁니다.

```csharp
// 천장에 가려진 셀의 경계에서 바깥으로 거리장을 BFS 전파
// → 경계에서 멀수록 snowFactor가 0→1로 차오름 (천장 밑 경사 표현)
while (queue.Count > 0)
{
    int i = queue.Dequeue();
    int x = i % res, z = i / res;
    for (int d = 0; d < 8; d++) // 8방향(대각선 비용 √2)
    {
        int ni = (z + ddz[d]) * res + (x + ddx[d]);
        float nd = ceilDist[i] + dcost[d];
        if (nd < ceilDist[ni]) { ceilDist[ni] = nd; queue.Enqueue(ni); }
    }
}
// snowFactor: 차폐=0, 노출은 경계로부터 거리 / 경사폭 만큼 0→1 램프
float snowFactor = occluded[i] ? 0f : saturate(ceilDist[i] / slopePixels);
```

**5×5 타일 아틀라스** — 플레이어 주변을 `tileSize` 단위의 5×5 타일 그리드로 나누고, 각 타일의 occlusion 결과를 하나의 **아틀라스 텍스처**에 모아 셰이더가 한 번에 샘플링합니다. 플레이어가 이동하면 새로 들어온 타일만 스캔하고 나머지는 캐시를 재사용해, 매 프레임 전체를 다시 계산하지 않습니다.

타일 경계의 이음새는 각 타일을 경사폭만큼 **이웃까지 넓혀(halo) 스캔**해, 경계 셀도 이웃 타일의 천장을 보도록 함으로써 제거했습니다.

이 occlusion 결과는 **눈과 비가 공유**합니다(`SkyOcclusionCommon.hlsl`). 천장 밑에 눈이 안 쌓이는 것과 비에 안 젖는 것은 같은 데이터로 처리됩니다.

---

## 동적 천장

엘리베이터·문처럼 **움직이는 천장**은 위치가 바뀌면 그 아래 적설 상태도 바뀌어야 합니다. 천장이 비키면 가려졌던 자리에 눈이 다시 생기고, 천장이 덮이면 그 자리 눈이 사라져야 하죠.

이를 위해 움직이는 천장에 `DynamicCeiling`을 붙입니다. 위치/회전이 임계값 이상 바뀌면, 매니저가 **이전 자리와 새 자리의 타일을 모두 다시 스캔**해 occlusion을 갱신합니다. 정적 천장은 한 번만 스캔하므로, 이 비용은 실제로 움직이는 천장에만 듭니다.

occlusion이 바뀔 때 적설 변화는 **방향에 따라 다른 속도**로 페이드합니다. 천장이 비켜 눈이 **생길 때는 빠르게**(기본 1.5초), 천장이 덮여 눈이 **사라질 때는 느리게**(기본 5초) 전환해, 눈이 툭 사라지지 않고 서서히 메말라가는 느낌을 줍니다.

---

## 발자국 RT 파이프라인

발자국은 세 개의 작은 셰이더 패스로 처리하는 RT 파이프라인입니다.

```mermaid
flowchart LR
    STAMP["stamp<br/>밟은 자리 누적"] --> FADE["fade<br/>시간이 지나면 감쇠"]
    FADE --> SHIFT["shift<br/>플레이어 따라 RT 이동"]
    SHIFT --> STAMP
```

- **stamp** — 발이 닿은 위치에 원형 자국을 RT에 누적. 가장자리는 smoothstep으로 부드럽게.
- **fade** — 매 프레임 RT를 조금씩 감쇠시켜, 눈이 다시 쌓이며 발자국이 메워지는 효과.
- **shift** — 플레이어가 움직이면 RT 중심을 따라 이동시켜, 한정된 RT로 넓은 영역을 커버.

```hlsl
// stamp: 발자국 위치에서의 거리로 원형 자국을 만들어 기존 RT에 누적
float dist  = length(i.uv - _FootprintUV.xy);
float t     = saturate(1.0 - dist / _FootprintSize);
float stamp = t * t * (3.0 - 2.0 * t) * _FootprintIntensity; // smoothstep falloff
return half4(saturate(existing + stamp), 0, 0, 1);
```

셰이더는 이 발자국 RT를 적설량과 함께 읽어, 밟은 자리만큼 정점을 눌러 내립니다.

---

## 계단 적설

계단은 평면이 아니라 **단마다 높이가 다른 구조**라, 단순 높이 변위로는 측면이 비어 보입니다. 계단 메시에서 발판들의 경계와 진행 방향을 추출(`StairBoundsExtractor`)해 셰이더에 넘기고, 셰이더의 **Stair 모드**가 계단의 로컬 좌표 기준으로 측면·윗면에 눈을 채웁니다.

발판 경계는 메시 정점의 높이를 그룹으로 묶어 단 단위로 추출하며, 윗면(수평 발판)만 대상으로 삼도록 위를 향한 법선만 통과시킵니다.

---

## 표면 셰이딩

적설은 변위뿐 아니라 **눈처럼 보이는 음영**도 중요합니다. 눈 표면 셰이더(`SnowSurface.shader`)는 다음을 처리합니다.

- **능선 음영 보정** — 변위로 생긴 눈 표면의 실제 기울기를 노멀에 반영합니다. occlusion 높이장에서 매크로 노멀을 구해 기본 노멀과 블렌드하여, 쌓인 눈의 능선이 음영으로 드러나게 합니다.
- **SSS (서브서피스 스캐터링)** — 역광에서 빛이 눈 내부를 통과하는 산란을 흉내 내, 눈이 빛을 머금은 느낌을 줍니다.
- **발자국 음영** — 발자국이 파인 부분을 조명 밝기에 비례해 어둡게 해, 밝은 햇빛에서도 자국이 보이도록 합니다.

---

## 테셀레이션과 T-junction

> 이 절은 **mesh 방식**에서 풀어야 했던 문제입니다. RT 방식은 GPU 하드웨어 테셀레이션을 쓰므로 발생하지 않지만, 문제 해결 과정으로서 기록합니다.

카메라 거리에 따라 메시를 적응적으로 세분화하면, **세분화된 삼각형과 안 된 이웃 삼각형이 만나는 변**에서 정점이 어긋나 **틈(T-junction 균열)** 이 생깁니다.

해결은 **변(edge)의 중간점을 공유**하는 것입니다. 각 변을 정점 인덱스 쌍의 **고유 키로 캐싱**해서, 같은 변을 공유하는 두 삼각형이 **동일한 중간점 정점**을 쓰도록 했습니다. 그러면 한쪽만 세분화돼도 경계가 벌어지지 않습니다.

```csharp
// 같은 edge를 공유하는 인접 삼각형이 '같은 중간점 정점'을 쓰게 캐싱 → T-junction 방지
long edgeKey = MakeEdgeKey(v0, v1);          // 정점 인덱스 쌍의 순서 무관 고유 키
if (cache.TryGetValue(edgeKey, out int existingIdx))
    return existingIdx;                       // 이미 만든 중간점 재사용

int newIdx = verts.Count;
verts.Add((srcVerts[v0] + srcVerts[v1]) * 0.5f); // 변의 중간점 생성
cache[edgeKey] = newIdx;
```

---

## 관련 코드

| 역할 | 클래스 / 셰이더 |
|---|---|
| RT 생성 · occlusion · 발자국 관리 | `SnowRenderTextureManager` |
| URP 렌더 패스 등록 | `SnowRendererFeature` |
| 적설 누적 공통 베이스 · 전략 | `SnowAccumulatorBase`, `IAccumulationStrategy` |
| 움직이는 천장 등록 | `DynamicCeiling` |
| 계단 발판 추출 | `StairBoundsExtractor` |
| 눈 표면 변위 · 음영 | `SnowSurface.shader`, `SnowTessellation.hlsl` |
| occlusion 공통 계산(눈·비 공유) | `SkyOcclusionCommon.hlsl` |
| 발자국 RT 패스 | `SnowFootprintStamp / Fade / Shift.shader` |
| (레거시) mesh 적설 · 세분화 | `SnowPlaneController`, `SnowObjectController` |
