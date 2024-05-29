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
        public Dictionary<char, Mesh> meshData;
        
        private int[] _currentBoardParentCount;
        
        private int _currentLevelNumber;
        private LevelData _currentLevelData;
        private LevelController _currentGameObject;

        private GlassBaseBall _cacheBallObject;

        private List<GlassBaseBall> simplePool = new List<GlassBaseBall>();

        protected void Awake()
        {
            meshData = new Dictionary<char, Mesh>();
        }

        public GlassBaseBall[] SetBoard(int currentLevelNumber, LevelController currentGameObject,
            LevelData currentLevelData)
        {
            _currentLevelNumber = currentLevelNumber;
            _currentLevelData = currentLevelData;
            _currentGameObject = currentGameObject;

            Instantiate(currentGameObject);
            int ballNumber = _currentLevelData.glassBallNumber;
            currentBoardGlassBalls = _currentLevelData.glassBaseBalls.ToArray();
            
            //SetValues

            return currentBoardGlassBalls;
        }

        public void LevelFinished()
        {
            int glassBallNumber = _currentLevelData.glassBallNumber;
            for (int i = 0; i < glassBallNumber; i++)
            {
                ReturnBoardGlassBall(currentBoardGlassBalls[i]);
            }
        }

        private int activePoolBalls;
        private GlassBaseBall _cacheBoardGlassBall;
        public GlassBaseBall GetBoardTils()
        {
            if (simplePool.Count == 0)
            {
                _cacheBoardGlassBall = Instantiate(_glassBasePrefab, _boardHolder);
                return _cacheBoardGlassBall;
            }

            _cacheBoardGlassBall = simplePool[simplePool.Count-1];
            _cacheBoardGlassBall.gameObject.SetActive(true);
            _cacheBoardGlassBall.Reset();
            simplePool.Remove(_cacheBoardGlassBall);
            return _cacheBoardGlassBall;
        }

        public void ReturnBoardGlassBall(GlassBaseBall boardGlassBaseBall)
        {
            boardGlassBaseBall.gameObject.SetActive(false);
            simplePool.Add(boardGlassBaseBall);
        }
    }
}