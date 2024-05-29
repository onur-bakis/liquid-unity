using Scripts.Context.Signals;
using Scripts.Keys;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        [Inject] public SignalBus _signalBus;

        private InputDataParams _inputDataParams;
        private float _clickInterval = 0.4f;
        private float _lastClickTime;

        private TapSignal _tapSignal;
        protected void Awake()
        {
            _lastClickTime = int.MinValue;
            _tapSignal = new TapSignal();
        }

        public void Update()
        {
            if (Time.timeSinceLevelLoad - _lastClickTime < _clickInterval) return;
            
            if(Input.GetMouseButton(0))
            {
                _lastClickTime = Time.timeSinceLevelLoad;
                _inputDataParams.width = (int)Input.mousePosition.x;
                _inputDataParams.height = (int)Input.mousePosition.y;

                _tapSignal.inputDataParams = _inputDataParams;
                _signalBus.Fire(_tapSignal);
            }
        }
    }
}