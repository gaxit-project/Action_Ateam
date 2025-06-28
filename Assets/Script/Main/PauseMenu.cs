using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button titleButton;
    private GameInput input;
    private GameManager gameManager;

    private Button ResumeButton;
    private Button TitleButton;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        ResumeButton = GameObject.Find("Canvas/PausePanel/ResumeButton").GetComponent<Button>();
        TitleButton = GameObject.Find("Canvas/PausePanel/TitleButton").GetComponent<Button>();

        input = new GameInput();
        input.GamePlay.Pause.performed += OnPause;
    }
    void OnEnable()
    {
        input.GamePlay.Enable();
    }

    void OnDisable()
    {
        input.GamePlay.Disable();   
    }

    void Start()
    {
        ResumeButton.Select();
        pausePanel.SetActive(false);
        InitializeButtons();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gameManager.isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void InitializeButtons()
    {
        if(resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(Resume);
        }

        if(settingButton != null)
        {
            settingButton.onClick.RemoveAllListeners();
            settingButton.onClick.AddListener(GotoSetting);
        }

        if (titleButton != null)
        {
            titleButton.onClick.RemoveAllListeners();
            titleButton.onClick.AddListener(GoToTitle);
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        gameManager.isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameManager.isPaused = false;
    }

    public void GotoSetting()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("Main", currentScene);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneChangeManager.Instance.SceneChange("Setting");
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("Title");
    }

    public bool ReturnIsPaused()
    {
        return gameManager.isPaused;
    }

    void OnDestroy()
    {
        input.GamePlay.Pause.performed -= OnPause;
    }
}
