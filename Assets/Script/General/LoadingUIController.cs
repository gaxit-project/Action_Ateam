using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Slider _loadSlider;

    /// <summary>
    /// Canvas(ロード画面)を表示する
    /// </summary>
    public void ShowLoading()
    {
        if (_loadingPanel != null)
        {
            _loadingPanel.SetActive(true);
        }
    }

    /// <summary>
    /// ロード画面のスライダーを動かす
    /// </summary>
    /// <param name="value">ロードがどこまで進んでいるかの値</param>
    public void UpdateProgress(float value)
    {
        if (_loadSlider != null)
        {
            _loadSlider.value = value;
        }
    }
}
