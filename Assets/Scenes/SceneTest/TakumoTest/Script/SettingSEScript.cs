using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingSEScript : MonoBehaviour
{
    private GameObject LastSelected = null;

    [SerializeField] private Slider[] sliders; // Inspector‚Å•¡”“o˜^‰Â

    void Start()
    {
        
    }

    void Update()
    {
        UpdateArrowPositionResult();
    }

    void UpdateArrowPositionResult()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null || selected.GetComponent<RectTransform>() == null)
            return;

        if (selected == LastSelected) return;

        AudioManager.Instance.PlaySound(0); // © ‘I‘ğˆÚ“®‚Ì‰¹
        LastSelected = selected;
    }

    
}
