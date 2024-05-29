using DG.Tweening;
using Scripts.Models;
using Scripts.Views.Common;
using TMPro;
using UnityEngine;
using Zenject;

public class EndPanelController : PanelBase
{
    [SerializeField] private TextMeshProUGUI highScore;
    
    [SerializeField] private TextMeshProUGUI tapToContinue;

    [Inject] private GameModel GameModel;

    private bool highScoreWin;
    private Color _cacheColor;
    public override void Init()
    {
        base.Init();
        _cacheColor = new Color(1f, 1f, 1f, 0f);
    }

    public override void Show()
    {
        base.Show();
        if (GameModel.HighScore)
        {
            highScore.gameObject.SetActive(true);
            highScoreWin = true;
        }
        else
        {
            highScore.gameObject.SetActive(false);
            highScoreWin = false;
        }

        tapToContinue.color = _cacheColor;
    }

    public override void OnShowComplete()
    {
        base.OnShowComplete();
        tapToContinue.DOFade(1f, 1f).SetDelay(1f);
    }

    public override void HidePanel()
    {
        base.HidePanel();

    }
}
