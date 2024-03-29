using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainofResponsibility
{
    // NPC 상호작용을 다루는 핸들러 인터페이스
    public interface INpcHandler
    {
        void HandleInteractionRequest(Player player, Npc npc);
    }

    // 기본 플레이어 클래스
    public class Player
    {
    }

    // NPC 클래스
    public class Npc
    {
        public bool IsQuestGiver()
        {
            return true; // 예시를 위해 true로 설정
        }

        public bool IsMerchant()
        {
            return false; // 예시를 위해 false로 설정
        }

        public bool IsDialogueNPC()
        {
            return false; // 예시를 위해 false로 설정
        }

        public bool IsTrainerNPC()
        {
            return false; // 예시를 위해 false로 설정
        }

        public bool IsInnkeeperNPC()
        {
            return false; // 예시를 위해 false로 설정
        }

        public void GiveQuest(Player player)
        {
            Debug.Log("NPC가 퀘스트를 주었습니다.");
        }

        public void SellItems(Player player)
        {
            Debug.Log("NPC가 아이템을 판매합니다.");
        }

        public void StartDialogue(Player player)
        {
            Debug.Log("NPC와 대화를 시작합니다.");
        }

        public void TrainPlayer(Player player)
        {
            Debug.Log("NPC가 플레이어를 훈련합니다.");
        }

        public void ReserveRoom(Player player)
        {
            Debug.Log("NPC가 방을 예약합니다.");
        }
    }

    // QuestGiverHandler 클래스: 퀘스트를 주는 NPC 상호작용을 처리하는 구체적인 핸들러
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
                // 퀘스트 상호작용 처리
                npc.GiveQuest(player);
            }
            else if (nextHandler != null)
            {
                // 다음 핸들러에게 전달
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // MerchantHandler 클래스: 아이템 판매 NPC 상호작용을 처리하는 구체적인 핸들러
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
                // 거래 상호작용 처리
                npc.SellItems(player);
            }
            else if (nextHandler != null)
            {
                // 다음 핸들러에게 전달
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // DialogueHandler 클래스: 대화 NPC 상호작용을 처리하는 구체적인 핸들러
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
                // 대화 상호작용 처리
                npc.StartDialogue(player);
            }
            else if (nextHandler != null)
            {
                // 다음 핸들러에게 전달
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // TrainerHandler 클래스: 트레이너 NPC 상호작용을 처리하는 구체적인 핸들러
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
                // 훈련 상호작용 처리
                npc.TrainPlayer(player);
            }
            else if (nextHandler != null)
            {
                // 다음 핸들러에게 전달
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // InnkeeperHandler 클래스: 여관주인 NPC 상호작용을 처리하는 구체적인 핸들러
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
                // 여관 상호작용 처리
                npc.ReserveRoom(player);
            }
            else if (nextHandler != null)
            {
                // 다음 핸들러에게 전달
                nextHandler.HandleInteractionRequest(player, npc);
            }
        }
    }

    // 게임 관리자 클래스
    public class GameManager : MonoBehaviour
    {
        private INpcHandler interactionHandlerChain;

        void Start()
        {
            // NPC 핸들러 체인 구축
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

            // NPC와의 상호작용 시뮬레이션
            Player player = new Player();
            Npc npc = new Npc();

            interactionHandlerChain.HandleInteractionRequest(player, npc);
        }
    }
}
