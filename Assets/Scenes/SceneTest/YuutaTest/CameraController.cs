using System;
using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    private GameObject player;
    private float initialRotationY;
    private Vector3 targetPosition;
    private bool isChasingPlayer = true;
    [SerializeField] private float rotateSpeed = 200f;


    protected override void Awake()
    {
        base.Awake();
        initialRotationY = transform.eulerAngles.y; //カメラがy軸を中心にどの程度回転しているか
    }

    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerBase>().gameObject; //PlayerBaseがアタッチされたオブジェクトを取得
        if(player != null)
        {
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

        }
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.fixedDeltaTime; //左右回転
        transform.RotateAround(targetPosition, Vector3.up, mouseX);

        //Oキーを押すとカメラがその場で停止
        if (Input.GetKeyDown(KeyCode.O))
        {
            switch(isChasingPlayer)
            {
                case false:
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
    public float GetInitialRotationY()
    {
        return initialRotationY;
    }
}
