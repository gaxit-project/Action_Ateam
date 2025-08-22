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
            this.gameObject.SetActive(false);
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
