using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerJoinManger : MonoBehaviour
{
    //プレイヤーがゲームに参加するためのInputAction
    [SerializeField] private InputAction _joinInputAction = default;
    //PlayerInputがアタッチされているplayerobject
    [SerializeField] private PlayerInput _playerPrefab = default;
    //最大参加人数
    [SerializeField] private int _maxPlayerCount = default;

    //参加済のデバイス情報
    private InputDevice[] joinedDevices = default;
    //現在のプレイヤー数
    [SerializeField]private int currentPlayerCount = 0;

    private PlayerInfo[] playerInfos = default;

    private PlayerInputManager playerInputManager;

    [SerializeField] private GameObject _botPrefab;

    //Text
    [SerializeField] private TextMeshProUGUI[] texts;
    private void Awake()
    {
        //配列初期化
        joinedDevices = new InputDevice[_maxPlayerCount];

        playerInfos = new PlayerInfo[_maxPlayerCount];

        //IputActionを有効化して、コールバックを設定
        _joinInputAction.Enable();
        _joinInputAction.performed += OnJoin;

        playerInputManager = GetComponent<PlayerInputManager>();

        //playerInputManager.onPlayerJoined += onPlayerJoined;
        //playerInputManager.onPlayerLeft -= onPlayerLeft;
    }

    /*private void onPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Join");
    }

    private void onPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Left");
    }*/

    private void OnDestroy()
    {
        _joinInputAction.Dispose();
    }

    private void OnJoin(InputAction.CallbackContext context)
    {
        if(currentPlayerCount >= _maxPlayerCount)
        {
            return;
        }

        foreach(var device in joinedDevices)
        {
            if(context.control.device == device)
            {
                return;
            }
        }

        PlayerInput newPlayer = PlayerInput.Instantiate(
            prefab: _playerPrefab.gameObject,
            playerIndex: currentPlayerCount,
            pairWithDevice: context.control.device
            );

        Transform playerPosition = newPlayer.transform;
        playerPosition.position = new Vector3(0f, 0f, 0f);

        joinedDevices[currentPlayerCount] = context.control.device;
        CharacterType characterType = (CharacterType)currentPlayerCount;
        playerInfos[currentPlayerCount] = new PlayerInfo(joinedDevices[currentPlayerCount], characterType);

        TextChanged(currentPlayerCount);

        DontDestroyOnLoad(newPlayer);

        GameManager gameManager = GameManager.Instance;

        gameManager.NumHumanPlayers++;
        gameManager.NumBots--;

        gameManager.playerObj[currentPlayerCount] = newPlayer.gameObject;
        var player = newPlayer.gameObject.GetComponent<Player>();
        player.Init($"Player{currentPlayerCount + 1}", false);
        gameManager.players.Add(player);
        if(gameManager.IsStart == false)
        {
            gameManager.playerScores.Add(new PlayerScoreData($"Player{currentPlayerCount + 1}", false));
        }

        currentPlayerCount++;       

    }

    public void GameStart()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.playerInfos = this.playerInfos;


        for(int i = 0; i < gameManager.NumBots; i++)
        {
            var botObj = Instantiate(_botPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            var bot = botObj.GetComponent<PlayerBase>();
            bot.Init($"Bot{i + 1}", true);
            gameManager.players.Add(bot);
            if(gameManager.IsStart==false)
            {
                gameManager.playerScores.Add(new PlayerScoreData($"Bot{i + 1}", true));
            }
            DontDestroyOnLoad(botObj);
            gameManager.playerObj[i + gameManager.NumHumanPlayers] = botObj;
        }

        SceneChangeManager.Instance.SceneChange("Main");
    }

    private void TextChanged(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                texts[0].text = "Joined!";
                break;
            case 1:
                texts[1].text = "Joined!";
                break;
            case 2:
                texts[2].text = "Joined!";
                break;
            case 3:
                texts[3].text = "Joined!";
                break;
        }
    }
}
public enum CharacterType
{
    Character1,
    Character2,
    Character3,
    Character4,
}

public class PlayerInfo
{
    public InputDevice PairWithDevice { get;  private set; } = default;
    public CharacterType SelectedCharacter { get;  private set; } = default;

    public PlayerInfo(InputDevice pairWithDevice, CharacterType selectedCharacter)
    {
        PairWithDevice = pairWithDevice;
        SelectedCharacter = selectedCharacter;
    }
}
