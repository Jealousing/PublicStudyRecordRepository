# BackendStudy — Unity 클라 · C# 서버 · MySQL 풀스택 학습

Unity(클라) ↔ C# 콘솔 서버(.NET 8) ↔ MySQL(DB) 구조로 간단한 멀티플레이 게임을 만들며,
**클라이언트–서버–DB 전체 흐름**을 직접 구현하며 이해하는 학습 프로젝트입니다.
네트워크 솔루션(Mirror·Photon 등)을 쓰지 않고 TCP 소켓부터 바닥에서 쌓아, "그 안에서 무슨 일이 일어나는지"를 익히는 것이 목적입니다.

---

## 기술 스택

| 영역 | 사용 기술 |
|---|---|
| 클라이언트 | Unity (C#) |
| 서버 | C# 콘솔 앱 (.NET 8, CMD 실행) |
| DB | MySQL (MySqlConnector) |
| 통신 | TCP 소켓 |
| 직렬화 | JSON (Newtonsoft.Json) |
| 멀티 테스트 | ParrelSync |

---

## 학습 로드맵

| 단계 | 주제 | 핵심 산출물 | 상태 | 문서 |
|---|---|---|---|---|
| 1 | TCP 소켓 통신 기초 | 패킷 프레이밍 [4B 길이][JSON], 에코 | ✅ 완료 | [Step1](docs/Step1_TCP.md) |
| 2 | MySQL DB 연결 | 스키마 설계 + 저장/조회 | ✅ 완료 | [Step2](docs/Step2_MySQL.md) |
| 3 | 로그인 / 회원가입 | 비밀번호 해싱(bcrypt), 세션 | ✅ 완료 | [Step3](docs/Step3_Auth.md) |
| 4 | 게임 데이터 저장 | 위치(3D)·재접속 복원 | ✅ 완료 | [Step4](docs/Step4_GameData.md) |
| 5 | 멀티플레이 | 다중 접속, 위치 동기화·보간, 채팅, 치팅 방지 | ✅ 완료 | [Step5](docs/Step5_Multiplayer.md) |
| + | 확장 (설계) | 자동 재연결, 전송 암호화, 서버 로그, 매치메이킹 | 📄 설계 문서 | [Extensions](docs/Extensions.md) |

**최종 목표 기능:** 회원가입/로그인 → 캐릭터 이름 DB 저장 → 맵 이동 → 타 플레이어 실시간 표시 → 채팅 → 재접속 시 위치 복원

---

## 프로젝트 구조

```
BackendStudy/
├── README.md              ← (이 문서) 로드맵 + 단계별 링크
└── docs/
    ├── Step1_TCP.md       ← 1단계: TCP 소켓·프레이밍
    ├── Step2_MySQL.md      ← 2단계: MySQL 연결
    ├── Step3_Auth.md       ← 3단계: 회원가입·로그인·세션
    ├── Step4_GameData.md   ← 4단계: 위치 저장·재접속 복원
    └── Step5_Multiplayer.md ← 5단계: 멀티플레이(동기화·보간·채팅·치팅방지)
    └── ...

(코드 위치 — 레포 내 기존 Unity 프로젝트 기준)
Assets/Code/Scripts/Net/   ← 클라이언트 네트워크 코드
DedicatedServer/           ← C# 콘솔 서버 (Unity Assets 바깥)
```

> 서버는 Unity의 `Assets/` 바깥에 둔다. `Assets/` 안의 `.cs`는 Unity가 자동으로 컴파일하므로, .NET 콘솔 서버 코드가 들어가면 충돌하기 때문이다.

---

## 진행 방식

각 단계는 **동작 → 왜 → 고장내보기 → 회고**를 반복하며 진행한다.
단계마다 ① 동작 확인 ② 이해 점검 질문 ③ 트러블슈팅 러닝 로그 ④ 아키텍처·패킷 다이어그램을 문서로 남긴다.

실행은 항상 **서버(CMD) 먼저 → Unity 나중** 순서다.
```bash
# 서버 실행 (예시)
cd DedicatedServer
dotnet run
```

---

## 각 단계 문서

- **[1단계 — TCP 소켓 통신 기초](docs/Step1_TCP.md)** ✅
  TCP가 바이트 스트림이라는 것, 길이 프리픽스 프레이밍, 에코 통신, 입력 검증의 첫 사례.
- **[2단계 — MySQL DB 연결](docs/Step2_MySQL.md)** ✅
  스키마 설계, C# ↔ MySQL 연결, 파라미터 바인딩(SQL 인젝션 방지), 접속정보 분리, 예외 처리.
- **[3단계 — 로그인 / 회원가입](docs/Step3_Auth.md)** ✅
  비밀번호 해싱(bcrypt·솔트), 해시 검증 로그인, 세션 기반 신원 관리, 계정 열거 방지.
- **[4단계 — 게임 데이터 저장 / 재접속 복원](docs/Step4_GameData.md)** ✅
  characters 테이블(외래 키), 3D 위치·회전 저장(UPSERT), 세션 기반 소유권, 재접속 복원, 값 검증.
- **[5단계 — 멀티플레이](docs/Step5_Multiplayer.md)** ✅
  다중 접속·공유 상태(ConcurrentDictionary), 위치 동기화(틱레이트), 보간, 채팅, 이동 속도 검증·서버 교정(치팅 방지).
