using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance {  get; private set; }

    #region シングルトン
    public static SceneChangeManager GetInstance()
    {
        if(Instance == null)
        {
            Instance = FindObjectOfType<SceneChangeManager>();
        }
        return Instance;
    }

    private void Awake()
    {
        if(this != GetInstance())
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject );
    }
    #endregion

    /// <summary>
    /// 同期でシーンをロード
    /// </summary>
    /// <param name="name">Sceneの名前</param>
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }
    /// <summary>
    /// 非同期でシーンをロード
    /// </summary>
    /// <param name="name">Sceneの名前</param>
    public void LoadSceneAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
