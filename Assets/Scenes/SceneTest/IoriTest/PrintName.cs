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
            _text.text = playerbase.PlayerID;
            _text.color = playerbase.PlayerColor;
        }
    }
}
