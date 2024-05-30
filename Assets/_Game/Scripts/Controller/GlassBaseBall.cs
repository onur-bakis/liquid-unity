using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controller.GamePlay;
using UnityEngine;

namespace Scripts.Controller
{
    public class GlassBaseBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
            
        public bool isLocked;
        public bool onBoard;
        public bool removed;
        public bool inMove;
        
        public int yIndex;
        public int xIndex;

        private GameBoardController _gameBoardController;
        
        public void Init(GameBoardController gameBoardController,int boardXIndex,int boardYIndex)
        {
            _gameBoardController = gameBoardController;
            xIndex = boardXIndex;
            yIndex = boardYIndex;
            Reset();
            if (yIndex == 0)
            {
                isLocked = false;
            }
        }

        public void Reset()
        {
            isLocked = true;
            onBoard = true;
            removed = false;
            inMove = false;

            _rigidbody2D.gravityScale = 0f;
        }

        public void Release()
        {
            _rigidbody2D.gravityScale = 1f;
            _gameBoardController.UnlockBallAbove(this);
        }

        public void InitialPos()
        {
            
        }
    }
}
