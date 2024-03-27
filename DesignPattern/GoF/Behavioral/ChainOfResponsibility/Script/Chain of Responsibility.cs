using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainofResponsibility
{
    // NPC ��ȣ�ۿ��� �ٷ�� �ڵ鷯 �������̽�
    public interface INpcHandler
    {
        void HandleInteractionRequest(Player player, Npc npc);
    }

    // �⺻ �÷��̾� Ŭ����
    public class Player
    {
    }

    // NPC Ŭ����
    public class Npc
    {
        public bool IsQuestGiver()
        {
            return true; // ���ø� ���� true�� ����
        }

        public bool IsMerchant()
        {
            return false; // ���ø� ���� false�� ����
        }

        public bool IsDialogueNPC()
        {
            return false; // ���ø� ���� false�� ����
        }

        public bool IsTrainerNPC()
        {
            return false; // ���ø� ���� false�� ����
        }

        public bool IsInnkeeperNPC()
        {
            return false; // ���ø� ���� false�� ����
        }

        public void GiveQuest(Player player)
        {
            Debug.Log("NPC�� ����Ʈ�� �־����ϴ�.");
        }

        public void SellItems(Player player)
        {
            Debug.Log("NPC�� �������� �Ǹ��մϴ�.");
        }

        public void StartDialogue(Player player)
        {
            Debug.Log("NPC�� ��ȭ�� �����մϴ�.");
        }

        public void TrainPlayer(Player player)
        {
            Debug.Log("NPC�� �÷��̾ �Ʒ��մϴ�.");
        }

        public void ReserveRoom(Player player)
        {
            Debug.Log("NPC�� ���� �����մϴ�.");
        }
    }

    // QuestGiverHandler Ŭ����: ����Ʈ�� �ִ� NPC ��ȣ�ۿ��� ó���ϴ� ��ü���� �ڵ鷯
    public class QuestGiverHandler : INpcHandler
    {
        private INpcHandler nextHandler;

        public void SetNextHandler(INpcHandler handler)
        {
            nextHandler = handler;
        }

        public void HandleInteractionRequest(Player player, Npc npc)
        {
            if (npc.IsQuestGiver())
            {
                // ����Ʈ ��ȣ�ۿ� ó��
                npc.GiveQuest(player);
            }
            else if (nextHandler != null)
            {
                // ���� �ڵ鷯���� ����
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // MerchantHandler Ŭ����: ������ �Ǹ� NPC ��ȣ�ۿ��� ó���ϴ� ��ü���� �ڵ鷯
    public class MerchantHandler : INpcHandler
    {
        private INpcHandler nextHandler;

        public void SetNextHandler(INpcHandler handler)
        {
            nextHandler = handler;
        }

        public void HandleInteractionRequest(Player player, Npc npc)
        {
            if (npc.IsMerchant())
            {
                // �ŷ� ��ȣ�ۿ� ó��
                npc.SellItems(player);
            }
            else if (nextHandler != null)
            {
                // ���� �ڵ鷯���� ����
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // DialogueHandler Ŭ����: ��ȭ NPC ��ȣ�ۿ��� ó���ϴ� ��ü���� �ڵ鷯
    public class DialogueHandler : INpcHandler
    {
        private INpcHandler nextHandler;

        public void SetNextHandler(INpcHandler handler)
        {
            nextHandler = handler;
        }

        public void HandleInteractionRequest(Player player, Npc npc)
        {
            if (npc.IsDialogueNPC())
            {
                // ��ȭ ��ȣ�ۿ� ó��
                npc.StartDialogue(player);
            }
            else if (nextHandler != null)
            {
                // ���� �ڵ鷯���� ����
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // TrainerHandler Ŭ����: Ʈ���̳� NPC ��ȣ�ۿ��� ó���ϴ� ��ü���� �ڵ鷯
    public class TrainerHandler : INpcHandler
    {
        private INpcHandler nextHandler;

        public void SetNextHandler(INpcHandler handler)
        {
            nextHandler = handler;
        }

        public void HandleInteractionRequest(Player player, Npc npc)
        {
            if (npc.IsTrainerNPC())
            {
                // �Ʒ� ��ȣ�ۿ� ó��
                npc.TrainPlayer(player);
            }
            else if (nextHandler != null)
            {
                // ���� �ڵ鷯���� ����
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // InnkeeperHandler Ŭ����: �������� NPC ��ȣ�ۿ��� ó���ϴ� ��ü���� �ڵ鷯
    public class InnkeeperHandler : INpcHandler
    {
        private INpcHandler nextHandler;

        public void SetNextHandler(INpcHandler handler)
        {
            nextHandler = handler;
        }

        public void HandleInteractionRequest(Player player, Npc npc)
        {
            if (npc.IsInnkeeperNPC())
            {
                // ���� ��ȣ�ۿ� ó��
                npc.ReserveRoom(player);
            }
            else if (nextHandler != null)
            {
                // ���� �ڵ鷯���� ����
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // ���� ������ Ŭ����
    public class GameManager : MonoBehaviour
    {
        private INpcHandler interactionHandlerChain;

        void Start()
        {
            // NPC �ڵ鷯 ü�� ����
            var questGiverHandler = new QuestGiverHandler();
            var merchantHandler = new MerchantHandler();
            var dialogueHandler = new DialogueHandler();
            var trainerHandler = new TrainerHandler();
            var innkeeperHandler = new InnkeeperHandler();

            questGiverHandler.SetNextHandler(merchantHandler);
            merchantHandler.SetNextHandler(dialogueHandler);
            dialogueHandler.SetNextHandler(trainerHandler);
            trainerHandler.SetNextHandler(innkeeperHandler);

            interactionHandlerChain = questGiverHandler;

            // NPC���� ��ȣ�ۿ� �ùķ��̼�
            Player player = new Player();
            Npc npc = new Npc();

            interactionHandlerChain.HandleInteractionRequest(player, npc);
        }
    }
}
