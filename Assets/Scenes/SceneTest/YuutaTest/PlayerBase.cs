using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : SingletonMonoBehaviour<PlayerBase>
{
    //プレイヤー関係
    private new Rigidbody rigidbody;
    Player player = new Player();

    //ステータス
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float weight = 10f;
    protected float rotation = 0f;
    private Vector3 currentVelocity  = Vector3.zero;

    //移動関係
    protected Vector3 x = Vector3.zero;
    protected Vector3 z = Vector3.zero;
    protected Vector2 lookVec;
    [SerializeField] protected float rayDistance = 1.2f;
    [SerializeField] protected float rotateSpeed = 100f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float deceleration = 5f;
    [SerializeField] private Vector3 gravity = new Vector3(0f, -20f, 0f);
    [SerializeField] private InputActionReference lookAction;

    private void Awake()
    {
        lookAction.action.performed += OnLook;
        lookAction.action.canceled += OnLook;
    }

    private void OnDestroy()
    {
        lookAction.action.performed -= OnLook;
        lookAction.action.canceled -= OnLook;
    }

    private void OnEnable() => lookAction.action.Enable();
    private void OnDisable() => lookAction.action.Disable();

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

    
    void FixedUpdate()
    {
        //ステータスを取得
        speed = player.Speed;
        weight = player.Weight;
        rotation = player.Rotation;

        //移動関係
        //transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (rigidbody)
        {
            //地面についている場合のみスティックまたはWASD,矢印キーの入力を受付
            if (player.IsGrounded(transform.position, rayDistance))
            {
                x = transform.right * Input.GetAxis("Horizontal") * speed;
                z = transform.forward * Input.GetAxis("Vertical") * speed;
            }

            //ローカル座標に変換
            Vector3 targetVelocity = transform.TransformDirection(Vector3.ClampMagnitude(x + z, 1f)) * speed;

            //加減速処理
            float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);

            //移動
            rigidbody.MovePosition(rigidbody.position + currentVelocity * Time.fixedDeltaTime);

             //rigidbody.MovePosition(rigidbody.position + (x + z) * Time.deltaTime);
                
            //rigidbody.linerVelocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime;
            //固有の重力
            rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }
        
        //float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.fixedDeltaTime; //左右回転
        float mouseX = lookVec.x * rotateSpeed * Time.fixedDeltaTime; //左右回転
        rotation += mouseX;
        transform.Rotate(0f,rotation,0f);
            
    }
    

    private void OnLook(InputAction.CallbackContext context)
    {
        lookVec = context.ReadValue<Vector2>();
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

        /// <summary>
        /// 地面に接しているか判定する
        /// </summary>
        public bool IsGrounded(Vector3 pos, float distance)
        {
            Ray ray = new Ray(pos + Vector3.up * 0.1f, Vector3.down);

            return true;//Physics.Raycast(ray, distance);
        }

    }

    public class Player : Character
    {
        
    }
}
