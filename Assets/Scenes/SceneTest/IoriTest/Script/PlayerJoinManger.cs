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
    private int currentPlayerCount = 0;
    private void Awake()
    {
        //配列初期化
        joinedDevices = new InputDevice[_maxPlayerCount];

        //IputActionを有効化して、コールバックを設定
        _joinInputAction.Enable();
        _joinInputAction.performed += OnJoin;
    }

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

        PlayerInput.Instantiate(
            prefab: _playerPrefab.gameObject,
            playerIndex: currentPlayerCount,
            pairWithDevice: context.control.device
            );

        joinedDevices[currentPlayerCount] = context.control.device;

        currentPlayerCount++;       
    }
}
