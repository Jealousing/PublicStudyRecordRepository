using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdapterPattern
{
    // 어댑터 인터페이스
    public interface ITarget
    {
        void Request();
    }

    // 어댑터 클래스
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

    // 기존의 클래스 또는 라이브러리 ( 연결해야될 클래스 )
    public class ExternalSystem
    {
        public void PerformRequest()
        {
            Debug.Log("외부 시스템 요청 수행");
        }
    }

    // 클라이언트
    public class Client : MonoBehaviour
    {
        void Start()
        {
            // 기존의 클래스 또는 라이브러리를 어댑터를 통해 사용
            ITarget adapter = new Adapter(new ExternalSystem());
            adapter.Request();
        }
    }
}
