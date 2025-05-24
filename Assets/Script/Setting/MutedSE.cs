using UnityEngine;
using UnityEngine.UI;

public class MutedSE : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    public Sprite muteSprite;
    public Sprite unmuteSprite;

    private Image seImage;

    private void Start()
    {
        seImage = GetComponent<Image>();

    }

    private void Update()
    {
        if (audioManager.IsSEMuted())
        {
            seImage.sprite = muteSprite;
        }
        else if(!audioManager.IsBGMMuted())
        {
            seImage.sprite  = unmuteSprite;
        }
        if(audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }
}
