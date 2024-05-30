using Scripts.Controller.GamePlay;
using Scripts.Context.Signals;
using Scripts.Controller;
using Scripts.Keys;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class GamePlayManager:MonoBehaviour
    {
        [SerializeField] private GameBoardController _gameBoardController;
        [Inject] public SignalBus _signalBus;

        private GameBoardClick _gameBoardClick;
        
        private int _currentLevelNumber;
        private LevelController _currentLevelController;
  
        private LevelFinishParams _cacheLevelFinishParams;

        public GlassBaseBall[] _currentBoardGlassBaseBalls;
        private bool ingame;

        private LevelFinishedSignals _levelFinishedSignals;

        public void Awake()
        {
            _levelFinishedSignals = new LevelFinishedSignals();
            _gameBoardClick = new GameBoardClick(this);
        }

        private void Start()
        {
            _signalBus.Subscribe<OnGameInitializeSignal>(OnGameInitialize);
        }

        public void OnEnable()
        {
            _signalBus.Subscribe<OnStartGameSignal>(OnPlayGame);
        }
        
        public void OnDisable()
        {
            _signalBus.Unsubscribe<OnStartGameSignal>(OnPlayGame);
        }

        private void OnGameInitialize(OnGameInitializeSignal onGameInitializeSignal)
        {
            _currentLevelNumber = onGameInitializeSignal.levelNumber;
            LevelDataManager.SetLevelNumber(_currentLevelNumber);
            LoadDataAndStartGame();
        }

        private async void LoadDataAndStartGame()
        {
            _currentLevelController = await LevelDataManager.GetLevelData(_currentLevelNumber);
            _signalBus.Fire<OnStartGameSignal>();
        }
        private void OnPlayGame()
        {
            
            _currentBoardGlassBaseBalls = _gameBoardController.SetBoard(_currentLevelNumber,_currentLevelController);
            _gameBoardClick.SetBoardValues(_currentBoardGlassBaseBalls);
            

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if(ingame)
                return;
            ingame = true;
            _signalBus.Subscribe<TapSignal>(_gameBoardClick.OnInputTaken);
        }

        private void UnSubscribeEvents()
        {
            ingame = false;
            _signalBus.Unsubscribe<TapSignal>(_gameBoardClick.OnInputTaken);
        }

        
        public void GlassBallClicked(GlassBaseBall glassBaseBall)
        {
            if(!ingame)
                return;

            glassBaseBall.Release();
        }

        public void GameEnd()
        {
            UnSubscribeEvents();
            _gameBoardController.LevelFinished();

            _levelFinishedSignals.levelFinishParams = new LevelFinishParams { highScore = true, score = 25 };
            _signalBus.Fire(_levelFinishedSignals);
        }
    }
}