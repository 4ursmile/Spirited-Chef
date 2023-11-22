using System.Collections;
using System.Collections.Generic;
using Architecture;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winlosePanel : BaseUIPanel
{
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private string _winText;
    [SerializeField] private string _loseText;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private InGameUIBridgeSO _inGameUIBridgeSO;
    public override void Init(OverlayUIManager manager)
    {
        base.Init(manager);
        _inGameUIBridgeSO.SetWinLosePanel(this);
    }
    public void SetWin()
    {
        base.EnableUIPanel();
        _textMeshProUGUI.text = _winText;
        _soundManager.Play(_winSound);
        GameManager.Instance.PauseGame();
    }
    public void SetLose()
    {
        base.EnableUIPanel();
        _textMeshProUGUI.text = _loseText;
        _soundManager.Play(_loseSound);
        GameManager.Instance.PauseGame();
    }
    public void ReplayButton()
    {
        GameManager.Instance.RestartGame();
    }
}
