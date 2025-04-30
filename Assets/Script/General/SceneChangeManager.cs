using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{
    [SerializeField] private LoadingUIController loadingUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange("Result");
        }
    }
    public void LoadNextScene(string name)
    {
        if (loadingUI != null)
        {
            loadingUI.ShowLoading();
        }
        StartCoroutine(LoadSceneAsync(name));
    }

    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

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
