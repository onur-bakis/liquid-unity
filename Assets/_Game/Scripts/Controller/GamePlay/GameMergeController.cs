using System.Collections.Generic;
using DG.Tweening;
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

        public bool gameOnMerge;
        public float animationTime;
        public GameMergeController(GamePlayManager gamePlayManager)
        {
            _gamePlayManager = gamePlayManager;
            animationTime = 0.6f;
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
                    if (CheckMergeForBall())
                    {
                        //Returns checking process and continues for merge animation  
                        return;
                    }
                }
            }
        }

        private bool CheckMergeForBall()
        {
            _glassBaseBallsMergeList = new List<GlassBaseBall>();

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
                    //Passing over checking ball itself
                    continue;
                }
                

                if (distance < 1.05f)
                {
                    _glassBaseBallsMergeList.Add(glassBaseBall);
                    if (_glassBaseBallsMergeList.Count == 2)
                    {
                        MergeGlassBalls();
                        //Returns checking process and continues for merge animation  
                        return true;
                    }
                }
                
            }

            return false;
        }

        public void MergeGlassBalls()
        {
            gameOnMerge = true;
            _glassBaseBallToCheckMerge.inMerge = true;
            foreach (var mergeBalls in _glassBaseBallsMergeList)
            {
                mergeBalls.inMerge = true;
            }
            MergeAnimation();
        }

        public void MergeAnimation()
        {
            Transform mainBall = _glassBaseBallToCheckMerge.transform;
            mainBall.DOScale(1.3f * Vector3.one, animationTime / 2f)
                .OnComplete(ChangeValuesOfMainBall);
            mainBall.DOScale(Vector3.one, animationTime / 2f).SetDelay(animationTime / 2f)
                .OnComplete(MergeFinished);;


            foreach (var glassBaseBall in _glassBaseBallsMergeList)
            {
                Transform mergeBalls = glassBaseBall.transform;
                glassBaseBall.ChangeCollisionToMerge(true);
                mergeBalls.DOMove(mainBall.position, animationTime);
                mergeBalls.DOScale(Vector3.zero, animationTime);
            }
        }

        public void ChangeValuesOfMainBall()
        {
            _glassBaseBallToCheckMerge.IncreaseBallValue(1);
        }

        public void MergeFinished()
        {
            foreach (var mergeGlassBall in _glassBaseBallsMergeList)
            {
                mergeGlassBall.inMove = false;
                mergeGlassBall.removed = true;
                mergeGlassBall.gameObject.SetActive(false);
            }
            
            //Check if merged ball can cut rope
            _gamePlayManager.CheckRopeCanCut(_glassBaseBallToCheckMerge);
            
            _glassBaseBallToCheckMerge.inMerge = false;
            gameOnMerge = false;
        }

    }
}