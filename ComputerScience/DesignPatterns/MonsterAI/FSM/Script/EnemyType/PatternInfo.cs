using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PatternInfo : MonoBehaviour
{
    public bool IsPattern = false;
    public Queue<IEnumerator> waitHpPattern = new Queue<IEnumerator>();
    public LinkedList<IEnumerator> waitCooldownPattern = new LinkedList<IEnumerator>();

    // ���� �迭�� Ȯ���ؼ� ��밡���Ѱ� ������ ����ϵ����ϴ� �Լ�
    public abstract bool HpPattern();
    public abstract bool CooldownPattern();

    // �Ϲݰ���
    public abstract bool BasicAttack();

    /*
     ������ ü���� Ȯ���ؼ� �����ϴ� �Լ�  + ������ : update�� ���۽� �ڷ�ƾ���� �ݺ�Ƚ�� ��������.
    1) ü�¿� �´� ������ waitHpPattern queue�� �߰� 
    2) ������ ���Ͽ� �߰����ָ� �����Ѱ� -> ���Ϳ� ���� �ƽ��� �ְų� �� �پ��� �����̳� �̸��� �ٸ� �ִϸ��̼Ǽ��� ����
     */
    public abstract void HpCheck();

    /*
     ��Ÿ���� ������ ���� �߰� �����
    
    ���1)  ���Ͱ� �����̵Ǹ� ���� ���Ͼȿ� �������
    ���2) serch ���¿��� target�� ����� ����Ʈ �ʱ�ȭ �� ��Ÿ������ �缳��
     */
    //public abstract void AddCooldownPattern();

}
