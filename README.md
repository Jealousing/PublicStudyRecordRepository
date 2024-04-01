# PublicStudyRecordRepository
 공부한 것들을 모아두는 공개형 저장소

## 저장소의 목적
 다양한 공부한것을 저장하기위해서 만들어진 저장소입니다. 
 
## 저장소 구조 소개
  카테고리에 맞는 폴더 안에 그에 대한 공부 한 기록들이 저장되어 있습니다.
 * BuildGame 폴더 : 빌드된 게임을 올려두는 폴더입니다.
 * ComputerScience 폴더 : CS 공부하며 저장하는 곳입니다. ( 코테자료 및 디자인패턴 등 )
 * Various Functions 폴더 : 유니티로 다양한 기능을 제작해서 저장하는 곳입니다.
 * TeamProject 폴더 : 팀 프로젝트에 참가해서 사용한 코드들 저장하는 곳입니다.
 * TemporaryFolder 폴더 : 이 저장소의 양식을 만들어 사용하기 위한 폴더입니다.

 각 내부 폴더에 스크립트를 저장하고 리드미를 따로 두어 그 기능이 무엇인지 간단한 소개, 구현할때 어려웠던 점, 해결했던 방법, 느낀점 등을 작성할 예정입니다.


## 폴더별 간단소개

 <details>
 <summary><b><em>빌드된 게임</em></b> </summary>
	 
 *****

Unity:    
 * **[3D 수박게임][3dsuikagamelink]**  : 수박게임을 3d로 바꿔서 제작     
 
 *****
 
 </details>   

 <details>
 <summary><b><em> CS(ComputerScience) </em></b> </summary>
	 
 *****

* <details> <summary><b><em>디자인패턴</em></b> </summary> 
	
	* <details> <summary><b><em> GoF </em></b> </summary>
		
 		* <details> <summary><b><em>생성 패턴(Creational)</em></b> </summary>
			
			* **[싱글톤 패턴][Singletonlink]**  : 특정 클래스가 단 하나의 인스턴스만 가지도록 하는 디자인 패턴이며 전역 접근이 가능하다, 제네릭으로 구현되어있는 스크립트.    
			* **[팩토리 패턴][Factorylink]**  : 객체 생성을 공장(Factory) 클래스로 캡슐화 처리하여 대신 생성하게 하는 생성 디자인 패턴이다.    
			* **[추상 팩토리 패턴][AbstractFactorylink]**  : 객체 군을 생성하는 인터페이스 제공과 서로 다른 종류의 팩토리를 사용하여 객체를 생성하는 디자인패턴.      
			* **[빌더 패턴][Builderlink]**  : 빌더 패턴은 복잡한 객체의 생성 및 구성을 단순화하고, 객체의 생성 과정을 분리하여 유연성을 제공하는 디자인 패턴이다.    
			* **[프로토타입 패턴][PrototypePatternlink]**  : 기존 객체를 복사하여 새 객체를 생성하는 디자인 패턴으로, 새로운 객체를 처음부터 만드는 것을 피합니다.
     	  	</details>
		* <details> <summary><b><em>구조 패턴(Structural)</em></b> </summary>
			
			* **[어댑터 패턴][AdapterPatternlink]**  : 호환되지 않는 인터페이스를 변환하여 함께 작동하도록 만드는 디자인패턴.    
			* **[브릿지 패턴][BridgePatternlink]**  : 추상화와 구현을 분리하여, 두 요소 간의 결합을 약화시키고, 확장성이 높고 유용한 디자인 패턴.    
			* **[컴포지트 패턴][CompositePatternlink]**  : 부분-전체 계층을 나타내기 위해 객체를 트리 구조로 구성하고 개별 객체와 구성을 통일적으로 다룰 수 있는 디자인 패턴입니다.      
			* **[데코레이터 패턴][DecoratorPatternlink]**  : 데코레이터 패턴은 객체에 동적으로 새로운 기능을 추가할 수 있는 구조적 디자인 패턴입니다. (객체의 기능을 확장하거나 변경하기 위해 객체의 감싸는 래퍼 클래스를 생성하는 방식으로 작동)     
			* **[퍼사드 패턴][FacadePatternlink]** : 복잡한 시스템에 대한 간소화된 인터페이스를 제공하여 복잡성을 클라이언트로부터 숨기는 디자인 패턴입니다.       
			* **[플라이웨이트 패턴][FlyweightPatternlink]** : 유사한 상태를 가진 객체를 공유하여 인스턴스의 수를 줄이고 메모리 사용량을 최소화하여 성능을 향상시키는 디자인 패턴입니다.      
			* **[프록시 패턴][ProxyPatternlink]** : 다른 객체에 대한 대리자 또는 대체물을 제공하여 해당 객체에 대한 접근을 제어하고 추가 기능을 제공하는 디자인 패턴입니다. 예를 들어, 지연 초기화 또는 접근 제어와 같은 기능을 제공할 수 있습니다.
     		</details> 
		* <details> <summary><b><em>행동 패턴(Behavioral)</em></b> </summary>
			
			* **[책임 연쇄 패턴][ChainOfResponsibilityPatternlink]** : 여러 객체가 연쇄적으로 요청을 처리할 수 있는 디자인 패턴으로, 요청이 처리되거나 체인의 끝에 도달할 때까지 요청이 체인을 따라 전달됩니다.        
			* **[커맨드 패턴][CommandPatternlink]** : 클라이언트의 요청을 객체로 캡슐화하여 다양한 시점에 대기열, 요청 및 작업을 호출할 수 있도록 하는 디자인 패턴.             
			* **[인터프리터 패턴][InterpreterPatternlink]** : 언어의 문법을 정의하고 그 언어의 문장을 해석하는 디자인 패턴으로, 특정 도메인 언어를 해석하고 해당 언어를 실행 가능한 코드나 동작으로 변환하는 데 유용한 패턴입니다.            
			* **[이터레이터 패턴][IteratorPatternlink]** : 집합 객체의 요소에 순차적으로 액세스하는 방법을 제공하여 내부 표현을 노출하지 않고도 해당 요소에 접근하는 디자인 패턴입니다.            
			* **[중재자 패턴][MediatorPatternlink]** : 상호작용하는 객체 사이의 결합도를 줄이기 위해 중재자 객체를 통해 통신을 중앙 집중화하는 디자인 패턴입니다.            
			* **[메멘토 패턴][MementoPatternlink]** : 객체의 특정 상태로 다시 되돌아올 수 있도록 하는 설계 패턴입니다.           
			* **[옵저버 패턴][ObserverPatternlink]** : 객체가 상태를 변경할 때 관련 객체들이 업데이트를 받는 설계 패턴입니다.      
			* **[상태 패턴][StatePatternlink]** : 객체가 내부 상태가 변경될 때 동작을 변경할 수 있도록 하며, 상태별 동작을 별도의 클래스로 캡슐화하는 설계 패턴입니다.        
			* **[전략 패턴][StrategyPatternlink]** : 동일 계열의 알고리즘을 정의하고 각 알고리즘을 캡슐화하며, 동일한 계열 내에서 교환 가능하게 만드는 설계 패턴입니다.     
			* **[방문자 패턴][VisitorPatternlink]** : 객체 구조를 수정하지 않고 새로운 작업을 추가하여 알고리즘과 객체를 분리하는 설계 패턴입니다.
  		</details>
     * <details> <summary><b><em> ETC </em></b> </summary>
     
		* **[오브젝트풀 패턴][ObjectPoolingBaselink]**  : 객체를 재사용하여 자주 발생하는 가비지 컬렉션 호출을 줄여서 메모리 사용을 효율적으로 개선하는 패턴.
		* **[몬스터 AI FSM][FSMlink]**  : 객체의 동작을 다양한 상태로 나누고, 이 상태들 간의 전환과 각 상태에서의 행동을 관리하는 패턴.
		* **[몬스터 AI BehaviorTree][BehaviorTreelink]**  : 객체의 동작을 트리 구조 내의 노드로 구성하여 객체의 동작과 결정을 효과적으로 관리하는 디자인패턴.
		* **[Dependency Injection (DI)][DependencyInjectionlink]** : 클래스 내부에 종속성을 하드 코딩하는 대신 외부에 주입하여 클래스 간의 종속성을 관리하는 방법을 제공합니다.
	</details>

 * <details> <summary><b><em>프로그래밍 패러다임</em></b> </summary>
	
	* **[데이터 지향 설계, Data Oriented Design (DOD)][DODlink]** : 프로그램을 데이터의 구조와 접근 방식을 중심으로 설계하여 성능 향상과 메모리 사용 최적화를 목표로 하는 개발 방법론.    
   
* <details> <summary><b><em>아키텍처 패턴</em></b> </summary>
	
	* **[Entity Component System (ECS)][ECSlink]** : 게임 엔터티의 데이터 및 동작을 구성하기 위해 게임 개발에서 일반적으로 사용되는 패턴입니다. 게임 개체를 더 작은 구성 요소로 분해하여 더 나은 확장성과 재사용성을 가능하게 합니다.    
   
* <details> <summary><b><em>Books</em></b> </summary>
        
	* **[Effective C++][EffectiveClink]**
   
*****      
</details> 
<details><summary><b><em>다양한 기능구현</em></b> </summary>

*****

Unity:    
* **[체력바][HPBarlink]**  : 체력을 가진 오브젝트 머리위에 표시되는 막대로 HP상태를 알려주는 기능.
* **[로프액션][GrapplingHookslink]**  : 마우스 에임 방향으로 로프를 발사해 그 곳으로 직선이동이나 스윙이동하는 기능.
* **[Json 데이터 관리][JsonDataManagerlink]**  : Json으로 데이터를 관리 및 저장하는 시스템
* **[메쉬로 도형그리기][DrawShapeMeshlink]**  : Unity Graphics 시스템을 이용해서 도형모양의 메쉬를 생성해 보여주는 기능
* **[범위공격 시스템][RangeHitSystemlink]**  : 메쉬로 도형을 그리면서 그 구역내에 있는 오브젝트에 대미지를 주는 방식.
* **[캐릭터 이동관련 스크립트][Movementlink]**  :  캐릭터가 이동하는 방식에 대한 스크립트.
* **[파쿠르 시스템][Parkourlink]**  :  캐릭터가 특정 오브젝트와 상호작용해서 구조물을 활용해 이동하는 시스템.
* **[IK][IKlink]**  : 손과 발의 IK(역운동학) 사용해보는 스크립트.
* **[스킬트리][SkillTreelink]**  : Path of Exile 스킬트리처럼 나무가지가 뻣어나가는 형태의 스킬트리 구현
* **[퀵슬롯스킬][QuickSlotSkilllink]**  : UI에 스킬을 등록해서 스킬을 사용하고 쿨타임같은 사용경험을 높여주는 기능 구현
* **[반응형UI][UIResolutionAdaptationlink]**  : 해상도가 변경됨에 따라 UI의 위치조정
* **[카메라][Cameralink]**  : 플레이어의 카메라 구현
* **[전투시스템][CombatSystemlink]**  : 플레이어의 전투시스템 관련 스크립트 모음    
* **[플레이어스킬구현][PlayerSkilllink]**  : 플레이어가 사용하는 스킬을 구현해보는 부분    
* **[월드 생성 시스템][WorldSystemlink]**  : 청크 단위로 월드를 시드 값을 통해 절차적으로 생성하고 풀링을 통해 관리하는 기능
* **[스캔 시스템][ScanSystemlink]**  : 플레이어 기준으로 특정 오브젝트를 탐지하는 시스템
* **[씬 전환 시스템][ScenManagerlink]**  : 씬 전환시 로딩 씬으로 진입해 비동기 로딩 및 로딩 화면을 통한 부드러운 씬 전환 구현
* **[시네머신을 활용한 타이틀씬][CinemachineTitleScreenlink]**  : 시작, 옵션, 끝내기
* **[게임 사용자 설정 화면][SettingScreenlink]**  : 사용자가 옵션을 설정할 수 있도록 하는 화면  
* **[미니맵][MiniMaplink]**  : 플레이어 근처의 오브젝트 정보를 간단하게 UI를 통해 전달하는 시스템    
* **[NPC 대화 시스템][NpcDialogSystemlink]**  : 대화 스크립트가 담긴 csv를 불러와서 대화를 진행하는 듯한 연출을 해주는 시스템
* **[퀘스트 시스템][QuestSystemlink]**  : 퀘스트를 통해 플레이어에게 방향성을 제공하는 시스템

아래의 목록은 코드 공개 예정이 없거나 정리가 안돼서 스크립트 업로드가 안된 기능들입니다.

* **[인벤토리][InventorySystemlink]**  : 흔히 플레이어가 사용하는 인벤토리를 구현 ( 정렬, 슬롯 추가 및 슬롯 잠금, 아이템 스왑 등 )
* **[공중섬 조종 및 건설][IslandSystemlink]**  : 레프트라는 게임의 배처럼 플레이어가 땅을 늘리고 그 땅덩어리를 방향과 속도 등을 조절할 수 있는 시스템
* **[포탈 저장서 및 포탈][PortalSystemlink]**  : 플레이어가 저장한 포탈을 관리하고 생성하는 시스템, 포탈은 이동할 위치를 이미지를 통해 보여줌 (수정)
* **[글라이딩 시스템][GlidingSystemlink]**  : 날개 비행 시스템
* **[뱀서라이크 만들어보기][GamesLikeVampireSurvivorslink]**  : 몹 소환, 레벨링시스템, 스킬관련 등


*****
 </details>
<details>
 <summary><b><em> 코딩테스트 및 알고리즘 </em></b> </summary>

*****

* **[간단한 알고리즘 정리][Algorithmllink]** : 간단하게 알고리즘 정리
* **[백준][BAEKJOONllink]**  : 코딩테스트 사이트인 백준을 풀고 정리한 폴더

*****
 </details>
<details>
<summary><b><em>팀 프로젝트</em></b> </summary>

*****

* **[TeamDemonStrate][TeamProjectDemonStratelink]**  : 21.04.13 ~ 21.10.12 학교생활 중 진행한 중단된 팀 프로젝트에서 만들었던 코드
 
*****
 </details>   
     
[3dsuikagamelink]: /BuildGame/3DSuikaGame  

[ObjectPoolingBaselink]: /DesignPatterns/ObjectPoolingBase
[FSMlink]: /DesignPatterns/MonsterAI/FSM

[Singletonlink]: /ComputerScience/DesignPatterns/GoF/Creational/Singleton
[Factorylink]: /ComputerScience/DesignPatterns/GoF/Creational/FactoryPattern
[AbstractFactorylink]: /ComputerScience/DesignPatterns/GoF/Creational/AbstractFactoryPattern
[Builderlink]: /ComputerScience/DesignPatterns/GoF/Creational/BuilderPattern
[PrototypePatternlink]: /ComputerScience/DesignPatterns/GoF/Creational/PrototypePattern
[AdapterPatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/AdapterPattern
[BridgePatternlinklink]: /ComputerScience/DesignPatterns/GoF/Structural/BridgePattern
[CompositePatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/CompositePattern
[DecoratorPatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/DecoratorPattern
[BridgePatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/BridgePattern
[BehaviorTreelink]: /ComputerScience/DesignPatterns/MonsterAI/BehaviorTree
[FacadePatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/FacadePattern
[FlyweightPatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/FlyweightPattern
[ProxyPatternlink]: /ComputerScience/DesignPatterns/GoF/Structural/ProxyPattern
[ChainOfResponsibilityPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/ChainOfResponsibility
[CommandPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/CommandPattern
[InterpreterPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/InterpreterPattern
[IteratorPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/IteratorPattern
[MediatorPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/MediatorPattern
[MementoPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/MementoPattern
[ObserverPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/ObserverPattern
[StatePatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/StatePattern
[StrategyPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/StrategyPattern
[VisitorPatternlink]: /ComputerScience/DesignPatterns/GoF/Behavioral/VisitorPattern
[ECSlink]: /ComputerScience/ArchitecturePatterns/ECS(EntityComponentSystem)
[DependencyInjectionlink]: /ComputerScience/DesignPatterns/DependencyInjection
[DODlink]: /ComputerScience/ProgrammingParadigm/DataOrientedDesign


[HPBarlink]: /VariousFunctions/HPBar   
[GrapplingHookslink]: /VariousFunctions/GrapplingHooksAndRopeSwing
[JsonDataManagerlink]: /VariousFunctions/JsonDataManager
[DrawShapeMeshlink]: /VariousFunctions/DrawShapeMesh
[RangeHitSystemlink]: /VariousFunctions/RangeHitSystem
[Movementlink]: /VariousFunctions/Movement&Parkour/Movement
[Parkourlink]: /VariousFunctions/Movement&Parkour/Parkour
[IKlink]: /VariousFunctions/IK(InverseKinematics)
[SkillTreelink]: /VariousFunctions/SkillSystem/SkillTree
[QuickSlotSkilllink]: /VariousFunctions/SkillSystem/QuickSlotSkill
[UIResolutionAdaptationlink]: /VariousFunctions/UIResolutionAdaptation
[Cameralink]: /VariousFunctions/Camera
[CombatSystemlink]: /VariousFunctions/CombatSystem
[PlayerSkilllink]: /VariousFunctions/SkillSystem/PlayerSkill
[WorldSystemlink]: /VariousFunctions/WorldSystem
[ScenManagerlink]: /VariousFunctions/ScenManager
[InventorySystemlink]: /VariousFunctions/InventorySystem
[ScanSystemlink]: /VariousFunctions/WorldScan
[IslandSystemlink]: /VariousFunctions/IslandSystem
[PortalSystemlink]: /VariousFunctions/PortalSystem
[GlidingSystemlink]: /VariousFunctions/Gliding
[CinemachineTitleScreenlink]: /VariousFunctions/CinemachineTitleScreen
[SettingScreenlink]: /VariousFunctions/SettingScreen
[GamesLikeVampireSurvivorslink]: /VariousFunctions/GamesLikeVampireSurvivors
[MiniMaplink]: /VariousFunctions/MiniMap
[NpcDialogSystemlink]: /VariousFunctions/NpcDialogSystem
[QuestSystemlink]: /VariousFunctions/QuestSystem

[Algorithmllink]: /ComputerScience/DataStructure_Algorithm_CodingTest/CodingTest   
[BAEKJOONllink]: /ComputerScience/DataStructure_Algorithm_CodingTest/CodingTest/Baekjoon   

[EffectiveClink]: /ComputerScience/Books/EffectiveC++

[TeamProjectDemonStratelink]: /TeamProject/DemonStrate
