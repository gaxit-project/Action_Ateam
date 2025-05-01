using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : SingletonMonoBehaviour<PlayerBase>
{
    //プレイヤー関係
    private new Rigidbody rigidbody;
    Player player = new Player();

    //ステータス
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float weight = 10f;
    protected float rotation = 0f;

    //移動関係
    [SerializeField] protected float rotateSpeed = 100f;
    [SerializeField] private Vector3 gravity = new Vector3(0f, -20f, 0f);


    void Start()
    {
        //Rigidbodyを取得
        rigidbody = GetComponent<Rigidbody>();
        //クラス内のステータスを初期化する
        player.InitializeStatus(speed, weight);
        
        if (rigidbody)
        {
            //回転を無効化する
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            //標準の重力を無効化する
            rigidbody.useGravity = false;
        }
        else
        {
            Debug.LogError("PlayerにRigidBodyがアタッチされていません!");
        }
    }

    
    void Update()
    {
        //ステータスを取得
        speed = player.Speed;
        weight = player.Weight;
        rotation = player.Rotation;

        //移動関係
        //transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (rigidbody)
        {
            //スティックまたはWASD,矢印キーで移動
            Vector3 x = transform.right * Input.GetAxis("Horizontal") * speed;
            Vector3 z = transform.forward * Input.GetAxis("Vertical") * speed;
            rigidbody.MovePosition(rigidbody.position + (x + z) * Time.deltaTime); 
            //rigidbody.linerVelocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime;
            //固有の重力
            rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }
        
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime; //左右回転
        rotation += mouseX;
        transform.Rotate(0f,rotation,0f);
            
    }
        
        

    public class Character
    {
        //ステータス
        public float Speed;
        public float Weight;
        public float Rotation;

        /// <summary>
        /// ステータスの初期化
        /// </summary>
        public void InitializeStatus(float speed, float weight)
        {
            Speed = speed;
            Weight = weight;
        }

    }

    public class Player : Character
    {
        
    }
}
