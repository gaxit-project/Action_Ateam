using UnityEngine;

public class TitleBGM : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGM(0);
    }


}
