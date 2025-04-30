using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Slider _loadSlider;

    public void ShowLoading()
    {
        if (_loadingPanel != null)
        {
            _loadingPanel.SetActive(true);
        }
    }

    public void UpdateProgress(float value)
    {
        if (_loadSlider != null)
        {
            _loadSlider.value = value;
        }
    }
}
