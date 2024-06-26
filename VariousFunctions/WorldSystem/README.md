# 월드 생성 및 관리 시스템

## 기능 설명
 게임 내 월드를 생성하고 관리하는 기능을 제공한다. 청크 단위로 스네일 회전 패턴으로 체크하며 생성 및 관리를 하고있다.

 ### 장점
 - 동적 월드 생성: 플레이어의 위치 주변에 동적으로 월드를 생성하여 성능을 최적화
 - 객체 풀링: 오브젝트 풀링을 활용하여 청크를 생성하고 재활용하여 성능을 향상
 - 랜덤한 월드 생성: 시드 값을 기반으로 월드를 생성하며, 섬과 바다 등 다양한 지형을 구현

 ### 단점
 - 청크 간의 경계 부분에서 이음새가 발생할 수 있기 때문에 스무딩 기법을 추가 구현해야할 필요가 있다.
 
## 느낀 점
 마인크래프트나 레프트를 생각하면서 만들어본 시스템이며 효율적인 월드 생성과 관리를 어떻게 만들어볼까 고민하면서 만들게된 시스템이다.
하지만 청크경계나 청크 로딩으로 인한 딜레이가 발생할 수 있어 개선이 필요할 수 있다 생각한다.
 
## 추가로 개선하고 싶은 부분
 - 로딩 최적화 : 비동기 로딩 및 멀티스레딩을 통한 로딩 딜레이 최소화 및 최적화 작업
 
## 유튜브
 [![Video Label](http://img.youtube.com/vi/bbcdDrUW3Mg/0.jpg)](https://youtu.be/bbcdDrUW3Mg)
 
