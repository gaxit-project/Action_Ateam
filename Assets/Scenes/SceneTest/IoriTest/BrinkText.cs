using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField] private float _blinkSpeed = 1f;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private float _minAlpha = 0.3f; // 最小の透明度（0～1）
    [SerializeField] private float _maxAlpha = 1.0f; // 最大の透明度（0～1）

    private void Update()
    {
        float alphaRange = _maxAlpha - _minAlpha; //透明度の範囲指定
        float alpha = Mathf.PingPong(Time.time * _blinkSpeed, alphaRange) + _minAlpha; //時間によって透明度合いを調整
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha); //上の計算をテキストに適用
    }
}
