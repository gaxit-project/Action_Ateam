using TMPro;
using UnityEngine;

public class PrintName : MonoBehaviour
{
    [SerializeField]private Transform target;

    private PlayerBase playerbase;
    [SerializeField] private TextMeshProUGUI _text;
    void Start()
    {
        TextName();
    }

    private void TextName()
    {
        if(playerbase == null)
        {
            playerbase = GetComponentInParent<PlayerBase>();
        }

        if(playerbase.IsBot == false)
        {
            _text.text = "YOU";
            _text.color = playerbase.PlayerColor;
        }
        else
        {
            string tmp = playerbase.GetColorName();
            _text.text = tmp;
            _text.color = playerbase.PlayerColor;
        }
    }
}
