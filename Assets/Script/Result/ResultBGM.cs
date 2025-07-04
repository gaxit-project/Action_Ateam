using UnityEngine;

public class ResultBGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PointManager pointManager = GameObject.FindFirstObjectByType<PointManager>();
        if (pointManager == null) Debug.LogError("PointManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ!");
        else pointManager.ResultPrint();
        AudioManager.Instance.PlaySound(2);   
    }
}
