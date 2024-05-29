using System.Threading.Tasks;
using Scripts.Managers;
using Scripts.Views;
using Scripts.Context.Signals;
using Scripts.Data.ValueObject;
using Scripts.Views.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Controller.UI
{
    public class LevelSelectionPanel : PanelBase
    {
        [SerializeField] private LevelSelectionButton _levelSelectButtonPrefab;
        [SerializeField] private RectTransform LevelSelectionButtonHolder;
        [SerializeField] private ScrollRect _scrollRect;
        
        private LevelSelectionButton[] _levelSelectionButtons;

        [Inject] public UIManager UIManager;
        [Inject] public SignalBus _signalBus;
        [Inject] public DiContainer _diContainer;

        private LevelSelectionButton _currentUnLock;
        private bool _showAnim;
        private Rect _rect;
        private int levelNumber = 20;
        private LevelData[] _levelData;

        private OnGameInitializeSignal _playGameInitializeSignal;
        public override void Init()
        {
            base.Init();
            _playGameInitializeSignal = new OnGameInitializeSignal();
            _levelData = new LevelData[levelNumber];
            _levelSelectionButtons = new LevelSelectionButton[levelNumber];
            StartGettingData();
        }

        private async void StartGettingData()
        {
            for (int i = 0; i < 20; i++)
            {
                //Debug.Log(_levelData[i].title);
                //Debug.Log("asffa");
                _levelData[i] = await LevelDataManager.GetLevelData(i);
                //Debug.Log(_levelData[i].title);
                //Debug.Log("asffa");
                _levelSelectionButtons[i] =
                    _diContainer.InstantiatePrefab(_levelSelectButtonPrefab.gameObject, LevelSelectionButtonHolder.transform).GetComponent<LevelSelectionButton>();
                //_levelSelectionButtons[i] = Instantiate(_levelSelectButtonPrefab, LevelSelectionButtonHolder);
                
                int highScore = LevelDataManager.GetLevelScore(i);
                int lastUnlock = LevelDataManager.GetNewUnlock();
                
                _levelSelectionButtons[i].Init(this,i,_levelData[i].title,highScore,lastUnlock,false);
                if (i == lastUnlock)
                {
                    _currentUnLock = _levelSelectionButtons[i];
                }
            }
        }

        public  override async void Show()
        {
            int lastUnlock = LevelDataManager.GetNewUnlock();
            
            base.Show();
            for (int i = 0; i < levelNumber; i++)
            {
                Task<LevelData> las = LevelDataManager.GetLevelData(i);
                await las;
                _levelData[i] = las.Result;
                int highScore = LevelDataManager.GetLevelScore(i);
                bool animInfo = false;
                
                if (i == lastUnlock && _currentUnLock != _levelSelectionButtons[i])
                {
                    animInfo = true;
                    _currentUnLock = _levelSelectionButtons[i];
                    _showAnim = true;
                }
                
                _levelSelectionButtons[i].Init(this,i,_levelData[i].title,highScore,lastUnlock,animInfo);
                
            }
            
            if (lastUnlock != -1)
            {
                _scrollRect.verticalScrollbar.value = 1 - (lastUnlock - 1f) / (levelNumber-3f);
            }

        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();
            if (_showAnim && _currentUnLock != null)
            {
                _currentUnLock.PlayAnim();
                _showAnim = false;
            }
        }
        

        public void GoLevel(int id)
        {
            _playGameInitializeSignal.levelNumber = (byte)id;
            _signalBus.Fire(_playGameInitializeSignal);
        }
    }
}