using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : Skill
{
    public abstract void Apply();
}


/*
 
�нú� ��ų�� ������ ���� ���̵���Ʈ

�нú� ��ų�� ����? -> ���� ����нú�(������,hp����,���׼�ġ ��),  Ư�� ���� �ر�(?)

1) ���� ��� �нú�
PlayerInfo�� PlayerStats�������� ������ �����ϰ� ������ �нú� ��ų�� ��ųƮ������ ���� ��ų�� ����ǵ��� �����ϱ�

��ųƮ���� �����ϴ� ���� -> 1 - �÷��̾��� �нú� ����� �ʱ�ȭ
                                        2 - ��ųƮ������ ��������� ���� �� ����
                                        3 - ��� �����Ҷ� �ش� ��尡 �нú� ��ų�̸� �÷��̾������� �����ؼ� �нú긮��Ʈ�� �߰�.

 */