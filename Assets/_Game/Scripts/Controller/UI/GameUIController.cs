using System;
using System.Collections.Generic;
using Scripts.Signals;
using Scripts.Views.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controller
{
    public class GameUIController:PanelBase
    {
        [SerializeField] private TextMeshProUGUI _levelTitleTMPRUI;
        [SerializeField] private TextMeshProUGUI _topScoreTMPRUI;
        
        public override void Init()
        {
            base.Init();
        }

        public void SetStartValues(string title)
        {
            _levelTitleTMPRUI.text = title;
            OnScoreChange(0);
        }

        public override void Show()
        {
            base.Show();
        }

        public override void HidePanel()
        {
            base.HidePanel();
        }

        public override void OnHideFinish()
        {
            base.OnHideFinish();
        }
        
        public void OnScoreChange(OnScoreChangeSignal onScoreChangeSignal)
        {
            OnScoreChange(onScoreChangeSignal.score);
        }

        public void OnScoreChange(int score)
        {
            _topScoreTMPRUI.text = "Score: " + score;
        }

    }
}