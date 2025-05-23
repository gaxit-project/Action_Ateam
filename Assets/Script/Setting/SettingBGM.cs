using UnityEngine;

public class SettingBGM : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGM(1);       
    }
}
