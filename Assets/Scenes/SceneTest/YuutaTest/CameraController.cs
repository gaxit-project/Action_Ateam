using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    private GameObject player;
    private Vector3 targetPosition;
    private bool isChasingPlayer = true;
    [SerializeField] private float rotateSpeed = 200f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerBase>().gameObject;
        if(player != null)
        {
            targetPosition = player.transform.position;
        }
        else
        {
            Debug.LogError("PlayerBaseがアタッチされたオブジェクトが見つかりません!");
        }
    }

    // Update is called once per frame
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
}
