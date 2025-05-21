using UnityEngine;

public class MainBGM : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGM(0);   
    }
}
