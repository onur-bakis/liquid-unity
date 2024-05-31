using System.Collections.Generic;
using Scripts.Managers;
using UnityEngine;

namespace Scripts.Controller.GamePlay
{
    public class GameMergeController
    {
        private GamePlayManager _gamePlayManager;
        private List<GlassBaseBall> _glassBaseBalls;

        private GlassBaseBall _glassBaseBallToCheckMerge;
        private List<GlassBaseBall> _glassBaseBallsMergeList;

        private bool gameOnMerge;
        public GameMergeController(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
        }

        public void SetMergeValues(List<GlassBaseBall> glassBaseBalls)
        {
            _glassBaseBalls = glassBaseBalls;
        }

        public void CheckMerge()
        {
            if (gameOnMerge)
            {
                return;
            }
            
            foreach (var glassBaseBall in _glassBaseBalls)
            {
                if (glassBaseBall.inMove)
                {
                    _glassBaseBallToCheckMerge = glassBaseBall;
                    ChergeMergeForBall();
                }
            }
        }

        private void ChergeMergeForBall()
        {
            _glassBaseBallsMergeList = new List<GlassBaseBall>();
            _glassBaseBallsMergeList.Add(_glassBaseBallToCheckMerge);

            foreach (var glassBaseBall in _glassBaseBalls)
            {
                if (!glassBaseBall.inMove ||
                    _glassBaseBallToCheckMerge.ballValue != glassBaseBall.ballValue)
                {
                    continue;
                }
                
                float distance = Vector2.Distance(glassBaseBall.transform.position,
                    _glassBaseBallToCheckMerge.transform.position);
                
                if (glassBaseBall == _glassBaseBallToCheckMerge)
                {
                    //Pass itself
                    continue;
                }
                

                if (distance < 1.05f)
                {
                    _glassBaseBallsMergeList.Add(glassBaseBall);
                    if (_glassBaseBallsMergeList.Count == 3)
                    {
                        MergeGlassBalls(_glassBaseBallsMergeList);
                    }
                }
                
            }
        }

        public void MergeGlassBalls(List<GlassBaseBall> mergeGlassBalls)
        {
            gameOnMerge = true;
            
            GlassBaseBall mainBall = mergeGlassBalls[0];
            foreach (var mergeBalls in mergeGlassBalls)
            {
                mergeBalls.inMerge = true;
            }

            mainBall.ballValue++;
            mainBall.UpdateVisuals();
            mergeGlassBalls.Remove(mainBall);

            foreach (var mergeGlassBall in mergeGlassBalls)
            {
                mergeGlassBall.inMove = false;
                mergeGlassBall.removed = true;
                mergeGlassBall.gameObject.SetActive(false);
            }
            
            //Check if merged ball can cut rope
            _gamePlayManager.CheckRopeCanCut(mainBall);
            
            mainBall.inMerge = false;
            gameOnMerge = false;
        }

    }
}