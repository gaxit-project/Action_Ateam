using UnityEngine;

public class ResolutionChanger : MonoBehaviour
{

    /// <summary>
    /// ボタンにオンclickして使ってください。
    /// </summary>
    /// 

    
    public void SetResolution960x540()
    {
        SetResolution(960, 540);
    }

    public void SetResolution1280x720()
    {
        SetResolution(1280, 720);
    }

    public void SetResolution1920x1080()
    {
        SetResolution(1920, 1080);
    }

    public void SetResolution2560x1440()
    {
        SetResolution(2560, 1440);
    }

    public void SetResolution3840x2160()
    {
        SetResolution(3840, 2160);
    }

    private void SetResolution(int width, int height)
    {
        // 現在のRefreshRate型の値を取得（refreshRateRatioを使う）
        var refreshRate = Screen.currentResolution.refreshRateRatio;

        Debug.Log($"解像度を {width}x{height} に変更します");

        // 新形式：RefreshRateを渡すオーバーロード
        Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen, refreshRate);
        //引数は順番に横の長さ、縦の長さ、Windowsの種類、画面を切り替える回数(fps)
    }
}
