using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : SingletonMonoBehaviour<PlayerBase>
{
    //プレイヤー関係
    private new Rigidbody rigidbody;
    private new CameraController camera;
    Player player = new Player();

    //ステータス
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float weight = 10f;
    [SerializeField] protected float initialRotation = 0f;
    protected float rotation = 0f;
    private Vector3 currentVelocity  = Vector3.zero;

    //移動関係
    protected Vector3 x = Vector3.zero;
    protected Vector3 z = Vector3.zero;
    protected Vector2 lookVec;
    protected float rayDistance = 1.2f;
    [SerializeField] protected float rotateSpeed = 100f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float deceleration = 5f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private InputActionReference lookAction;

    protected override void Awake()
    {
        base.Awake(); //SingletonMonoBehaviourのAwakeに元々あったものを実行する
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
        //CameraControllerを取得
        camera = GameObject.FindFirstObjectByType<CameraController>();
        //CameraControllerがアタッチされたオブジェクトがある場合rotation.yを取得
        if (camera)
        {
            initialRotation = camera.GetInitialRotationY() / 2;
            //Debug.Log(initialRotation);
            transform.rotation = Quaternion.Euler(0f, initialRotation, 0f);
        }
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
        if (rigidbody)
        {
            //地面についている場合のみスティックまたはWASD,矢印キーの入力を受付
            if (player.IsGrounded(transform.position, rayDistance))
            {
                x = transform.right * Input.GetAxis("Horizontal");
                z = transform.forward * Input.GetAxis("Vertical");
            }

            //ローカル座標に変換
            Vector3 targetVelocity = transform.TransformDirection(Vector3.ClampMagnitude(x + z, 1f)) * speed;

            //加減速処理
            float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);

            //移動
            rigidbody.MovePosition(rigidbody.position + currentVelocity * Time.fixedDeltaTime);

            //固有の重力
            rigidbody.AddForce(new Vector3(0f, -gravity, 0f), ForceMode.Acceleration);
        }

        float RstickX = lookVec.x * rotateSpeed * Time.fixedDeltaTime; //右スティックで左右回転
        rotation += RstickX;
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
        /// <param name="speed"></param>
        /// <param name="weight"></param>
        public void InitializeStatus(float speed, float weight)
        {
            Speed = speed;
            Weight = weight;
        }

        /// <summary>
        /// 地面に接しているか確認する
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public bool IsGrounded(Vector3 pos, float distance)
        {
            Ray ray = new Ray(pos + Vector3.up * 0.1f, Vector3.down);

            return Physics.Raycast(ray, distance);
        }

    }

    public class Player : Character
    {
        
    }
}
