using System.Collections.Generic;
using DG.Tweening;
using Scripts.Data.ValueObject;
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

        private GlassBaseBall _cacheBallObject;
        private int _boardHeight;
        private int _boardWidth;

        public GlassBaseBall[] SetBoard(int currentLevelNumber, LevelController currentLevelController)
        {
            _currentLevelNumber = currentLevelNumber;
            _currentLevelController = currentLevelController;
            _currentLevelData = _currentLevelController.levelData;

            int ballNumber = _currentLevelData.glassBallNumber;
            currentBoardGlassBalls = _currentLevelData.glassBaseBalls.ToArray();
            
            //SetValues
            SetBoardValues();

            return currentBoardGlassBalls;
        }

        private void SetBoardValues()
        {
            _boardWidth = _currentLevelData.levelData.Count;
            _boardHeight = _currentLevelData.levelData[0].boardColumn.Length;
            boardGlassBall2dArray = new GlassBaseBall[_boardWidth,_boardHeight];
            
            for (int i = 0; i < _boardWidth; i++)
            {
                for (int j = 0; j < _boardHeight; j++)
                {
                    _cacheBallObject = _currentLevelData.levelData[i].boardColumn[j];
                    
                    _cacheBallObject.Init(this,i,j);

                    boardGlassBall2dArray[i, j] = _cacheBallObject;
                }
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