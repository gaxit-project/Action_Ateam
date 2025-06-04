using UnityEngine;

public class PlayerScoreData{
    public string PlayerID {  get; private set; }
    public bool IsBot {  get; private set; }
    public int[] FrameScores = new int[11];

    public PlayerScoreData(string id, bool isBot)
    {
        PlayerID = id;
        IsBot = isBot;
        for (int i = 0; i < FrameScores.Length; i++) FrameScores[i] = 0;
    }

    public void Addscore(int frame, int score)
    {
        if (frame < 1 || frame > 10) return;
        FrameScores[frame] = score;
        FrameScores[0] += score;
    }

    public int GetTotalScore()
    {
        return FrameScores[0];
    }

    public int GetScore(int frame)
    {
        return FrameScores[frame];
    }
}
