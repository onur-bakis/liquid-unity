using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Views.Common
{
    [RequireComponent(typeof(Button))]
    public class BaseButton : MonoBehaviour
    {
        
        protected Button Button;

        public void Start()
        {
            Button = GetComponent<Button>();
            if (Button != null)
            {
                Button.onClick.AddListener(OnButtonClick);
            }
        }
        
        public virtual void OnButtonClick()
        {
        }

        public void OnDestroy()
        {
            if (Button != null)
            {
                Button.onClick.RemoveListener(OnButtonClick);
            }
        }
    }
}