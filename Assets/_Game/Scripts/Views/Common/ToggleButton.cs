using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Views.Common
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _iconOn, _iconOff;
        
        public void SetActive(bool active)
        {
            _image.sprite = active ? _iconOn : _iconOff;            
        }
    }
    
    
}