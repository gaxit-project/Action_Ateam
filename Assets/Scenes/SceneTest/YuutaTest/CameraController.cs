using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private PlayerBase playerBase;
    private float rotationY;
    private Vector3 targetPosition;
    private bool isChasingPlayer = true;
    //[SerializeField] private float rotateSpeed = 200f;
    private Vector3 InitialCameraDirection;

    void Start()
    {
        /*player = GameObject.FindFirstObjectByType<PlayerBase>().gameObject; //PlayerBaseがアタッチされたオブジェクトを取得
        if(player != null)
        {
            playerBase = GameObject.FindFirstObjectByType<PlayerBase>();
            //Debug.Log("(" + InitialCameraDirection.x + ", " + InitialCameraDirection.y + ", " + InitialCameraDirection.z + ")");
        }
        else
        {
            Debug.LogError("PlayerBaseがアタッチされたオブジェクトが見つかりません!");
        }*/
    }

    void FixedUpdate()
    {
        //位置の更新

        if (player == null || playerBase == null) return;

        if(isChasingPlayer)
        {
            transform.position += player.transform.position - targetPosition;
            targetPosition = player.transform.position;
            float RstickX = playerBase.GetRstickX(); //左右回転
            transform.RotateAround(targetPosition, Vector3.up, RstickX * 2.0f);
        }

        //Oキーを押すとカメラがその場で停止
        if (Input.GetKeyDown(KeyCode.O))
        {
            switch(isChasingPlayer)
            {
                case false:
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, player.transform.eulerAngles.y, transform.eulerAngles.z);
                    isChasingPlayer = true;
                    Debug.Log("カメラの追跡を有効化");
                    break;
                case true:
                    isChasingPlayer = false;
                    Debug.Log("カメラの追跡を無効化");
                    break;
            }
        }
    }

    /// <summary>
    /// このスクリプトが付いたカメラのy軸の角度を返す(多分もう使わない)
    /// </summary>
    /// <returns></returns>
    public float GetCameraRotationY()
    {
        rotationY = transform.eulerAngles.y; //カメラがy軸を中心にどの程度回転しているか
        return rotationY;
    }

    public void StopCameraMove()
    {
        isChasingPlayer = false;
    }
    public void RestartCameraMove()
    {
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, player.transform.eulerAngles.y, transform.eulerAngles.z);
        isChasingPlayer = true;
    }

    public void ChangeCameraMode()
    {
        Debug.Log("カメラの向き変更");
        this.transform.eulerAngles = InitialCameraDirection - new Vector3(InitialCameraDirection.x / 2f, 0f, 0f);
        this.transform.position -= new Vector3(0f, this.transform.position.y / 2f, 0f);
    }

    /// <summary>
    /// GameManagerのStart関数側でPlayerBaseをアタッチ
    /// </summary>
    /// <param name="player">PlayerのPlayerBase</param>
    public void SetTargetPlayer(PlayerBase player)
    {
        this.playerBase = player;
        this.player = player.gameObject;
        targetPosition = player.transform.position;
        InitialCameraDirection = this.transform.eulerAngles;
    }
}
