using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{


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
