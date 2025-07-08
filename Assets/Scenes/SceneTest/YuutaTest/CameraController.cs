using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerobj;
    private Player player;
    
    private float rotationY;
    private Vector3 targetPosition;
    public Vector3 throwingCameraPosition { private get;  set; }
    private bool isChasingPlayer = true;
    //[SerializeField] private float rotateSpeed = 200f;
    private Vector3 InitialCameraDirection;
    private bool isModeChanged = false;
    private bool canMoveLaterally = true;

    void Start()
    {
        /*playerobj = GameObject.FindFirstObjectByType<PlayerBase>().gameObject; //PlayerBaseがアタッチされたオブジェクトを取得
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

        if (playerobj == null ||  player == null) return;

        if(isChasingPlayer)
        {
            if (canMoveLaterally) transform.position += playerobj.transform.position - targetPosition;
            else transform.position += new Vector3(playerobj.transform.position.x - targetPosition.x, 0f, 0f);
                targetPosition = playerobj.transform.position;
            float RstickX = player.GetRstickX; //左右回転
            //if (!isModeChanged) transform.RotateAround(targetPosition, Vector3.up, RstickX);
        }

        //Oキーを押すとカメラがその場で停止
        if (Input.GetKeyDown(KeyCode.O))
        {
            switch(isChasingPlayer)
            {
                case false:
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, playerobj.transform.eulerAngles.y, transform.eulerAngles.z);
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
        isModeChanged = true;
        this.transform.eulerAngles = InitialCameraDirection/* - new Vector3(InitialCameraDirection.x / 2f, 0f, 0f)*/;
        //this.transform.position = throwingCameraPosition + new Vector3(-18f, 12f, 0f);/*new Vector3(this.transform.position.x, this.transform.position.y, playerobj.transform.position.z) - new Vector3(0f, 3f, 0f)*/;
        canMoveLaterally = false;
    }

    /// <summary>
    /// GameManagerのStart関数側でPlayerBaseをアタッチ
    /// </summary>
    /// <param name="player">PlayerのPlayerBase</param>
    public void SetTargetPlayer(Player player, Vector3 start)
    {
        this.player = player;
        this.playerobj = player.gameObject;
        targetPosition = player.transform.position;
        InitialCameraDirection = this.transform.eulerAngles;

        //transform.position = targetPosition + new Vector3(-18f, 12f, 0f);
        transform.position = start + new Vector3(-18f, 12f, 0f);
    }
}
