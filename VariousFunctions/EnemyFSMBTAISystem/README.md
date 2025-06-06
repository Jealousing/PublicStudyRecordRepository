---
# **FSM & Behavior Tree 통합 몬스터 AI 시스템**

Unity에서 몬스터 행동을 다양하게 관리하기 위한 몬스터AI 시스템을 만들어 봤습니다.
이 AI 시스템은 유한 상태 기계(Finite State Machine, FSM)와 행동 트리(Behavior Tree, BT)의 통합을 통해 다양한 NPC(Non-Player Character) 행동 관리 방식을 제공합니다. 각 몬스터는 맞춤형 상태와 개별적인 행동 트리를 가질 수 있어, 동일한 상태에서도 고유한 행동을 수행할 수 있습니다.

 
## 1. 목적
이 AI 시스템의 주요 목적은 Unity에서 몬스터의 행동을 유연하고 구조적으로 관리하는 것입니다.
FSM과 BT를 결합함으로써 복잡한 다중 상태 행동을 명확하게 관리할 수 있습니다. 이 설계는 각 몬스터가 정의된 조건과 상태에 따라 고유한 움직임을 할 수 있도록 하여 다양한 게임 플레이 경험을 가능하게 합니다.

## 2. 주요기능
* 상태 기반 제어: Idle, Chase, Attack 등과 같은 주요 상태 관리를 위한 FSM 활용
* 행동 트리 유연성: 각 상태에 대한 맞춤형 BT를 할당하여 해당 상태에서 고유한 몬스터 행동 수행
* 동적 상태 전환: 게임 내 트리거 또는 조건에 따라 효율적인 상태 전환 가능
* 모듈화 및 확장성: 새로운 상태나 행동 노드를 추가하거나 각 몬스터에 고유한 BT를 할당하여 시스템을 쉽게 확장 가능

## 3. 시스템 구조 설명 
 * FSM(Finite State Machine) : 이 시스템에서 FSM은 Idle, Search, Chase, Attack 등 주요 상태를 관리하며 각 상태에 BT를 매핑시켜 사용합니다.
 * BT(Behavior Tree) : 각 상태 내에서 수행할 세부 행동을 정의합니다. 각 몬스터는 주어진 상태에서 고유한 행동 트리를 통해 다양한 행동을 수행 할 수 있습니다.

## 4. 장단점
### 장점
* 높은 유연성 : FSM과 BT 구조를 결합하여 Enemy에 따라 다양한 다른 상태와 상태에 따른 BT를 이용한 행동을 설정할 수 있습니다.
* 재사용성 : 모듈형 설계를 통해 FSM 및 BT 구성 요소를 여러 몬스터에 재사용할 수 있습니다.
* 명확한 구조: 상태 관리(FSM)와 행동 논리(BT)의 분리를 통해 코드 가독성과 사용성이 향상됩니다.

### 단점
* 리소스 오버헤드: 복잡한 BT 및 FSM 상태에 따른 더 많은 리소스를 사용할 수도  있습니다.
* 복잡성 증가: FSM과 BT를 통합해서 잘 관리하지 않으면 복잡한 종속성이 발생할 수 있습니다.

---
