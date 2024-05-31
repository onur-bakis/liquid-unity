using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controller.GamePlay;
using TMPro;
using UnityEngine;

namespace Scripts.Controller
{
    public class GlassBaseBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private MeshRenderer _fakeLiquidRenderer;

        [SerializeField] private List<Material> ballMaterials;
            
        public bool textAlwaysLooksUp;
        
        public bool isLocked;
        public bool onBoard;
        public bool removed;
        public bool inMove;
        public bool inMerge;
        
        public int yIndex;
        public int xIndex;
        public int ballValue { private set;get;}

        private GameBoardController _gameBoardController;
        
        public void Init(GameBoardController gameBoardController
            ,int boardXIndex
            ,int boardYIndex)
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
            ballValue = 1;
            UpdateVisuals();
        }

        public void ChangeCollisionToMerge(bool inMerge)
        {
            _collider2D.enabled = !inMerge;
            _rigidbody2D.gravityScale = inMerge?0f:1f;
        }

        public void Release()
        {
            inMove = true;
            _rigidbody2D.gravityScale = 1f;
            _gameBoardController.UnlockBallAbove(this);
        }
        public void Update()
        {
            if (inMove)
            {
                if (textAlwaysLooksUp)
                {
                    _textMeshPro.gameObject.transform.up = Vector3.up;
                }
            }
        }

        public void IncreaseBallValue(int value)
        {
            ballValue += value;
            UpdateVisuals();
        }
        public void SetBallValue(int ballValue)
        {
            if (ballValue < 1)
            {
                ballValue = 1;
            }

            this.ballValue = ballValue;
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            _textMeshPro.text = ballValue.ToString();

            int materialIndex = ballValue-1;
            
            if (materialIndex >= ballMaterials.Count)
            {
                materialIndex = (ballValue - 1) % ballMaterials.Count;
            }
            
            _fakeLiquidRenderer.material = ballMaterials[materialIndex];
        }
    }
}
