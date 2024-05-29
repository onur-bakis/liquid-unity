using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Context.Signals;
using Scripts.Enums;
using Scripts.Views.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Scripts.Controller.UI
{
    public class UIPanelController : MonoBehaviour
    {
        private SignalBus _signalBus;
        private DiContainer _diContainer;

        [SerializeField] private List<GameObject> layers = new List<GameObject>();

        private Dictionary<UIPanelTypes, PanelBase> loadedLayers;

        private PanelBase _currentActivePanel;
        
        private PanelBase _cachePanelBase;

        [Inject]
        public void Construct(SignalBus signalBus,DiContainer diContainer)
        {
            _signalBus = signalBus;
            _diContainer = diContainer;
        }
        
        public void Awake()
        {
            loadedLayers = new Dictionary<UIPanelTypes, PanelBase>();

            LoadPanels();
        }
        

        public async void LoadPanels()
        {
            for (int i = 1; i < 4; i++)
            {
                Task<PanelBase> taskGetPanel = GetPanel((UIPanelTypes)i, i);
                await taskGetPanel;
                taskGetPanel.Result.gameObject.SetActive(false);
            }
        }
        
        public void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _signalBus.Subscribe<OpenPanelSignal>(OnOpenPanel);
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<OpenPanelSignal>(OnOpenPanel);
        }

        public void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void OnOpenPanel(OpenPanelSignal openPanelSignal)
        {
            UIPanelTypes panel = openPanelSignal.uiPanelTypes;
            OpenPanel(panel, (int)panel);
        }

        public bool IsPanelLoaded(UIPanelTypes panel)
        {
            return loadedLayers.ContainsKey(panel);
        }

        public PanelBase GetLoadedPanel(UIPanelTypes panel)
        {
            return loadedLayers[panel];
        }

        public async Task<PanelBase> GetPanel(UIPanelTypes panel,int layerValue)
        {
            if (loadedLayers.ContainsKey(panel))
            {
                return loadedLayers[panel];
            }

            AsyncOperationHandle<GameObject> aOHpanelBase = Addressables.LoadAssetAsync<GameObject>($"Panels/{panel}Panel.prefab");
            await aOHpanelBase.Task;
            // Debug.Log(aOHpanelBase.IsDone+"test"+$"Panels/{panel}Panel");
            // Debug.Log(aOHpanelBase.IsDone+"test"+aOHpanelBase.DebugName);
            
            if (loadedLayers.ContainsKey(panel))
            {
                return loadedLayers[panel];
            }
            
            _cachePanelBase = _diContainer.InstantiatePrefab(aOHpanelBase.Result, layers[layerValue].transform).GetComponent<PanelBase>();
            // _cachePanelBase.transform.parent = layers[layerValue].transform;
            
            //_cachePanelBase = Instantiate(aOHpanelBase.Result, layers[layerValue].transform).GetComponent<PanelBase>();
            
            _cachePanelBase.Init();
            
            loadedLayers.Add(panel,_cachePanelBase);
            return _cachePanelBase;
        }

        private async void OpenPanel(UIPanelTypes panel,int layer)
        {
            if (_currentActivePanel != null)
            {
                _currentActivePanel.HidePanel();
            }

            if (IsPanelLoaded(panel))
            {
                _currentActivePanel = GetLoadedPanel(panel);
            }
            else
            {
                Task<PanelBase> taskPanel = GetPanel(panel, layer);
                await taskPanel;
                _currentActivePanel = taskPanel.Result;
            }
            _currentActivePanel.Show();
        }
        
        private void OnCloseAllPanel()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.transform.childCount; i++)
                {
                    Destroy(layer.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}