using UnityEngine;

public class ResultBGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {     
        AudioManager.Instance.PlaySound(2);   
    }
}
