using UnityEngine;
using UnityEngine.UI;

public class MutedBGM : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    public Sprite muteSprite;
    public Sprite unmuteSprite;

    private Image bgmImage;

    private void Start()
    {
        bgmImage = GetComponent<Image>();

    }

    private void Update()
    {
        if (audioManager.IsBGMMuted())
        {
            bgmImage.sprite = muteSprite;
        }
        else if(!audioManager.IsBGMMuted())
        {
            bgmImage.sprite = unmuteSprite;
        }
        if(audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }
}
