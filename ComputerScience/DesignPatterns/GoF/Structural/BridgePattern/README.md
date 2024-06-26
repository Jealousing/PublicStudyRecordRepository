# 브릿지 패턴 (Bridge Pattern)

## 기능 설명  
 브릿지 패턴은 추상화와 구현을 분리하여 두 개의 독립적인 계층을 형성하는 디자인 패턴입니다.   
이를 통해 추상화된 부분과 구체적인 구현 사이의 결합을 약화시켜 유연하고 확장 가능한 코드를 작성할 수 있습니다.    

### 장점   
* 추상화와 구현을 분리함으로써 각각의 변화에 대해 독립적으로 확장할 수 있습니다.   
* 클라이언트와 서버 사이의 연결을 유연하게 관리할 수 있습니다.    

### 단점   
* 추가적인 추상화 계층이 필요할 수 있어 복잡성이 증가할 수 있습니다.   
* 추상화와 구현의 동적 결합은 런타임에 성능 손실을 가져올 수 있습니다.   
 
## 느낀 점
브릿지 패턴은 코드의 유연성과 확장성을 높이기 위한 패턴이라는 것을 느꼈습니다.    
추상화와 구현을 분리함으로써 변화에 대응하기 쉬운 코드를 작성할 수 있으며,    
다양한 시나리오에서 유용하게 활용될 수 있음을 알게 되었습니다.    
브릿지 패턴은 Unity에서 다양한 상황에서 유용하게 활용될 수 있습니다.  
특히 게임 개발에서는 다양한 플랫폼 간 호환성을 유지하고, 다양한 입력 디바이스를 처리하며, 시스템 간의 통신을 관리하는 등 다양한 상황에서 브릿지 패턴이 유용하게 사용될 것으로 생각됩니다.

플랫폼 호환성 관리: 다양한 플랫폼 (PC, 모바일) 간의 입력 처리를 분리하여 관리할 수 있습니다.
다양한 입력 디바이스 지원: 키보드, 마우스, 터치 스크린 등 다양한 입력 디바이스를 처리하는 데 유연하게 대응할 수 있습니다.
다양한 데이터 형식 처리: 다른 형식의 데이터를 처리하는데 필요한 추상화와 구현을 분리하여 코드를 유연하게 유지할 수 있습니다.
시스템 간 통신 관리: 서로 다른 시스템 간의 통신을 추상화하여, 각 시스템이 독립적으로 확장 가능하도록 합니다.