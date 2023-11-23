using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Manager;
using ObjectS;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace Architecture
{
    [CreateAssetMenu(fileName = "InGameUIBridgeSO", menuName = "ScriptableObjects/UI/InGameUIBridgeSO", order = 1)]
    public partial class InGameUIBridgeSO : ScriptableObject
    {
        [Header("Selected Unit UI")]
        private UIFollow _selectedCircle;

        [SerializeField] private Vector3 _selectedCircleOffset;
        [SerializeField] private Vector3 _selectedCircleScale = Vector3.one;
        [Header("Innfor UI")]
        private InforUI _inforUI;
        [SerializeField] private Vector3 _inforUIOffset;
        [SerializeField] private Vector3 _inforUIScale = Vector3.one;
        public Vector3 InforUIScale => _inforUIScale;

        [Header("Warning Text")]
        [SerializeField] private WarningText _warningTextPrefab;
        [SerializeField] private int _warningTextPoolSize = 5;
        private LinkedListPool<WarningText> _warningTextPool;
        [Header("Panel Tab UI")]
        [SerializeField] private TabGroup _tabGroup;
        [Header("Counter")]
        [SerializeField] private CounterUI _counterUI;
        [SerializeField] private int _counterPoolSize = 5;
        private LinkedListPool<CounterUI> _counterPool;
        private InGameUIManager _inGameUIManager;
        [Header("Particel")]
        [SerializeField] private List<ParticelPoolSO> _particelPoolSOs;
        [SerializeField] private SoundManagerSO _soundManagerSO;
        [Header("Intergridient")]
        [SerializeField] private IntergridientUI _intergridientUI;
        private LinkedListPool<IntergridientUI> _intergridientUIPool;
        public Transform transform => _inGameUIManager.transform;
        public void Init(InGameUIManager manager)
        {
            _inGameUIManager = manager;
            InitPool();
            foreach (var item in _particelPoolSOs)
            {
                item.Init(_soundManagerSO);
            }
        }
        public void SetTabGroup(BaseUIPanel tabGroup)
        {
            _tabGroup = tabGroup as TabGroup;
        }
        public void UseTabGroup(ListTabConfigSO listTabConfigSO, InteractiveObject interactiveObject)
        {
            _tabGroup.InitTab(listTabConfigSO, interactiveObject);
        }
        public void SetSelectedImage(UIFollow selectedCircle)
        {

            _selectedCircle = selectedCircle;
            _selectedCircle.gameObject.SetActive(false);
            _selectedCircle.transform.localScale = _selectedCircleScale;
        }
        public void SetInforUI(InforUI inforUI)
        {
            _inforUI = inforUI;
            _inforUI.gameObject.SetActive(false);
            _inforUI.transform.localScale = _inforUIScale;
        }
        public void SetFollowTarget(Transform target)
        {
            _selectedCircle.SetTarget(target, _selectedCircleOffset);
        }
        public void UnSetTarget()
        {
            _selectedCircle.UnSetTarget();
        }
        public void SetInforUITarget(Transform target, string name, string description)
        {
            _inforUI.SetInfor(name, description);
            _inforUI.SetTarget(target, _inforUIOffset);
        }
        public void UnSetInforUITarget()
        {
            _inforUI.UnSetTarget();
        }
        public void SetWarningText(string text, Vector3 position, bool dontUseDic = false)
        {
            WarningText warningText = _warningTextPool.Get();
            if (warningText == null)
            {
                warningText = Instantiate(_warningTextPrefab);
                warningText.SetPosition(position);
                warningText.Set(text, (warningText) => {
                    Destroy(warningText.gameObject);
                }, dontUseDic);
                return;
            }
            warningText.SetPosition(position);
            warningText.Set(text, (warningText) => {
                _warningTextPool.Release(warningText);
            }, dontUseDic);
        } 
        public void SetCounter(float duration, Vector3 position, bool flip = false)
        {
            var counterUI = _counterPool.Get();
            if (counterUI == null)
            {
                counterUI = Instantiate(_counterUI, _inGameUIManager.transform);
                counterUI.SetCounter(duration, position, flip, () => {
                    Destroy(counterUI.gameObject);
                });
                return;
            }
            counterUI.SetCounter(duration, position, flip, () => {
                _counterPool.Release(counterUI);
            });
        } 
        public IntergridientUI GetIntergridientUI()
        {
            var intergridientUI = _intergridientUIPool.Get();
            if (intergridientUI == null)
            {
                intergridientUI = Instantiate(_intergridientUI, _inGameUIManager.transform);
                return intergridientUI;
            }
            return intergridientUI;
        }
        public void ReleaseIntergridientUI(IntergridientUI intergridientUI)
        {
            _intergridientUIPool.Release(intergridientUI);
        }
        public void SetPartical(int index, Vector3 position, float duration)
        {
            if (index >= _particelPoolSOs.Count)
            {
                return;
            }
            var particel = _particelPoolSOs[index].Get;
            particel.SetPosition(position);
            particel.Play(duration);
        }
        [SerializeField] private EmojiBehaviour _emojiBehaviourPrefab;
        [SerializeField] private int _emojiPoolSize = 5;
        private LinkedListPool<EmojiBehaviour> _emojiPool;
        public void InitPool()
        {
            _warningTextPool = new LinkedListPool<WarningText>(
                () => {
                    WarningText warningText = Instantiate(_warningTextPrefab);
                    warningText.gameObject.SetActive(false);
                    return warningText;
                },
                (warningText) => {
                    warningText.gameObject.SetActive(true);
                },
                (warningText) => {
                    warningText.gameObject.SetActive(false);
                },
                (warningText) => {
                    Destroy(warningText.gameObject);
                },
                false,
                _warningTextPoolSize,
                _warningTextPoolSize
            );
            _counterPool = new LinkedListPool<CounterUI>(
                () => {
                    CounterUI counterUI = Instantiate(_counterUI, _inGameUIManager.transform);
                    counterUI.gameObject.SetActive(false);
                    return counterUI;
                },
                (counterUI) => {
                    counterUI.gameObject.SetActive(true);
                },
                (counterUI) => {
                    counterUI.gameObject.SetActive(false);
                },
                (counterUI) => {
                    Destroy(counterUI.gameObject);
                },
                false,
                _counterPoolSize,
                _counterPoolSize
            );
            _intergridientUIPool = new LinkedListPool<IntergridientUI>(
                () => {
                    IntergridientUI intergridientUI = Instantiate(_intergridientUI, _inGameUIManager.transform);
                    intergridientUI.gameObject.SetActive(false);
                    return intergridientUI;
                },
                (intergridientUI) => {
                    intergridientUI.gameObject.SetActive(true);
                },
                (intergridientUI) => {
                    intergridientUI.gameObject.SetActive(false);
                },
                (intergridientUI) => {
                    Destroy(intergridientUI.gameObject);
                },
                false,
                _counterPoolSize,
                _counterPoolSize
            );
            _emojiPool = new LinkedListPool<EmojiBehaviour>(
                () => {
                    EmojiBehaviour emojiBehaviour = Instantiate(_emojiBehaviourPrefab);
                    emojiBehaviour.gameObject.SetActive(false);
                    return emojiBehaviour;
                },
                (emojiBehaviour) => {
                    emojiBehaviour.gameObject.SetActive(true);
                },
                (emojiBehaviour) => {
                    emojiBehaviour.gameObject.SetActive(false);
                },
                (emojiBehaviour) => {
                    Destroy(emojiBehaviour.gameObject);
                },
                false,
                _emojiPoolSize,
                _emojiPoolSize
            );
        }
        public void OpenEmoji(string text, Vector3 position)
        {
            var emoji = _emojiPool.Get();
            emoji.SetSpriteByKey(text, position, () => {
                _emojiPool.Release(emoji);
            });
        }
        public void OpenEmoji(Sprite sprite, Vector3 position)
        {
            var emoji = _emojiPool.Get();
            emoji.SetSprite(sprite, position, () => {
                _emojiPool.Release(emoji);
            });
        }

    }
    public partial class InGameUIBridgeSO
    {
        
        private CardManager _cardManager;
        public void SetCardManager(CardManager cardManager)
        {
            _cardManager = cardManager;
        }
        public void OpenCardPanel()
        {
            _cardManager.EnableUIPanel();
        }
        private winlosePanel _winlosePanel;
        public void SetWinLosePanel(winlosePanel winlosePanel)
        {
            _winlosePanel = winlosePanel;
        }
        public void SetWin()
        {
            _winlosePanel.SetWin();
        }
        public void SetLose()
        {
            _winlosePanel.SetLose();
        }
        private OrderMessageUI _orderMessageUI;
        public void SetOrderMessageUI(OrderMessageUI orderMessageUI)
        {
            _orderMessageUI = orderMessageUI;
        }
        public void OpenOrderMessageUI(string Text, float waitingTime)
        {
            _orderMessageUI.SetText(Text, waitingTime);
        }
        public void CloseOrderMessageUI()
        {
            _orderMessageUI.Close();
        }
    }
}

