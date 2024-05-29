using Scripts.Signals;
using Scripts.Context.Signals;
using Scripts.Controller;
using Scripts.Data.ValueObject;
using Scripts.Keys;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [Inject] public SignalBus _signalBus;

        [SerializeField] private GameUIController _gameUIController;
        
        private LevelData _currentLevelData;
        
        public void Awake()
        {
            _currentLevelData = new LevelData();
            _signalBus.Subscribe<OnGameInitializeSignal>(SubscribeSignals);
            _signalBus.Subscribe<LevelFinishedSignals>(UnSubscribeSignals);
        }

        private void UnSubscribeSignals()
        {
            _signalBus.Unsubscribe<OnStartGameSignal>(OnGameStart);
            _signalBus.Unsubscribe<OnScoreChangeSignal>(_gameUIController.OnScoreChange);
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnStartGameSignal>(OnGameStart);
            _signalBus.Subscribe<OnScoreChangeSignal>(_gameUIController.OnScoreChange);
        }
        
        private void OnGameStart()
        {
            _currentLevelData = LevelDataManager.currentLevelData;
            _gameUIController.SetStartValues(_currentLevelData.title);
        }
        
        public void OnFinish()
        {
            LevelFinishParams lfp = new LevelFinishParams();
            lfp.score = 100;
            lfp.highScore = false;
            
            LevelFinishedSignals lfs = new LevelFinishedSignals();
            lfs._levelFinishParams = lfp;
            
            _signalBus.Fire(lfs);
        }
        
    }
}