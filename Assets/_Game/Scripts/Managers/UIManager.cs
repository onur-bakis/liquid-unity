using Scripts.Context.Signals;
using Scripts.Controller.UI;
using Scripts.Enums;
using Scripts.Keys;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIPanelController _uiPanelController;

        [Inject] public SignalBus _signalBus;
        private OpenPanelSignal _openPanelSignal =new OpenPanelSignal();
        
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _openPanelSignal = new OpenPanelSignal();
        }
        
        public void Start()
        {
            OpenStartPanel();
        }
        public void OnTapToContinueButton()
        {
            _signalBus.Fire<TapToContinueSignal>();
            _signalBus.Fire<ResetGameSignal>();
        }
        
        public void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _signalBus.Subscribe<OnGameInitializeSignal>(OnPlayGameInitialize);
            _signalBus.Subscribe<LevelFinishedSignals>(OnLevelFinished);
            _signalBus.Subscribe<ResetGameSignal>(OnReset);
            _signalBus.Subscribe<TapToContinueSignal>(OnTapToContinue);
        }
  
        private void UnSubscribeEvents()
        {
            _signalBus.Unsubscribe<OnGameInitializeSignal>(OnPlayGameInitialize);
            _signalBus.Unsubscribe<LevelFinishedSignals>(OnLevelFinished);
            _signalBus.Unsubscribe<ResetGameSignal>(OnReset);
            _signalBus.Unsubscribe<TapToContinueSignal>(OnTapToContinue);
        }
        public void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void OpenStartPanel()
        {
            _openPanelSignal.uiPanelTypes = UIPanelTypes.Start;
            _signalBus.Fire(_openPanelSignal);
        }
        public void OnSeeLevels()
        {
            _openPanelSignal.uiPanelTypes = UIPanelTypes.LevelSelection;
            _signalBus.Fire(_openPanelSignal);
        }
        private void OnPlayGameInitialize()
        {
            _openPanelSignal.uiPanelTypes = UIPanelTypes.Game;
            _signalBus.Fire(_openPanelSignal);
        }
        
        private void OnLevelFinished(LevelFinishedSignals levelFinishedSignals)
        {
            _openPanelSignal.uiPanelTypes = UIPanelTypes.EndScreen;
            _signalBus.Fire(_openPanelSignal);
        }

        private void OnTapToContinue()
        {
            _openPanelSignal.uiPanelTypes = UIPanelTypes.LevelSelection;
            _signalBus.Fire(_openPanelSignal);
        }

        private void OnReset()
        {
        }
    }
}