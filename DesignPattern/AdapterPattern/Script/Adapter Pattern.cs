using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdapterPattern
{
    // ����� �������̽�
    public interface ITarget
    {
        void Request();
    }

    // ����� Ŭ����
    public class Adapter : ITarget
    {
        private ExternalSystem externalSystem;

        public Adapter(ExternalSystem externalSystem)
        {
            this.externalSystem = externalSystem;
        }

        public void Request()
        {
            externalSystem.PerformRequest();
        }
    }

    // ������ Ŭ���� �Ǵ� ���̺귯�� ( �����ؾߵ� Ŭ���� )
    public class ExternalSystem
    {
        public void PerformRequest()
        {
            Debug.Log("�ܺ� �ý��� ��û ����");
        }
    }

    // Ŭ���̾�Ʈ
    public class Client : MonoBehaviour
    {
        void Start()
        {
            // ������ Ŭ���� �Ǵ� ���̺귯���� ����͸� ���� ���
            ITarget adapter = new Adapter(new ExternalSystem());
            adapter.Request();
        }
    }
}
