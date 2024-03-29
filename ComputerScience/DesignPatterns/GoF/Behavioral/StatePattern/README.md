# 상태 패턴(State Pattern)    

## 기능 설명  
 상태 패턴은 객체의 내부 상태가 변경될 때 해당 객체의 동작을 변경할 수 있게 해주는 행위 디자인 패턴입니다.     
이 패턴은 상태별 동작을 별도의 클래스로 캡슐화하고 객체가 이러한 클래스들 사이를 런타임에 전환할 수 있게 합니다.    

### 장점   
* 상태 캡슐화: 상태별 동작이 별도의 클래스로 캡슐화되어 모듈화와 유지보수성을 증진시킵니다.   
* 단순화된 객체: 객체의 동작이 명확하게 상태별로 구분되어 유지보수와 이해가 용이해집니다.   
* 유연성: 객체는 런타임에 동적으로 다양한 상태로 전환될 수 있어 객체 자체를 수정하지 않고 동적인 동작 변경이 가능합니다.      

### 단점   
* 클래스 수 증가: 상태를 별도의 클래스로 구현하는 것은 클래스 수의 증가로 이어질 수 있어 복잡성을 증가시킬 수 있습니다.
* 남용 가능성: 상태 패턴의 잘못된 사용, 예를 들어 부적절한 상태 전이나 과도한 상태 클래스 사용 등은 이해와 유지보수가 어려운 코드로 이어질 수 있습니다.

## 느낀 점
 상태 패턴을 공부하면서 객체가 여러 상태 변화를 겪는 경우 해당 동작을 관리하는 데 그 중요성을 깨달았습니다.      
상태별 동작을 별도의 클래스로 캡슐화함으로써 패턴은 보다 깔끔하고 유지보수가 쉬운 설계를 촉진합니다.     
또한 런타임에서 동적인 동작 변경이 가능해 시스템의 유연성을 향상시킵니다.     

특히 게임 개발에서는 객체가 현재 상태에 따라 다양한 작업을 수행하기 때문에 상태 패턴이 특히 유용합니다.       
예를 들어 게임의 캐릭터는 현재 상태에 따라 다른 동작(걷기, 뛰기, 공격 등)을 할 수 있습니다.   
상태 패턴을 사용하면 이러한 동작들을 별도의 상태 클래스로 캡슐화하여 캐릭터의 동작을 쉽게 관리하고 수정할 수 있습니다.    

종합적으로, 상태 패턴은 다양한 상태와 동작을 가진 복잡한 시스템을 유연하고 모듈화된 방식으로 관리할 수 있는 효과적인 해결책을 제공합니다.   
