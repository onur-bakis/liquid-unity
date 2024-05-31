using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Controller.GamePlay;
using Scripts.Context.Signals;
using Scripts.Controller;
using Scripts.Keys;
using Scripts.Models;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private GameBoardController _gameBoardController;
        [Inject] public SignalBus _signalBus;
        [Inject] public GameModel GameModel;

        private GameBoardClick _gameBoardClick;
        private GameMergeController _gameMergeController;
        
        private int _currentLevelNumber;
        private LevelController _currentLevelController;
  
        private LevelFinishParams _cacheLevelFinishParams;

        public GlassBaseBall[] _currentBoardGlassBaseBalls;
        private bool ingame;

        private LevelFinishedSignals _levelFinishedSignals;

        private List<ObiRopeController> _currentLevelRopeControllers;
        private int _currentLevelRopeIndex;
        
        public void Awake()
        {
            _levelFinishedSignals = new LevelFinishedSignals();
            _gameBoardClick = new GameBoardClick(this);
            _gameMergeController = new GameMergeController(this);
        }

        private void Start()
        {
            _signalBus.Subscribe<OnGameInitializeSignal>(OnGameInitialize);
        }

        private void Update()
        {
            if(!ingame)
                return;
            
            _gameMergeController.CheckMerge();
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
            
            _currentBoardGlassBaseBalls = _gameBoardController.SetBoard(_currentLevelNumber,_currentLevelController,_currentLevelNumber+3);
            _gameBoardClick.SetBoardValues(_currentBoardGlassBaseBalls);
            _gameMergeController.SetMergeValues(_currentBoardGlassBaseBalls.ToList());
            _currentLevelRopeControllers = _currentLevelController.levelData.obiRopeControllers;

            _currentLevelRopeControllers[0].SetBreakValue(_currentLevelNumber + 3);
            _currentLevelRopeControllers[1].SetBreakValue(_currentLevelNumber + 4);
            
            _currentLevelRopeIndex = 0;
            
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

        public void GameWin()
        {
            UnSubscribeEvents();
            _gameBoardController.LevelFinished();

            _levelFinishedSignals.levelFinishParams = new LevelFinishParams
                { highScore = true, win = true, score = 25 };
            GameModel.LevelFinishParams =  new LevelFinishParams
                { highScore = true, win = true, score = 25 };
            _signalBus.Fire(_levelFinishedSignals);
        }

        public void NoMoveRemain()
        {
            Invoke(nameof(GameLose),2f);
        }

        public void GameLose()
        {
            UnSubscribeEvents();
            _gameBoardController.LevelFinished();

            _levelFinishedSignals.levelFinishParams = new LevelFinishParams
                { highScore = false, win = false, score = 25 };
            GameModel.LevelFinishParams =  new LevelFinishParams
                { highScore = false, win = false, score = 25 };
            _signalBus.Fire(_levelFinishedSignals);
        }
        
        public void CheckRopeCanCut(GlassBaseBall mergedGlassBaseBall)
        {
            ObiRopeController currentController = _currentLevelRopeControllers[_currentLevelRopeIndex];
            if (currentController.ropeBreakValue == mergedGlassBaseBall.ballValue)
            {
                currentController.BreakRope(mergedGlassBaseBall.transform.position);
                _currentLevelRopeIndex++;

                if (_currentLevelRopeIndex == _currentLevelRopeControllers.Count)
                {
                    Invoke(nameof(GameWin), 1f);
                }
            }
            
        }
    }
}