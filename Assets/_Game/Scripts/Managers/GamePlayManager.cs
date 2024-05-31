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

        private OnLevelFinishedSignals _onLevelFinishedSignals;

        private List<ObiRopeController> _currentLevelRopeControllers;
        private int _currentLevelRopeIndex;
        
        public void Awake()
        {
            _onLevelFinishedSignals = new OnLevelFinishedSignals();
            _gameBoardClick = new GameBoardClick(this);
            _gameMergeController = new GameMergeController(this);
        }

        private void Start()
        {
            _signalBus.Subscribe<OnGameInitializeSignal>(OnGameInitialize);
            _signalBus.Subscribe<OnLevelFinishedSignals>(GameWinButton);
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
            if(!ingame || _gameMergeController.gameOnMerge)
                return;

            glassBaseBall.Release();
        }
        
        public void NoMoveRemain()
        {
            Invoke("GameLose",5f);
        }

        public void GameLose()
        {
            if(!ingame)
                return;
            UnSubscribeEvents();
            _gameBoardController.LevelFinished();

            _cacheLevelFinishParams.highScore = false;
            _cacheLevelFinishParams.win = false;
            _cacheLevelFinishParams.score = 0;

            _onLevelFinishedSignals.levelFinishParams = _cacheLevelFinishParams;
            LevelDataManager.levelFinishParams = _cacheLevelFinishParams;
            
            _signalBus.Fire(_onLevelFinishedSignals);
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
        public void GameWin()
        {
            if(!ingame)
                return;
            UnSubscribeEvents();
            _gameBoardController.LevelFinished();

            int newScore = 0;
            foreach (var obiRopeController in _currentLevelRopeControllers)
            {
                newScore += obiRopeController.ropeBreakValue*100;
            }

            newScore += (_currentBoardGlassBaseBalls.Length - _gameBoardClick.clickedBallCount) * 10;
            
            
            if (newScore > LevelDataManager.GetLevelScore(_currentLevelNumber))
            {
                LevelDataManager.SetLevelHighScore(_currentLevelNumber,newScore);  
                _cacheLevelFinishParams.highScore = true; 
            }
            else
            {
                _cacheLevelFinishParams.highScore = false;
            }
            _cacheLevelFinishParams.score = newScore;
            _cacheLevelFinishParams.win = true;
            
            
            int currentLock = LevelDataManager.GetNewUnlock();
            if (currentLock == _currentLevelNumber || currentLock == -1)
            {
                LevelDataManager.NewUnLock(_currentLevelNumber+1);
            }

            _onLevelFinishedSignals.levelFinishParams = _cacheLevelFinishParams;
            LevelDataManager.levelFinishParams = _cacheLevelFinishParams;
            _signalBus.Fire(_onLevelFinishedSignals);
        }

        public void GameWinButton()
        {  
            if(!ingame)
                return;
            
            UnSubscribeEvents();
            _gameBoardController.LevelFinished();
            if (100 > LevelDataManager.GetLevelScore(_currentLevelNumber))
            {
                LevelDataManager.SetLevelHighScore(_currentLevelNumber,100);  
                _cacheLevelFinishParams.highScore = true; 
            }
            
            int currentLock = LevelDataManager.GetNewUnlock();
            if (currentLock == _currentLevelNumber || currentLock == -1)
            {
                LevelDataManager.NewUnLock(_currentLevelNumber+1);
            }
        }
    }
}