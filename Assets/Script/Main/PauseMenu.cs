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

    private bool isPaused = false;

    private void Awake()
    {
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
        pausePanel.SetActive(false);
        InitializeButtons();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isPaused)
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
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GotoSetting()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("PreviousScene", currentScene);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Setting");
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public bool ReturnIsPaused()
    {
        return isPaused;
    }

    void OnDestroy()
    {
        input.GamePlay.Pause.performed -= OnPause;
    }
}
