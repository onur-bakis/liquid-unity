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

        
        public void Awake()
        {
            _signalBus.Subscribe<OnGameInitializeSignal>(OnGameStart);
            _signalBus.Subscribe<OnGameInitializeSignal>(SubscribeSignals);
            _signalBus.Subscribe<OnLevelFinishedSignals>(UnSubscribeSignals);
        }

        private void UnSubscribeSignals()
        {
            _signalBus.Unsubscribe<OnScoreChangeSignal>(_gameUIController.OnScoreChange);
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnScoreChangeSignal>(_gameUIController.OnScoreChange);
        }

        private void OnGameStart(OnGameInitializeSignal onGameInitializeSignal)
        {
            _gameUIController.SetStartValues("Level "+ (onGameInitializeSignal.levelNumber + 1));
        }
        
        public void OnFinish()
        {
            LevelFinishParams lfp = new LevelFinishParams();
            lfp.score = 100;
            lfp.highScore = false;
            lfp.win = true;
            
            OnLevelFinishedSignals lfs = new OnLevelFinishedSignals();
            lfs.levelFinishParams = lfp;
            LevelDataManager.levelFinishParams = lfp;
            
            _signalBus.Fire(lfs);
        }
    }
}