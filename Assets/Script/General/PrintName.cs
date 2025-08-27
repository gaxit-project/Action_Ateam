using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrintName : MonoBehaviour
{
    [SerializeField]private Transform target;

    private PlayerBase playerbase;
    [SerializeField] private TextMeshPro _text;
    void Start()
    {
        TextName();
        int buildindex = SceneManager.GetActiveScene().buildIndex;
        if(buildindex == 3)
        {
            _text.text = playerbase.Rank.ToString();
            switch(playerbase.Rank)
            {
                case 1:
                    _text.color = new Color32(230, 180, 84, 255);
                    break;
                case 2:
                    _text.color = new Color32(192, 192, 192, 255);
                    break;
                case 3:
                    _text.color = new Color32(184, 115, 51, 255);
                    break;
                case 4:
                    _text.color = new Color32(101, 49, 142, 255);
                    break;
                default:
                    _text.color = Color.white;
                    break;
            }
        }
    }

    private void TextName()
    {
        if(playerbase == null)
        {
            playerbase = GetComponentInParent<PlayerBase>();
        }

        if(playerbase.IsBot == false)
        {
            _text.text = "You";
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
