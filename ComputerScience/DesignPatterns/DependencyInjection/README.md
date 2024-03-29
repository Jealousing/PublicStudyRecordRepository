# 의존성 주입 디자인 패턴 (Dependency Injection, DI)

## 기능 설명
 의존성 주입(Dependency Injection, DI)은 객체 간의 의존성을 외부에서 주입하는 디자인 패턴입니다.   
이는 객체가 직접 필요한 의존성을 생성하거나 결정하지 않고, 외부에서 제공되는 방식으로 의존성을 해결함으로써 코드의 유연성과 재사용성을 향상시킵니다.   
   
 ### 주요 개념
* 의존성(Dependency): 한 객체가 다른 객체에 의존할 때, 그 객체를 의존성이라고 합니다. 예를 들어, 클래스 A가 클래스 B의 인스턴스를 사용한다면 클래스 A는 클래스 B에 의존성을 가지고 있습니다.   
* 의존성 주입(Dependency Injection): 의존성 주입은 객체가 직접 의존하는 객체를 생성하거나 결정하지 않고, 외부에서 의존성을 주입받는 방식입니다. 이는 객체 간의 결합도를 낮추고 테스트 용이성을 증가시킵니다.   
* 의존성 컨테이너(Dependency Container): 의존성 주입을 관리하는 도구로, 의존성 주입 컨테이너라고도 합니다. 이는 객체를 생성하고 관리하며, 필요한 의존성을 주입합니다.   
   
 ### 장점
* 코드의 유연성과 재사용성 향상: 객체 간의 결합도를 줄이고, 객체를 재사용하거나 교체하기 쉽게 합니다.   
* 객체 간의 결합도 감소: 각 객체가 서로 어떻게 생성되고 사용되는지에 대한 정보를 덜 필요로 합니다.   
* 테스트 용이성 증대: 의존성을 주입하므로써 테스트하기 쉽고 모의 객체(Mock Object)를 쉽게 사용할 수 있습니다.   

 ### 단점 
* 복잡성 증가: DI를 사용하면 추가적인 설정이나 코드가 필요하며, 초기에는 학습 곡선이 있을 수 있습니다.   

 ### 의존성 주입 방법
* 생성자 주입(Constructor Injection): 객체의 생성자를 통해 의존성을 주입합니다. 필수적으로 필요한 의존성이 있을 때 사용됩니다.   
* 설정 메서드 주입(Setter Injection): 의존성을 설정하는 메서드를 통해 주입합니다. 선택적인 의존성을 주입할 때 사용됩니다.   
* 인터페이스 주입(Interface Injection): 인터페이스를 통해 의존성을 주입합니다. 구체적인 사용 사례에 따라 선택될 수 있습니다.   

 ### 간단한 예시 코드
``` c#
using UnityEngine;

// 의존성 주입을 받는 컴포넌트
public class PlayerController : MonoBehaviour
{
    private IWeapon weapon;

    // 생성자 주입(Constructor Injection)
    public PlayerController(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 무기를 사용
            weapon.Fire();
        }
    }
}

// 의존성을 제공하는 클래스
public class Gun : MonoBehaviour, IWeapon
{
    // 무기 사용 메서드
    public void Fire()
    {
        Debug.Log("총 발사!");
    }
}

// 무기 인터페이스
public interface IWeapon
{
    void Fire();
}

// 의존성 주입 및 사용
public class Main : MonoBehaviour
{
    void Start()
    {
        // 총 생성
        Gun gun = new Gun();
        // 플레이어 컨트롤러에 의존성 주입
        PlayerController playerController = new PlayerController(gun);
    }
}
```

## 어떤 경우 사용하면 좋을까
* 게임에서 다양한 무기나 아이템을 간편하게 교체하고자 할 때
* 여러 유형의 플레이어나 적 캐릭터에게 동일한 기능을 제공하고자 할 때
* 유닛 테스트를 통해 무기 동작을 검증하고자 할 때

## 느낀 점
 의존성 주입을 사용하여 게임 내에서 다양한 기능을 교체하고 유연하게 확장할 수 있다는 것을 알게 되었습니다.    
DI 패턴은 코드의 유지보수가 훨씬 쉬워지고 코드 재사용성이 높아진다는 것을 알게 되었습니다.    
게임 개발에서는 주로 다양한 컴포넌트 간의 의존성을 관리해야 하므로 DI를 활용하는 것이 매우 유용하다고 생각됩니다.
