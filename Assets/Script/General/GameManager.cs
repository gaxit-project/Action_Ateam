using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public int score; //スコア

    /// <summary>
    /// スコア計算
    /// </summary>
    /// <returns>スコアを返す</returns>
    public int CalcScore()
    {
        score = PinManager.Instance.GetKnockedDownPinCount();
        return score;
    }
}
