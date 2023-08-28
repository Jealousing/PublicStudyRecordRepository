# 메쉬로 도형그리기

## 기능 설명
 이 스크립트들은 Unity에서 Graphics 시스템을 이용해서 다양한 모양의 Mesh를 생성합니다.
 
 ### Scripts  
  1. ShapeMesh : 다양한 형태의 메쉬를 생성하기 위한 추상 클래스.
  2. RectangleShapeMesh : ShapeMesh의 파생 클래스이며, 직사각형 모양의 메쉬를 그리는 클래스.
  3. PolygonShapeMesh : ShapeMesh의 파생 클래스이며, 다각형 모양의 메쉬를 그리는 클래스
  4. DonutShapeMesh : ShapeMesh의 파생 클래스이며, 도넛 모양의 메쉬를 그리는 클래스. 
  5. DrawShapeManager : 다양한 형태의 메쉬를 관리하고 그리는 스크립트.
 
## 어려움과 해결책
 직사각형을 처음만들때는 문제가 없었는데 도넛형을 만들면서 모양이 제대로 안나와서 어떤 문제인지 찾다가 정점의 갯수를 늘려주니 해결되었다. 정점의 갯수가 적으면 같은 원이여도 각진게 보이고 많아질수록 각이 안진 원에 가까워진다.
 
## 느낀 점
 처음 이 스크립트를 만들게 시작한건 게임에서 유저의 스킬이나 몬스터의 패턴 범위 표시를 어떻게 구현할까에서 시작된 구현이다. 물론 실제 개발에서 이러한 방식으로 개발할지는 모르지만 실제 개발에서는 더 좋은 방법이 있지 않을까 한다. 이 스크립트를 만들당시 학교에서 DX배우면서 정점 버퍼를 사용해서 그렸던 방식과 유사해서 이부분에서는 어렵진 않았다.
 
## 추가로 개선하고 싶은 부분
 더 좋은 개선 방향성이 떠오른다면 적는걸로 하겠습니다.
 
## 유튜브
 [![Video Label](http://img.youtube.com/vi/GdsiRnyc37poTetA/0.jpg)](https://youtu.be/GdsiRnyc37poTetA)'

## 참고사이트
 https://gamedev.stackexchange.com/questions/31170/drawing-a-dynamic-indicator-for-a-field-of-view
 
