# 이터레이터 패턴 (Iterator Pattern)

## 기능 설명  
 반복자 패턴은 객체의 집합체를 순차적으로 접근하는 방법을 제공하는 디자인 패턴입니다.  
이 패턴은 내부 구조를 노출시키지 않고 집합체의 요소를 반복하는 방법을 정의합니다. 이를 통해 클라이언트는 내부 구현 세부사항을 알 필요 없이 요소에 접근할 수 있습니다.

Iterator 패턴은 다음과 같은 주요 구성 요소를 포함합니다.

* 집합체(Iterable): 요소들의 집합을 나타내는 객체입니다.
* 반복자(Iterator): 집합체를 순회하고 각 요소에 접근하는 역할을 담당하는 객체입니다.
* 요소(Element): 집합체 내부에 있는 개별적인 객체 또는 데이터입니다.

### 장점   
* 일관성: 여러 종류의 데이터 구조를 순회하는 표준화된 방법을 제공하여 일관성을 유지합니다.     
* 은닉성: 내부 구현 세부사항을 숨겨서 클라이언트와 집합체 간의 의존성을 줄입니다.     
* 유연성: 객체는 집합체의 내부 구현에 독립적으로 구현되어 유연성을 제공합니다.     

### 단점   
* 추가 복잡성: 반복자 패턴을 구현하는 것은 일반적으로 추가 복잡성을 야기할 수 있습니다.     
* 캡슐화 위험: 잘못된 사용이나 구현 시 내부 구조가 노출될 수 있어 캡슐화 위반의 위험이 있습니다.     

## 느낀 점
 Iterator 패턴을 공부하면 데이터 구조를 효율적으로 순회하고 요소에 접근하는 방법을 이해할 수 있습니다.    
또한 패턴을 통해 코드의 일관성과 유연성을 높이며, 객체 간의 결합도를 낮출 수 있다는 것을 깨닫게 되었습니다.