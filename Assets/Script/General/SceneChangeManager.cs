using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{
    [SerializeField] private LoadingUIController loadingUI;

    private void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange("Result");
        }
    }

    /// <summary>
    /// Sceneを非同期で読み込む際のロード画面
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void LoadNextScene(string name)
    {
        if (loadingUI != null)
        {
            loadingUI.ShowLoading();
        }

        //コルーチン開始
        StartCoroutine(LoadSceneAsync(name));
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
            if (loadingUI != null)
            {
                loadingUI.UpdateProgress(async.progress);
            }
            yield return null;
        }
    }
}
