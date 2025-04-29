using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

[DisallowMultipleComponent]
public class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private Slider _loadSlider;

    void Start()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        if(_loadingUI == null || _loadSlider == null)
        {
            _loadingUI = canvas.transform.Find("Panel").gameObject;
            _loadSlider = canvas.transform.Find("Panel/LoadSlider").GetComponent<Slider>();
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
    private void OnSceneLoaded(Scene scene ,LoadSceneMode mode)
    {
        if(_loadingUI == null || _loadSlider == null)
        {
            _loadingUI = canvas.transform.Find("Panel").gameObject;
            _loadSlider = canvas.transform.Find("Panel/LoadSlider").GetComponent<Slider>();
        } 
    }
    /// <summary>
    /// ロード画面を表示する
    /// </summary>
    /// <param name="name">sceneの名前</param>
    public void LoadNextScene(string name)
    {
        if(_loadingUI == null || _loadSlider == null)
        {
            
        }
        _loadingUI.SetActive(true);
        StartCoroutine(LoadSceneAsync(name));
    }
    
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
    IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        while(!async.isDone)
        {
            _loadSlider.value = async.progress;
            yield return null;
        }
    }
}
