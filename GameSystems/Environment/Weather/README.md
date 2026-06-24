# Weather (날씨)

날씨를 확률로 고르고, 눈·비·맑음 사이를 강도 페이드로 전환합니다. 선택된 날씨에 맞춰 눈/비 이펙트와 구름이 바뀝니다.

이 폴더 아래에 눈([Snow](./Snow/README.md))과 비([Rain](./Rain/README.md)) 모듈이 있습니다.

<!-- 날씨 전환 gif -->

---

## 목차

- [날씨 선택](#날씨-선택) — weight 기반 확률
- [상태 전환과 강도 페이드](#상태-전환과-강도-페이드) — 끊김 없는 전환 (핵심)
- [이펙트 풀링](#이펙트-풀링) — 청크 단위 눈/비 이펙트
- [구름](#구름) — Volumetric Clouds
- [관련 코드](#관련-코드)

---

## 날씨 선택

다음 날씨는 후보들의 **weight(가중치) 기반 확률**로 고릅니다(`WeatherSelector`). 각 날씨는 **타입**(눈/비/맑음 등)과, 여러 타입을 묶는 상위 **카테고리**(`WeatherData`)를 가집니다. 카테고리는 전환 방식에 쓰입니다(아래).

---

## 상태 전환과 강도 페이드

날씨는 **FadeIn → Active → FadeOut** 의 강도 곡선을 따라 전환됩니다. 핵심은 **카테고리**에 따라 전환을 다르게 처리하는 점입니다.

- **같은 카테고리**로 바뀌면(예: 약한 눈 → 강한 눈) 강도를 **유지한 채 프로필만 교체**해 끊김이 없습니다.
- **다른 카테고리**로 바뀌면(예: 눈 → 비) 강도를 **0까지 페이드**해 이전 날씨가 자연스럽게 사라진 뒤 새 날씨가 올라옵니다.

```csharp
// 같은 카테고리면 강도 유지(프로필만 교체), 다른 카테고리면 0까지 페이드
currentIntensity = keepIntensity
    ? currentProfile.maxIntensity
    : Mathf.Lerp(currentProfile.maxIntensity, 0f, intensityT);
```

---

## 이펙트 풀링

눈/비 이펙트는 플레이어 주변 **청크 단위로 풀링**해 배치합니다. 카메라를 추적해 플레이어가 이동하면 멀어진 청크의 이펙트를 회수하고 가까워진 청크에 재배치하므로, 넓은 영역을 한정된 이펙트로 커버합니다. (`WeatherStateMachine`의 Effect Pool · Chunk · Camera Tracking)

---

## 구름

`CloudController`가 **Volumetric Clouds** 프리셋을 날씨 상태에 맞춰 적용합니다. 맑음과 눈/비일 때 구름의 밀도·모양이 달라집니다.

---

## 관련 코드

| 역할 | 클래스 |
|---|---|
| 날씨 상태 전환 · 강도 페이드 · 풀링 | `WeatherStateMachine` |
| weight 기반 날씨 선택 | `WeatherSelector` |
| 날씨 타입 · 카테고리 정의 | `WeatherData` |
| Volumetric Clouds 프리셋 | `CloudController` |
| 눈 모듈 | [Snow](./Snow/README.md) |
| 비 모듈 | [Rain](./Rain/README.md) |
