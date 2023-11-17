# 날개 비행 시스템

## 기능 설명
 기존에 사용하던 [플레이어 상태에 따른 이동 방식 ][Movementlink]에 추가한 이동 방식이다.
원신에서 사용하는 날개 비행을 비슷하게 구현해 볼까 해서 시작한 기능 구현이고 구현 방식은
상태 진입 시 -> 중력 비활성화, 상태 퇴장 시-> 중력 활성화
입력이 있으면 캐릭터의 현재 방향으로 이동, 입력에 따른 회전, 진입 후에 일정 속도로 하강을 통해 구현했는데 비슷하게는 나온 것 같다.
 
## 추가로 개선하고 싶은 부분
  상태 퇴장 시에 날개를 접으면서 끝나도록 변경하는 것이 자연스러울 것 같다.
 
## 유튜브
 [![Video Label](http://img.youtube.com/vi/Fk4OVNQ56G4/0.jpg)](https://youtu.be/Fk4OVNQ56G4)

[Movementlink]: /VariousFunctions/Movement&Parkour/Movement
