using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _SENumber;

    /// <summary>
    /// シーンを同期で読み込むボタンが押されたとき
    /// </summary>
    public void ButtonOnClicked()
    {
        AudioManager.Instance.PlaySound(_SENumber);
        SceneChangeManager.Instance.SceneChange(_sceneName);
    }

    /// <summary>
    /// シーンを非同期で読み込むボタンが押されたとき
    /// </summary>
    public void ButtonOnClickedAsync()
    {
        AudioManager.Instance.PlaySound(_SENumber);
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
}
