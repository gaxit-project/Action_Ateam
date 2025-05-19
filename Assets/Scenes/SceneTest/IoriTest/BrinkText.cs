using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrinkText : MonoBehaviour
{
    [SerializeField] private float _blinkSpeed = 1f;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time * _blinkSpeed, 1f);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
    }
}
