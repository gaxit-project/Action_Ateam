using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{
    [SerializeField] private LoadingUIController _loadingUI;

    private void Start()
    {
        if (_loadingUI == null)
        {
            _loadingUI = GetComponent<LoadingUIController>();
        }
    }
    private void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange("Result");
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
        if (_loadingUI != null)
        {
            _loadingUI.ShowLoading();
        }

        //コルーチン開始
        StartCoroutine(LoadSceneAsync(name));
    }

    public void ResetScene(string name)
    {
        //コルーチン開始
        StartCoroutine(ResetSceneAsync(name));
    }

    /// <summary>
    /// シーンを同期で読み込む
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// コルーチンでシーンを非同期で読み込む
    /// </summary>
    /// <param name="name">シーンの名前</param>
    /// <returns>ロード終了までnullを返す</returns>
    IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        while (!async.isDone)
        {
            if (_loadingUI != null)
            {
                _loadingUI.UpdateProgress(async.progress);
            }
            yield return null;
        }
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
