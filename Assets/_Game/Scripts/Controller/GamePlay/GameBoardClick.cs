using Scripts.Context.Signals;
using Scripts.Managers;
using Scripts.Views;
using Scripts.Keys;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameBoardClick
    {

        private GamePlayManager _gamePlayManager;
        private GlassBaseBall[] _glassBaseBalls;
        
        private Vector3 _cacheCameraRay;
        private Vector3 _cacheCameraRayResults;
        private float cameraDistance = 50f;
        
        public GameBoardClick(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
            _cacheCameraRay = Vector3.zero;
            _cacheCameraRay.z = cameraDistance;
        }

        public void SetBoardValues(GlassBaseBall[] glassBaseBalls)
        {
            _glassBaseBalls = glassBaseBalls;
        }

        public void OnInputTaken(TapSignal tapSignal)
        {
            InputDataParams inputDataParams = tapSignal.inputDataParams;
            _cacheCameraRay.x = inputDataParams.width;
            _cacheCameraRay.y = inputDataParams.height;
            _cacheCameraRayResults = Camera.main.ScreenToWorldPoint(_cacheCameraRay);
            
            foreach (var glassBaseBall in _glassBaseBalls)
            {
                if (glassBaseBall.isLocked || !glassBaseBall.onBoard || glassBaseBall.removed || glassBaseBall.inMove)
                {
                    continue;
                }

                if (Vector2.Distance(_cacheCameraRayResults, glassBaseBall.transform.position)<0.5f)
                {
                    _gamePlayManager.GlassBallClicked(glassBaseBall);
                    break;
                }
            }
            
        }

    }
}