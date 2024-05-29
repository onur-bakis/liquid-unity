using System;
using Scripts.Enums;
using Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Handler
{
    public class UIEventSubscriber : MonoBehaviour
    {

        [SerializeField] private UIEventSubscriptionTypes type;
        [SerializeField] private Button button;

        [Inject] public UIManager _uIManager;
        
        [Inject]
        public void Construct(UIManager uiManager)
        {
            Debug.Log("Construct");
            _uIManager = uiManager;
        }

        public void ButtonClick()
        {
            switch (type)
            {
                case UIEventSubscriptionTypes.OnSeeLevels:
                {
                    Debug.Log("SeeLevels");
                    _uIManager.OnSeeLevels();
                    break;
                }
                case UIEventSubscriptionTypes.OnTapToContinue:
                {
                    _uIManager.OnTapToContinueButton();
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
        private void SubscribeEvents()
        {
            button.onClick.AddListener(ButtonClick);
        }

        private void UnSubscribeEvents()
        {
            button.onClick.RemoveListener(ButtonClick);
        }

        public  void OnDisable()
        {
            UnSubscribeEvents();
        }
    }

    
}