using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : Skill
{
    public abstract void Apply();
}


/*
 
패시브 스킬을 구현을 위한 아이디어노트

패시브 스킬의 종류? -> 스탯 상승패시브(데미지,hp관련,저항수치 등),  특정 동작 해금(?)

1) 스텟 상승 패시브
PlayerInfo의 PlayerStats변수에서 스텟을 관리하고 있으며 패시브 스킬을 스킬트리에서 찍은 스킬만 저장되도록 구성하기

스킬트리를 저장하는 순간 -> 1 - 플레이어의 패시브 목록을 초기화
                                        2 - 스킬트리에서 노드정보를 저장 및 적용
                                        3 - 노드 저장할때 해당 노드가 패시브 스킬이면 플레이어정보에 접근해서 패시브리스트에 추가.

 */