using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public List<PlayerLobbyUI> playerSlots;
    private List<PlayerInput> players = new List<PlayerInput>();
    private int botCount = 3;
    private bool[] readyStates = new bool[4];

    public Button goButton;

    private void Start()
    {
        inputManager.JoinPlayer(playerIndex: 0, splitScreenIndex: -1, controlScheme: null);
        botCount = 3;
        UpdateLobbyUI();
        goButton.interactable = false;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        if (players.Count < 4)
        {
            players.Add(player);
            botCount = 4 - players.Count;
            UpdateLobbyUI();
        }
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        players.Remove(player);
        botCount = 4 - players.Count;
        UpdateLobbyUI();
    }

    private void UpdateLobbyUI()
    {
        for(int i = 0; i < 4; i++)
        {
            if (i < players.Count)
            {
                playerSlots[i].SetPlayer(players[i], i == 0, i);
            }
            else if (i < players.Count + botCount)
            {
                playerSlots[i].SetBot();
            }
            else
            {
                playerSlots[i].ClearSlot();
            }
        }
    }

    public void SetReady(int slotIndex, bool isReady)
    {
        readyStates[slotIndex] = isReady;
        CheckAllReady();
    }

    private void CheckAllReady()
    {
        bool allReady = true;
        for(int i = 0; i < players.Count + botCount; i++)
        {
            if (!readyStates[i])
            {
                allReady = false;
                break;
            }
        }
        goButton.interactable = allReady && players.Count > 0;
    }

    public void OnGoButton()
    {
        if (goButton.interactable)
        {
            GameManager.Instance.NumHumanPlayers = players.Count;
            GameManager.Instance.NumBots = botCount;
            SceneManager.LoadScene("Main");
        }
    }
}

