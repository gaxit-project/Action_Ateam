using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _SENumber;

    [Header("BGMを停止するかどうか")]
    [SerializeField] private bool _stopBGM;

    public bool _isStartButtonClicked;
    
    private PauseMenu pauseMenu;
    private GameManager gameManager;
    void Start()
    {
        pauseMenu = Object.FindFirstObjectByType<PauseMenu>();
    }

    private void Update()
    {
        if(gameManager == null)
        {
            //Debug.LogWarning("GameManagerNULL");
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }
    /// <summary>
    /// シーンを同期で読み込むボタンが押されたとき
    /// </summary>
    public void ButtonOnClicked()
    {
        if (_stopBGM)
        {
            AudioManager.Instance.StopBGM();
        }

        AudioManager.Instance.PlaySound(_SENumber);
        AudioManager.Instance._audioSourceBGM.loop = true;
        SceneChangeManager.Instance.SceneChange(_sceneName);
        
        
    }

    /// <summary>
    /// シーンを非同期で読み込むボタンが押されたとき
    /// </summary>
    public void ButtonOnClickedAsync()
    {
        if (_stopBGM)
        {
            AudioManager.Instance.StopBGM();
        }

        GameManager.Instance.ResetGame();
        AudioManager.Instance.PlaySound(_SENumber);
        AudioManager.Instance._audioSourceBGM.loop = true;
        SceneChangeManager.Instance.LoadNextScene(_sceneName);
    }

    /// <summary>
    /// ゲームを終了するとき
    /// </summary>
    public void ApplicationExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ButtonSelected()
    {
        AudioManager.Instance.PlaySound(0);
    }
}
