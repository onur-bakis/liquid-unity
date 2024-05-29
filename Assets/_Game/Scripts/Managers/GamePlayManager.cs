using System;
using System.Threading.Tasks;
using Scripts.Controller.GamePlay;
using Scripts.Data.ValueObject;
using Scripts.Views;
using Scripts.Context.Signals;
using Scripts.Controller;
using Scripts.Keys;
using Scripts.Signals;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Scripts.Managers
{
    public class GamePlayManager:MonoBehaviour
    {
        [SerializeField] private GameBoardController _gameBoardController;
        [Inject] public SignalBus _signalBus;

        private GameBoardClick _gameBoardClick;
        
        private int _currentLevelNumber;
        private LevelData _currentLevelData;
  
        private LevelFinishParams _cacheLevelFinishParams;

        public int remainingBoardGlassBalls = 0;
        public GlassBaseBall[] _currentBoardGlassBaseBalls;

        public void Awake()
        {
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
            _currentLevelData = await LevelDataManager.GetLevelData(_currentLevelNumber);
            _signalBus.Fire<OnStartGameSignal>();
        }
        private void OnPlayGame()
        {
            _currentLevelNumber = LevelDataManager.currentLevelNumber;
            _currentLevelData = LevelDataManager.currentLevelData;

            remainingBoardGlassBalls = _currentLevelData.glassBallNumber;
            _currentBoardGlassBaseBalls = _gameBoardController.SetBoard(_currentLevelNumber,LevelDataManager.currentGameObject,_currentLevelData);
            _gameBoardClick.SetBoardValues(_currentBoardGlassBaseBalls);
            

            SubscribeEvents();
        }

        private bool ingame;
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
            glassBaseBall.Release();
        }

    }
}