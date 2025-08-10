using UnityEngine;

public class PlayerScoreData{
    public string PlayerID {  get; private set; } //string型のID
    public bool IsBot {  get; private set; } //Bot判定
    public int[] FrameScores = new int[11]; //各フレームのスコア

    /// <summary>
    /// PlayerScoreDataを設定
    /// </summary>
    /// <param name="id">割り当てるID名</param>
    /// <param name="isBot">Bot判定を割り当てる</param>
    public PlayerScoreData(string id, bool isBot)
    {
        PlayerID = id;
        IsBot = isBot;
        for (int i = 0; i < FrameScores.Length; i++) FrameScores[i] = 0;
    }

    /// <summary>
    /// FrameScoreをリストに保存
    /// </summary>
    /// <param name="frame">保存するフレーム</param>
    /// <param name="score">フレームのスコア</param>
    public void Addscore(int frame, int score)
    {
        if (frame < 1 || frame > 10) return;
        FrameScores[frame] = score;
        FrameScores[0] += score;
    }

    /// <summary>
    /// 合計得点を取得
    /// </summary>
    /// <returns>合計得点</returns>
    public int GetTotalScore()
    {
        if (FrameScores[0] == 0)
        {
            return 0;
        }
        else
        {
            return FrameScores[0];
        }
    }

    /// <summary>
    /// 各フレームのスコアを取得
    /// </summary>
    /// <param name="frame">フレーム</param>
    /// <returns>frameに入れたスコア</returns>
    public int GetScore(int frame)
    {
        return FrameScores[frame];
    }
}
