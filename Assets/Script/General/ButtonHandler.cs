using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _SENumber;

    [Header("BGMを停止するかどうか")]
    [SerializeField] private bool _stopBGM;

    /// <summary>
    /// シーンを同期で読み込むボタンが押されたとき
    /// </summary>
    public void ButtonOnClicked()
    {
        if (_stopBGM)
        {
            AudioManager.Instance.StopBGM();
        }

        AudioManager.Instance.PlaySound(_SENumber);
        SceneChangeManager.Instance.SceneChange(_sceneName);
    }

    /// <summary>
    /// シーンを非同期で読み込むボタンが押されたとき
    /// </summary>
    public void ButtonOnClickedAsync()
    {
        if (_stopBGM)
        {
            AudioManager.Instance.StopBGM();
        }

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
