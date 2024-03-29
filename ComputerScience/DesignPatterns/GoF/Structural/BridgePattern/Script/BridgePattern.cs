using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BridgePattern 
{
    // Abstraction (�߻�ȭ)
    public abstract class Platform
    {
        protected IInputHandler inputHandler;

        public Platform(IInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        public abstract void ProcessInput();
    }

    // Implementor (����)
    public interface IInputHandler
    {
        void HandleInput();
    }

    // Concrete Implementor 1 (��ü�� ����)
    public class KeyboardInputHandler : IInputHandler
    {
        public void HandleInput()
        {
            Debug.Log("Ű���� �Է� ó��");
        }
    }

    // Concrete Implementor 2 (��ü�� ����)
    public class TouchInputHandler : IInputHandler
    {
        public void HandleInput()
        {
            Debug.Log("��ġ��ũ�� �Է�ó��");
        }
    }

    // Refined Abstraction 1 (����ȭ�� �߻�ȭ)
    public class PCPlatform : Platform
    {
        public PCPlatform(IInputHandler inputHandler) : base(inputHandler) { }

        public override void ProcessInput()
        {
            inputHandler.HandleInput();
        }
    }

    // Refined Abstraction 2 (����ȭ�� �߻�ȭ)
    public class MobilePlatform : Platform
    {
        public MobilePlatform(IInputHandler inputHandler) : base(inputHandler) { }

        public override void ProcessInput()
        {
            inputHandler.HandleInput();
        }
    }

    // Client (Ŭ���̾�Ʈ)
    public class GameManager : MonoBehaviour
    {
        private Platform platform;

        void Start()
        {
            // Ű���� �Է��� ó���ϴ� �÷���
            platform = new PCPlatform(new KeyboardInputHandler());
            platform.ProcessInput();

            // ��ġ �Է��� ó���ϴ� �÷���
            platform = new MobilePlatform(new TouchInputHandler());
            platform.ProcessInput();
        }
    }
}
