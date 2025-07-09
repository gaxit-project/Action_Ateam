using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{
    [SerializeField] private LoadingUIController _loadingUI;
    private AsyncOperation _asyncOperation;
    private bool _isLoadComplete = false;

    private ButtonHandler buttonHandler;

    private void Start()
    {
        if (_loadingUI == null)
        {
            _loadingUI = GetComponent<LoadingUIController>();
        }
        _isLoadComplete = false;
    }
    private void Update()
    {
        if (_isLoadComplete && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return)))
        {
            ActiveScene();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_loadingUI == null)
        {
            _loadingUI = FindFirstObjectByType<LoadingUIController>();
        }
           
    }

    /// <summary>
    /// Sceneを非同期で読み込む際のロード画面
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void LoadNextScene(string name)
    {
        Debug.Log("LoadNextSceneが呼び出された");
        if (_loadingUI != null)
        {
            _loadingUI.ShowLoading();
            Debug.Log("Canvasが表示された");
            _loadingUI.ShowLoadingSlider();
            Debug.Log("Sliderが表示された");
            _loadingUI.HidePushButtonText();
            Debug.Log("Textが表示された");
        }
        _isLoadComplete = false;

        //コルーチン開始
        StartCoroutine(LoadSceneAsync(name));
    }

    public void ResetScene(string name)
    {
        //コルーチン開始
        StartCoroutine(ResetSceneAsync(name));
    }

    /// <summary>
    /// ボタンが押されたときにシーンを切り替える
    /// </summary>
    public void ActiveScene()
    {
        if(_asyncOperation != null)
        {
            _asyncOperation.allowSceneActivation = true;
            _isLoadComplete = false;
        }
    }

    /// <summary>
    /// シーンを同期で読み込む
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void SceneChange(string name)
    {
        Debug.Log("SceneChangeが呼び出された");
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// コルーチンでシーンを非同期で読み込む
    /// </summary>
    /// <param name="name">シーンの名前</param>
    /// <returns>ロード終了までnullを返す</returns>
    IEnumerator LoadSceneAsync(string name)
    {
        _asyncOperation = SceneManager.LoadSceneAsync(name);
        _asyncOperation.allowSceneActivation = false;

        while (!_asyncOperation.isDone && _asyncOperation.progress < 0.9f)
        {
            if (_loadingUI != null)
            {
                _loadingUI.UpdateProgress(_asyncOperation.progress);
            }
            yield return null;
        }

        if (_loadingUI != null)
        {
            //_loadingUI.HideLoadingSlider();
            _loadingUI.ShowPushButtonText();
        }
        _isLoadComplete = true;
    }

    IEnumerator ResetSceneAsync(string name)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
