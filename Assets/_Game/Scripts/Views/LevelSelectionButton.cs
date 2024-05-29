using Scripts.Controller.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Views
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private Button _onPlayButton;
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private GameObject _lockImageHolder;
        [SerializeField] private ParticleSystem _particleSystemStar;
        [SerializeField] private ParticleSystem _particleSystemCelebration;
        
        [SerializeField] private Image _buttonImage;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Image _lockImageBG;
        [SerializeField] private Image _lockImage;
        
        private LevelSelectionPanel _levelSelectionPanel;
        private int _id;
        private bool _isLocked =false;

        private Color _cacheColor;
        public void Init(LevelSelectionPanel levelSelectionPanel, int id, string name, int highScore,int lastUnlock,bool playAnim)
        {
            
            _levelSelectionPanel = levelSelectionPanel;
            _levelName.text = "Level "+(id+1)+" - "+name;
            _id = id;
            _isLocked = false;
            
            _cacheColor = new Color(1f,1f,1f,0f);
            
            if (highScore > 0)
            {
                _highScore.text = "High Score: "+highScore;
                _isLocked = false;
            }
            else if (id == 0)
            {
                _highScore.text = "No Score";
                _isLocked = false;
            }
            else if (id == lastUnlock)
            {
                _highScore.text = "No Score";
                _isLocked = false;
                if (playAnim)
                {
                    _buttonImage.color = _cacheColor;
                    _buttonText.color = _cacheColor;
                    return;
                }
            }
            else if (highScore == -1)
            {
                _highScore.text = "Locked Level";
                _isLocked = true;
            }
            
            _onPlayButton.gameObject.SetActive(!_isLocked);
            _lockImageHolder.SetActive(_isLocked);
        }

        public void PlayAnim()
        {
            _buttonImage.DOFade(1f, 1f).SetDelay(1f);
            _buttonText.DOFade(1f, 1f).SetDelay(1f);
            _lockImage.DOFade(0f, 1f);
            _lockImageBG.DOFade(0f, 1f).OnComplete(Unlock);
            
            
            // _onPlayButton.transform.localScale = new Vector3(1f,0f,1f);
            // _onPlayButton.transform.DOScale(Vector3.one, 1f);
            _particleSystemCelebration.Play();
        }

        public void Unlock()
        {
            _onPlayButton.gameObject.SetActive(!_isLocked);
            _lockImageHolder.SetActive(_isLocked);
            _particleSystemStar.Play();
        }

        public void OnEnable()
        {
            _onPlayButton.onClick.AddListener(Play);
        }

        public void OnDisable()
        {
            _onPlayButton.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            _levelSelectionPanel.GoLevel(_id);
        }
    }
}