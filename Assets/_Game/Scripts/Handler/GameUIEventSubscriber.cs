using System;
using Scripts.Managers;
using Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Handler
{
    public class GameUIEventSubscriber : MonoBehaviour
    {
        [SerializeField] private GameUIEventSubscribtionTypes type;
        [SerializeField] private Button button;

        [Inject] public GameUIManager GameUIManager { get; set; }
        

        public void ButtonClick()
        {
            switch (type)
            {
                case GameUIEventSubscribtionTypes.FinishGame:
                {
                    GameUIManager.OnFinish();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnEnable()
        {
            SubscribeEvents();
        }
        public void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void SubscribeEvents()
        {
            button.onClick.AddListener(ButtonClick);
        }

        private void UnSubscribeEvents()
        {
            button.onClick.RemoveListener(ButtonClick);
        }
    }
}