using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    private GameObject player;
    private Vector3 targetPosition;
    private bool isChasingPlayer = true;

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
    void Update()
    {
        //位置の更新

        if(isChasingPlayer)
        {
            transform.position += player.transform.position - targetPosition;
            targetPosition = player.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            switch(isChasingPlayer)
            {
                case false:
                    isChasingPlayer = true; 
                    break;
                case true:
                    isChasingPlayer = false;
                    break;
            }
                
        }
    }
}
