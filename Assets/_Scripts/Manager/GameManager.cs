
using UnityEngine;

using TMPro;
using StateMachine;
using Manager;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class GameManager : SingleTon<GameManager>
{
    public int CurrentLevel { get; private set; } = 1;
    private IStateMachine stateMachine;

    public bool inGame = true;
    public bool isPause = false;
    public bool isEndGame = false;
    [field: SerializeField] public TMP_Dropdown FPSOption {get; private set;}
    // Start is called before the first frame update
    void Start()   {

    }
    protected override void Init()
    {
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1;

    }
    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0.0001f;
    }
    public void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        PauseGame();
    }
    public void RestartGame()
    {
        DOTween.KillAll();
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
public class SingleTon<T>: MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        Init();
    }
    protected virtual void Init()
    {}
}
public class PersistantSingleTon<T>: MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(_instance);
            Init();
        }
    }
    protected virtual void Init()
    {}
}
