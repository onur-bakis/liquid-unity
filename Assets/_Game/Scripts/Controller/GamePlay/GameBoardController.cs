using System.Collections.Generic;
using DG.Tweening;
using Scripts.Data.ValueObject;
using Scripts.Managers;
using Scripts.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardController : MonoBehaviour
    {
        [SerializeField] private GlassBaseBall _glassBasePrefab;
        [SerializeField] private Transform _boardHolder;
        
        public GlassBaseBall[] currentBoardGlassBalls;
        public GlassBaseBall[,] boardGlassBall2dArray;
        
        private int[] _currentBoardParentCount;
        
        private int _currentLevelNumber;
        private LevelData _currentLevelData;
        private LevelController _currentLevelController;
        private GamePlayManager _gamePlayManager;

        private GlassBaseBall _cacheBallObject;
        private int _boardHeight;
        private int _boardWidth;

        private int minBallValue;
        private int maxBallValue;

        public GlassBaseBall[] SetBoard(int currentLevelNumber, LevelController currentLevelController,int maxValue)
        {
            _currentLevelNumber = currentLevelNumber;
            _currentLevelController = currentLevelController;
            _currentLevelData = _currentLevelController.levelData;

            maxBallValue = maxValue;
            minBallValue = maxValue - 5;

            if (minBallValue <= 0)
            {
                minBallValue = 1;
            }
            
            //Set board values for glass balls on board
            SetBoardValues();
            
            //Set glassBall ballValues
            SetBoardGlassBallValues();

            return currentBoardGlassBalls;
        }


        private void SetBoardValues()
        {
            _boardWidth = _currentLevelData.levelData.Count;
            _boardHeight = _currentLevelData.levelData[0].boardColumn.Length;
            boardGlassBall2dArray = new GlassBaseBall[_boardWidth,_boardHeight];
            List<GlassBaseBall> allBallList = new List<GlassBaseBall>();
            
            for (int i = 0; i < _boardWidth; i++)
            {
                for (int j = 0; j < _boardHeight; j++)
                {
                    _cacheBallObject = _currentLevelData.levelData[i].boardColumn[j];
                    
                    _cacheBallObject.Init(this, i, j);
                    
                    boardGlassBall2dArray[i, j] = _cacheBallObject;
                    
                    allBallList.Add(_cacheBallObject);
                }
            }

            currentBoardGlassBalls = allBallList.ToArray();
        }
        
        private void SetBoardGlassBallValues()
        {
            List<int> newBallValues = new List<int>();
            
            for (int i = 0; i < currentBoardGlassBalls.Length; i++)
            {
                if (i< currentBoardGlassBalls.Length / 3f + 1)
                {
                    newBallValues.Add(maxBallValue-1);
                }
                else if (i < (currentBoardGlassBalls.Length / 3f) * 2f )
                {
                    newBallValues.Add(maxBallValue-2);
                }
                else if (i<currentBoardGlassBalls.Length-3)
                {
                    newBallValues.Add(maxBallValue-3);
                }
                else
                {
                    newBallValues.Add(maxBallValue-4);
                }
            }
            //Set ball values on the board
            AssignBoardGlassBallValues(newBallValues);
        }
        
        private void AssignBoardGlassBallValues(List<int> ballValues)
        {
            foreach (var boardGlassBall in currentBoardGlassBalls)
            {
                int newBallValue = ballValues[Random.Range(0, ballValues.Count)];
                ballValues.Remove(newBallValue);

                boardGlassBall.SetBallValue(newBallValue);
            }
        }


        public void UnlockBallAbove(GlassBaseBall glassBaseBall)
        {
            if(glassBaseBall.yIndex+1 == _boardHeight)
                return;

            boardGlassBall2dArray[glassBaseBall.xIndex, glassBaseBall.yIndex + 1].isLocked = false;
        }
        
        public void LevelFinished()
        {
            Destroy(_currentLevelController.gameObject);
        }
    }
}