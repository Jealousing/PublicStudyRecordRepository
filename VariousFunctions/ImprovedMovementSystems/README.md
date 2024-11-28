# 기존의 이동관련 시스템들을 개선

## 개선된 시스템들
 
 작성한 것보다 더 많은 변경점이 있을 수도 있습니다.
 
 ### **[Movement][Movementlink]**
 
 |          기 능          |                 변경 전                |                 변경 후                |
 |---------------|------------------------|------------------------|
 | 바닥 각도에 따른 이동 | 제한 없음 | 진행 방향의 바닥의 각도를 체크해서 이동 제한 |
 | 미끄러짐 | 리지드바디를 통한 중력의 영향 | 이동가능한 각도보다 큰 경우 해당방향으로 미끄러짐 |
 

 ### **[Parkour][Parkourlink]**

 |          기 능          |                 변경 전                |                 변경 후                |
 |---------------|------------------------|------------------------|
 | 감지 방법 변경 | 콜라이더를 이용한 감지 | 레이케스트를 이용한 감지방법 |
 | 파쿠르 동작추가 | Vault, Climbing, Wall Run 3가지 | 좀 더 다양한 파쿠르 동작과 벽타기 스크립트를 하나로 통합 |
 | 회전 관련 | 콜라이더와 레이케스트 1개를 이용해 각진 곳을 이동을 못했던 현상 | 레이케스트만 이용하여 진행방향 각도 체크로 자연스러운 이동 |

 ### **[Grappling Hook][GrapplingHookslink]**

 |          기 능          |                 변경 전                |                 변경 후                |
 |---------------|------------------------|------------------------|
 | 무기(도구)기능으로 변환 | 스킬 등록없이 단축키로 사용함 | 무기 카데고리별 스킬을 통한 스킬사용 |
 | 이동 시작 시점 | 로프를 발사하고 이동시작 단축키를 눌러야 이동 | 이동 시작 토글스킬을 누르면 종료전까지 로프가 도착하면 그 방향으로 이동 |
 | 구형 회전 이동 | 유니티 Vector3.Slerp을 이용한 계산 | Quaternion.AngleAxis을 이용한 계산 |
 | 직선 이동 도착 위치 조절 | 기존에는 직선으로 이동하면서 로프 도착위치나 도착 오브젝트의 위가 가깝다면 그쪽으로 이동하던 2가지방식 | 기존 방식에 키입력을 통한 도착위치 보정추가 |
 | 이동 타입 변경 | 진행중 불가 | 진행 도중 키입력을 통한 전환 가능 |

  
## 유튜브
 [![Video Label](http://img.youtube.com/vi/txSauiDIWvA/0.jpg)](https://youtu.be/txSauiDIWvA)
 
[GrapplingHookslink]: /VariousFunctions/GrapplingHooksAndRopeSwing 
[Movementlink]: /VariousFunctions/Movement&Parkour/Movement
[Parkourlink]: /VariousFunctions/Movement&Parkour/Parkour