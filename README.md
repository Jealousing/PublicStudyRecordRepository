# Unity_PublicRepositories
유니티로 공부한 코드들 모아두는 공개형 저장소

## 저장소의 목적
 유니티를 공부하면서 구현한 기능들을 카데고리 별로 정리하기 위해서 만들어진 저장소입니다.
 
 
## 저장소 구조 소개
기능별로 폴더에 스크립트를 저장하고 리드미를 따로 두어 그 기능이 무엇인지 간단한 소개, 구현할때 어려웠던 점, 
해결했던 방법, 느낀점 등을 작성할 예정입니다.


## 기능별 간단소개
 <details>
 <summary><b><em>Design Pattern</em></b> </summary>
   
* **[오브젝트풀 패턴][ObjectPoolingBaselink]**  : 객체를 재사용하여 자주 발생하는 가비지 컬렉션 호출을 줄여서 메모리 사용을 효율적으로 개선하는 패턴.    
* **[몬스터 AI FSM][FSMlink]**  : 객체의 동작을 다양한 상태로 나누고, 이 상태들 간의 전환과 각 상태에서의 행동을 관리하는 패턴.    
* **[싱글톤 패턴][Singletonlink]**  : 특정 클래스가 단 하나의 인스턴스만 가지도록 하는 디자인 패턴이며 전역 접근이 가능하다, 제네릭으로 구현되어있는 스크립트.

 </details>

 <details>
 <summary><b><em>Other Functions</em></b> </summary>

* **[체력바][HPBarlink]**  : 체력을 가진 오브젝트 머리위에 표시되는 막대로 HP상태를 알려주는 기능.
* **[로프액션][GrapplingHookslink]**  : 마우스 에임 방향으로 로프를 발사해 그 곳으로 직선이동이나 스윙이동하는 기능.
* **[Json 데이터 관리][JsonDataManagerlink]**  : Json으로 데이터를 관리 및 저장하는 시스템
* **[메쉬로 도형그리기][DrawShapeMeshlink]**  : Unity Graphics 시스템을 이용해서 도형모양의 메쉬를 생성해 보여주는 기능
* **[범위공격 시스템][RangeHitSystemlink]**  : 메쉬로 도형을 그리면서 그 구역내에 있는 오브젝트에 대미지를 주는 방식.
* **[캐릭터 이동관련 스크립트][Movementlink]**  :  캐릭터가 이동하는 방식에 대한 스크립트.
* **[파쿠르 시스템][Parkourlink]**  :  캐릭터가 특정 오브젝트와 상호작용해서 구조물을 활용해 이동하는 시스템.
* **[IK][IKlink]**  : 손과 발의 IK(역운동학) 사용해보는 스크립트.

 </details>

## 팀프로젝트
* **[TeamDemonStrate][TeamProjectDemonStratelink]**  : 21.04.13 ~ 21.10.12 학교생활 중 진행한 중단된 팀 프로젝트에서 만들었던 코드
 

[ObjectPoolingBaselink]: /ObjectPoolingBase
[FSMlink]: /MonsterAI/FSM
[Singletonlink]: /Singleton

[HPBarlink]: /HPBar
[GrapplingHookslink]: /GrapplingHooksAndRopeSwing
[JsonDataManagerlink]: /JsonDataManager
[DrawShapeMeshlink]: /DrawShapeMesh
[RangeHitSystemlink]: /RangeHitSystem
[Movementlink]: /Movement&Parkour/Movement
[Parkourlink]: /Movement&Parkour/Parkour
[IKlink]: /IK(InverseKinematics)

[TeamProjectDemonStratelink]: /TeamProject_DemonStrate
