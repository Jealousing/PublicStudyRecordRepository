# 책임 연쇄 패턴 (Chain of Responsibility Pattern)

## 기능 설명
 책임 연쇄 패턴은 요청을 처리하는 객체들을 연결하여 요청을 보내는 객체와 처리하는 객체를 분리하는 디자인 패턴입니다.    
요청을 처리할 수 있는 객체가 여러 개일 때, 이를 연쇄적으로 연결하여 처리할 객체를 동적으로 결정할 수 있습니다.   

이 패턴은 클라이언트가 여러 객체에게 요청을 보내고, 이들 중 하나가 요청을 처리할 수 있을 때까지 요청을 연쇄적으로 전달하는 구조를 가집니다.    
이를 통해 요청을 처리하는 객체와 클라이언트 사이의 의존성을 줄일 수 있습니다.   

 ### 장점
* 요청 처리의 유연성을 제공합니다. 객체를 동적으로 추가하거나 제거하여 처리 순서를 변경할 수 있습니다.   
* 클라이언트와 요청 처리 객체 사이의 결합도를 줄입니다. 클라이언트는 요청을 보내는 객체만 알고 있으면 되며, 실제 처리 객체를 알 필요가 없습니다.    
* 요청 처리 객체를 쉽게 재사용할 수 있습니다.   

 ### 단점
* 요청이 처리될 수 없는 경우, 전체 연쇄가 끝까지 진행되어야 합니다. 이로 인해 불필요한 처리가 발생할 수 있습니다.
* 연쇄에 속한 객체들 간의 관계를 잘못 구성하면 무한 루프나 처리 오류가 발생할 수 있습니다. 

## 느낀 점
 책임 연쇄 패턴을 통해 요청 처리의 유연성을 얻을 수 있다는 것을 느꼈습니다. 특히, 여러 객체에게 요청을 보내고 그 중 하나가 요청을 처리할 때까지 연쇄적으로 전달할 수 있는 구조는 시스템의 유연성과 확장성을 높여줍니다.   
또한, 클라이언트와 요청 처리 객체 사이의 결합도를 낮춰 유지보수를 용이하게 만든다는 것을 알게 되었습니다.
