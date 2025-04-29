using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _SENumber;

    public void ButtonOnClicked()
    {
        AudioManager.Instance.PlaySound(_SENumber);
        SceneChangeManager.Instance.SceneChange(_sceneName);
    }

    public void ButtonOnClickedAsync()
    {
        AudioManager.Instance.PlaySound(_SENumber);
        SceneChangeManager.Instance.LoadNextScene(_sceneName);
    }

    public void ApplicationExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
