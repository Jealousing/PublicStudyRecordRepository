# ECS(Entity Component System) 아키텍처 패턴

## 기능 설명
 Entity Component System (ECS)은 Unity 게임 개발에서 사용되는 중요한 아키텍처 패턴 중 하나입니다.    
이 패턴은 게임 객체를 엔터티, 컴포넌트 및 시스템으로 나누어 구성함으로써 객체 지향 프로그래밍(Object-Oriented Programming, OOP) 대신 데이터 중심 설계(Data-Driven Design)에 중점을 둡니다.    

## ECS의 주요 특징 
ECS는 OOP의 한계를 극복하기 위해 개발되었으며, 데이터 중심 설계를 통해 성능과 확장성을 향상시킵니다.    
이를 통해 Unity의 DOTS 프레임워크와 통합되어 대규모 게임 환경에서도 뛰어난 성능을 제공합니다.    
또한, ECS는 병렬 처리를 용이하게 하여 다중 스레드에서 효율적으로 동작할 수 있습니다.   

## 핵심 개념
* 엔터티(Entity): ECS에서 엔터티는 게임 객체를 나타내는 추상적인 개념입니다. 이는 엔터티 ID로 식별되며 행동을 나타내지 않습니다. 대신, 엔터티는 하나 이상의 컴포넌트를 가질 수 있습니다.    
* 컴포넌트(Component): 컴포넌트는 데이터만을 포함하며 특정 행동이나 동작을 갖지 않는 단순한 구조입니다. 예를 들어, 위치, 회전, 렌더링 정보 등을 저장할 수 있습니다.   
* 시스템(System): 시스템은 특정 작업을 수행하는 일련의 로직을 담당합니다. 각 시스템은 특정 컴포넌트의 집합을 처리하고, 이를 기반으로 엔터티의 동작을 결정합니다. 예를 들어, 이동 시스템은 모든 이동 가능한 엔터티를 처리하여 위치를 업데이트할 수 있습니다.    
* 아키타입(Archetype): 아키타입은 엔터티의 종류를 정의하는 데 사용됩니다. 이는 유사한 컴포넌트 구성을 가진 엔터티를 그룹화하여 성능을 최적화하고 메모리를 관리하기 위해 중요한 개념입니다.   
* 메모리 청크(Memory Chunk): 메모리 청크는 동일한 Archetype의 엔티티를 저장하는 연속적인 메모리 영역입니다. 이러한 구조는 데이터의 연속성을 유지하여 CPU 캐시 효율성을 향상시킵니다.   

 ### 장점
* 뛰어난 성능: 메모리 관리와 병렬 처리를 통해 높은 성능을 제공합니다.   
* 확장성: 대규모 게임 환경에서도 효과적으로 확장할 수 있습니다.   
* 유연성: 데이터 중심 설계를 통해 변경과 수정이 용이합니다.   

 ### 단점
* 학습 어려움: 전통적인 OOP와는 다른 패러다임이기 때문에 학습이 어려울 수 있습니다.   
* 복잡성: 시스템과 컴포넌트 간의 상호 작용을 이해하는 데 시간이 걸릴 수 있습니다.   

## ECS와 OOP의 차이점
ECS는 객체 지향 프로그래밍과는 다르게 데이터 중심 설계에 중점을 둡니다. 이는 상속이나 객체 간의 복잡한 상호 작용보다는 데이터의 흐름과 처리에 초점을 맞추어 성능을 향상시키는 방향입니다.    

## Unity 게임 개발에서의 활용
ECS는 Unity 게임 개발에서 대규모 엔터티 시스템, 실시간 물리 시뮬레이션, 고성능 AI 등의 시나리오에 적합합니다. DOTS와의 통합으로 인해 Unity에서 최신 기능과 함께 사용할 수 있으며, 메모리 최적화와 성능 향상을 위한 강력한 방법을 제공합니다.    

## 예제 코드

```cs
// 예제 코드: 간단한 이동 시스템 구현

// 필요한 네임스페이스
using Unity.Entities;
using UnityEngine;

// 위치 컴포넌트 정의
public struct Position : IComponentData
{
    public float3 Value;
}

// 이동 시스템 정의
public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // 모든 엔터티의 위치 컴포넌트 업데이트
        Entities.ForEach((ref Position position) =>
        {
            // 간단한 이동 로직: 위치를 이동 방향으로 이동
            position.Value += new float3(1, 0, 0) * Time.DeltaTime;
        }).Schedule();
    }
}

// 예제에서 사용할 엔터티 생성 및 초기화
public class ECSExample : MonoBehaviour
{
    EntityManager entityManager;

    void Start()
    {
        // EntityManager 가져오기
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // 엔터티 생성 및 위치 컴포넌트 추가
        Entity entity = entityManager.CreateEntity(typeof(Position));

        // 위치 컴포넌트 설정
        entityManager.SetComponentData(entity, new Position { Value = new float3(0, 0, 0) });

        // 이동 시스템 등록
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementSystem>();
    }
}
```

## 느낀 점
 ECS를 학습하고 적용하는 과정에서 얻은 경험은 깊었습니다. OOP 즉 객체 지향 프로그래밍이 램의 성능 향상보다 cpu의 성능향상이 빨라서 cpu를 더 사용하는 방식의 ECS(Entity Component System) 아키텍처 패턴이 성능향상에 큰 도움이 된다는게 신기했습니다.    
아직도 객체 지향 프로그래밍과 다르기 때문에 다룸에 있어서는 어려움을 겪겠지만, 배움으로서는 ECS의 가치를 깨닫게 되었습니다.   
ECS는 게임 개발에서 성능과 확장성이 중요한 경우에 매우 유용합니다. 데이터 주도 설계의 중요성을 이해하게 되었고, 성능이 향상되고 유연성이 향상되는 이점을 이해할 수 있었ㅅ브니다.
또한 ECS는 대규모 게임 환경에서 성능 향상을 위해 고려할 수 있게 되었습니다.
물론 초기 학습이 OOP구조와 다르기 때문에 처음에는 어려움이 있을 수 있지만, 성능과 유연성이 향상된 이점은 이를 공부하는 데 있어서 가치 있는 아키텍처 패턴이라고 생각합니다.    
또한, Unity 개발에서 ECS를 사용하는 것은 대규모 게임 환경을 효율적으로 처리하는 방법에 대한 통찰력을 제공하여 최적화된 메모리 사용 및 원활한 게임 플레이 경험을 얻을 수 있습니다.   
전반적으로 ECS는 게임 아키텍처에 대한 이해를 넓혀주고 더 견고하고 확장 가능한 게임 시스템을 만들 수 있도록 도와줍니다.

 유니티에서도 22.3, 23.3 버전에서 Unity DOTS가 지원하기 시작했고 Unity 게임을 개발할때 성능에 대한 이슈로 최적화를 위해 채택을 살펴보고 고려해볼 수 있게 되었다고 생각한다.

## [강의 링크] (https://youtu.be/7UphiG8UtTg)
