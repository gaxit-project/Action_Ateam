using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingPanel;
    //[SerializeField] private Slider _loadingSlider;
    [SerializeField] private TextMeshProUGUI _activeText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private ButtonHandler buttonHandler;

    /// <summary>
    /// Canvas(ロード画面)を表示する
    /// </summary>
    public void ShowLoading()
    {
        if (_loadingPanel != null)
        {
            _loadingPanel.SetActive(true);
            startButton.gameObject.SetActive(false);
            settingsButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
        }
    }

    public void ShowLoadingSlider()
    {
        /*if(_loadingSlider != null)
        {
            _loadingSlider.gameObject.SetActive(true);
        }*/
    }

    public void HideLoadingSlider()
    {
        /*if (_loadingSlider != null)
        {
            _loadingSlider.gameObject.SetActive(false);
        }*/
    }

    /// <summary>
    /// ロード画面のスライダーを動かす
    /// </summary>
    /// <param name="value">ロードがどこまで進んでいるかの値</param>
    public void UpdateProgress(float progress)
    {
       /* if (_loadingSlider != null)
        {
            _loadingSlider.value = progress;
        }*/
    }

    /// <summary>
    /// ロード画面のTextを表示する
    /// </summary>
    public void ShowPushButtonText()
    {
        if (_activeText != null)
        {
            _activeText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ロード画面のTextを隠す
    /// </summary>
    public void HidePushButtonText()
    {
        if (_activeText != null)
        {
            _activeText.gameObject.SetActive(false);
        }
    }
}
