using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

public class SelectPosition : MonoBehaviour
{
    [SerializeField] private Transform[] startPosition;

    [System.Serializable]
    public struct PlayerData
    {
        public string playerId;
        public int score;
        public GameObject playerObject;
    }

    [SerializeField] private List<PlayerData> players;

    private void Start()
    {
        AssignStartPositions();
    }

    void AssignStartPositions()
    {
        if(startPosition.Length == 0 || players.Count == 0)
        {
            Debug.LogWarning("スタート位置またはプレイヤーデータが設定されていない");
            return;
        }

        var sortedPlayers = players.OrderBy(p => p.score).ToList();

        for(int i = 0; i < sortedPlayers.Count; i++)
        {
            sortedPlayers[i].playerObject.transform.position = startPosition[i].position;
            sortedPlayers[i].playerObject.transform.rotation = startPosition[i].rotation;
        }
    }
}
