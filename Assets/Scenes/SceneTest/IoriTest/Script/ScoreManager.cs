using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    [SerializeField] private PinManager pinManager;

    private List<int> rolls = new List<int>();
    private int frame;
    private const int maxFrames = 10;

    private void Start()
    {
        if(pinManager == null)
        {
            pinManager = GetComponent<PinManager>();
        }
    }
    private void Update()
    {
        int pins = pinManager.GetKnockedDownPinCount();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Roll(pins);
            Debug.Log($"フレーム {frame + 1}: {pins} 本倒した");
            frame++;

            Debug.Log($"現在のスコア: {GetScore()}");
        }
    }

    public void Roll(int pins)
    {
        rolls.Add(pins);
    }

    public int GetScore()
    {
        int score = 0;
        for(int i = 0; i < rolls.Count; i++)
        {
            int current = rolls[i];
            score += current;

            if(current == 10 && i + 1 < rolls.Count)
            {
                score += rolls[i + 1];
            }
        }
        return score;
    }
}
