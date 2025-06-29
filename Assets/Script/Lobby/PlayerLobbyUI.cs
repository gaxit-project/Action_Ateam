using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerLobbyUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public Button readyButton;
    private LobbyManager lobbyManager;
    private int slotIndex;
    private bool isReady = false;

    private void Awake()
    {
        lobbyManager = Object.FindFirstObjectByType<LobbyManager>();
        readyButton.onClick.AddListener(ToggleReady);
    }

    public void SetPlayer(PlayerInput player, bool isPlayer1, int playerIndex)
    {
        statusText.text = $"Player{playerIndex + 1}";
        readyButton.gameObject.SetActive(true);
        if (isPlayer1)
        {
            lobbyManager.goButton.gameObject.SetActive(true);
        }
    }

    public void SetBot()
    {
        statusText.text = "Bot";
        readyButton.gameObject.SetActive(true);
        isReady = false;
        readyButton.interactable = true;
    }

    public void ClearSlot()
    {
        statusText.text = "Empty";
        readyButton.gameObject.SetActive(false);
        isReady = false;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    private void ToggleReady()
    {
        isReady = !isReady;
        readyButton.GetComponentInChildren<TextMeshPro>().text = isReady ? "Cancel" : "Ready";
        lobbyManager.SetReady(slotIndex, isReady);
    }
}
