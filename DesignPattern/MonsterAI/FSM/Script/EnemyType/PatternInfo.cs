using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PatternInfo : MonoBehaviour
{
    public bool IsPattern = false;
    public Queue<IEnumerator> waitHpPattern = new Queue<IEnumerator>();
    public LinkedList<IEnumerator> waitCooldownPattern = new LinkedList<IEnumerator>();

    // 패턴 배열을 확인해서 사용가능한게 있으면 사용하도록하는 함수
    public abstract bool HpPattern();
    public abstract bool CooldownPattern();

    // 일반공격
    public abstract bool BasicAttack();

    /*
     몬스터의 체력을 확인해서 관리하는 함수  + 생각중 : update나 시작시 코루틴으로 반복횟수 조절가능.
    1) 체력에 맞는 패턴을 waitHpPattern queue에 추가 
    2) 죽음을 패턴에 추가해주면 가능한것 -> 몬스터에 따라 컷신이 있거나 뭐 다양한 연출이나 이름이 다른 애니메이션설정 가능
     */
    public abstract void HpCheck();

    /*
     쿨타임을 가지는 패턴 추가 고민중
    
    방법1)  몬스터가 생성이되면 전부 패턴안에 집어넣음
    방법2) serch 상태에서 target이 생기면 리스트 초기화 및 쿨타임패턴 재설정
     */
    //public abstract void AddCooldownPattern();

}
