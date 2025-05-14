using System;
using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    private GameObject player;
    private PlayerBase playerBase;
    private float rotationY;
    private Vector3 targetPosition;
    private bool isChasingPlayer = true;
    //[SerializeField] private float rotateSpeed = 200f;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerBase>().gameObject; //PlayerBaseがアタッチされたオブジェクトを取得
        if(player != null)
        {
            playerBase = GameObject.FindFirstObjectByType<PlayerBase>();
            targetPosition = player.transform.position;
        }
        else
        {
            Debug.LogError("PlayerBaseがアタッチされたオブジェクトが見つかりません!");
        }
    }

    void FixedUpdate()
    {
        //位置の更新

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
    /// このスクリプトが付いたカメラのy軸の角度を返す
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
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, player.transform.eulerAngles.y, transform.eulerAngles.z);
        isChasingPlayer = true;
    }
}
