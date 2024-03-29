using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BridgePattern 
{
    // Abstraction (추상화)
    public abstract class Platform
    {
        protected IInputHandler inputHandler;

        public Platform(IInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        public abstract void ProcessInput();
    }

    // Implementor (구현)
    public interface IInputHandler
    {
        void HandleInput();
    }

    // Concrete Implementor 1 (구체적 구현)
    public class KeyboardInputHandler : IInputHandler
    {
        public void HandleInput()
        {
            Debug.Log("키보드 입력 처리");
        }
    }

    // Concrete Implementor 2 (구체적 구현)
    public class TouchInputHandler : IInputHandler
    {
        public void HandleInput()
        {
            Debug.Log("터치스크린 입력처리");
        }
    }

    // Refined Abstraction 1 (세분화된 추상화)
    public class PCPlatform : Platform
    {
        public PCPlatform(IInputHandler inputHandler) : base(inputHandler) { }

        public override void ProcessInput()
        {
            inputHandler.HandleInput();
        }
    }

    // Refined Abstraction 2 (세분화된 추상화)
    public class MobilePlatform : Platform
    {
        public MobilePlatform(IInputHandler inputHandler) : base(inputHandler) { }

        public override void ProcessInput()
        {
            inputHandler.HandleInput();
        }
    }

    // Client (클라이언트)
    public class GameManager : MonoBehaviour
    {
        private Platform platform;

        void Start()
        {
            // 키보드 입력을 처리하는 플랫폼
            platform = new PCPlatform(new KeyboardInputHandler());
            platform.ProcessInput();

            // 터치 입력을 처리하는 플랫폼
            platform = new MobilePlatform(new TouchInputHandler());
            platform.ProcessInput();
        }
    }
}
