using UnityEngine;

public class PlayerBase : SingletonMonoBehaviour<PlayerBase>
{
    //初期設定
    private new Rigidbody rigidbody;
    Player player = new Player();

    //ステータス
    [SerializeField] protected float speed = 300f;
    [SerializeField] protected float weight = 10f;
    protected float rotation = 0f;

    //移動関連
    [SerializeField] private Vector3 gravity = new Vector3(0f, -75f, 0f);

    void Start()
    {
        //Rigidbodyを取得
        rigidbody = GetComponent<Rigidbody>();
        //標準の重力を無効化する
        rigidbody.useGravity = false;
        //クラス内のステータスの初期化
        player.InitializeStatus(speed, weight);
    }

    
    void Update()
    {
        //ステータスの更新
        speed = player.Speed;
        weight = player.Weight;
        rotation = player.Rotation;

        //移動関連の処理
        transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (rigidbody)
        {
            //スティック、WASD、矢印キーで移動
            rigidbody.linearVelocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime;
            //重力の変更
            rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }
        else
        {
            Debug.LogError("RigidBodyがアタッチされていません！");
        }
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
