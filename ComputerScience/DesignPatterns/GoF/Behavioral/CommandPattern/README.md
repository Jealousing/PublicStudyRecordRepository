# 커맨드 패턴 (Command Pattern)

## 기능 설명  
Command 패턴은 작업을 객체로 캡슐화하여 매개변수화된 방식으로 클라이언트에게 작업을 요청하거나 대기시키는 행위를 추상화하는 행위 패턴입니다.    
이 패턴은 요청이나 연산을 객체로 만들어서 일급 객체(First Class Object)로 다루어주는데, 이를 통해 요청을 큐에 넣거나 로깅, 취소 등의 기능을 제공할 수 있습니다.    

> 일급객체(First-class Object)란 다른 객체들에 일반적으로 적용 가능한 연산을 모두 지원하는 객체를 가리킨다.   

### 장점   
* 요청 발신자와 요청 수신자를 분리하여 유연성을 증가시킵니다.    
* 새로운 명령을 쉽게 추가하거나 기존 명령을 수정할 수 있습니다.    
* 작업 로깅, 취소, 재실행과 같은 기능을 구현할 수 있습니다.    
* 코드의 읽기와 유지보수가 쉬워집니다.    

### 단점   
* 많은 클래스를 생성하게 되어 코드의 복잡성이 증가할 수 있습니다.

## 느낀 점
Command 패턴을 공부하면서, 객체 지향 디자인 원칙을 적용하는 방법과 코드의 재사용성을 높이는 방법에 대해 배울 수 있었습니다.     
특히, 각 작업을 캡슐화하여 유연한 구조를 만들고, 새로운 기능을 쉽게 추가하고 수정할 수 있는 장점을 깨달았습니다.     
또한, 코드의 가독성과 유지보수성을 높이는 중요성을 더욱 느낄 수 있었습니다.    

이 예시에서는 Command 패턴을 게임 개발에 적용한 것입니다.   
각 Command 클래스는 특정 작업을 캡슐화하고, CommandInvoker 클래스를 통해 실행됩니다.    
이를 통해 게임 내에서 다양한 작업을 명확하게 분리하고, 유지보수 및 확장이 쉽도록 만들 수 있습니다.    
