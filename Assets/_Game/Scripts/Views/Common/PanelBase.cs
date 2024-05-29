using DG.Tweening;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Views.Common
{
    public class PanelBase : MonoBehaviour
    {
        public UIPanelTypes panelType;
        public virtual void Init()
        {
        }
        public virtual void Show()
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(Screen.width*1.5f, Screen.height/2f, 0);
            transform.DOMove(new Vector3(Screen.width/2f,Screen.height/2f,0f),1f).OnComplete(OnShowComplete);
        }
        public virtual void HidePanel()
        {
            transform
                .DOMove(new Vector3(-Screen.width*1.5f, Screen.height/2f, 0),1f)
                .OnComplete(OnHideFinish);
        }
        
        public virtual void OnShowComplete()
        {
            
        }

        public virtual void OnHideFinish()
        {
            gameObject.SetActive(false);
        }
    }
}