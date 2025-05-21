using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBase : SingletonMonoBehaviour<PlayerBase>
{
    //プレイヤー関係
    private new Rigidbody rigidbody;
    private new CameraController camera;
    private Player player = new Player();

    //ステータス
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float weight = 10f;
    [SerializeField] protected float initialRotation = 0f;
    [SerializeField] protected float maxGaugeValue = 100f;
    [SerializeField] protected float initialGaugeSpeed = 200f;
    [SerializeField] protected float maxThrowPower = 10f;
    [SerializeField] private GameObject throwGauge;
    [SerializeField] private Slider gauge;
    protected float currentGaugeValue = 0f;
    protected float currentGaugeSpeed;
    protected float rotation = 0f;
    protected Vector3 currentVelocity = Vector3.zero;
    protected Vector3 throwVelocity;
    protected bool isModeChanged = false;
    protected bool isThrowed = false;
    protected bool isGaugeIncreasing = true;
    protected float throwPower = 0f;

    //移動関係
    Vector3 throwPosition;
    protected float RstickX;
    protected Vector3 x = Vector3.zero;
    protected Vector3 z = Vector3.zero;
    protected Vector2 lookVec;
    protected float rayDistance = 1.2f;
    [SerializeField] protected float rotateSpeed = 100f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float deceleration = 5f;
    [SerializeField] protected float gravity = 20f;
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
        //クラス内のステータスを初期化する
        player.InitializeStatus(speed, weight);

        if (rigidbody)
        {
            //回転を無効化する
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            //標準の重力を無効化する
            rigidbody.useGravity = false;
        }
        else Debug.LogError("PlayerにRigidBodyがアタッチされていません!");
        if(gauge)
        {
            gauge.maxValue = maxGaugeValue;
            throwGauge.SetActive(false);
        }
        else Debug.LogError("PlayerBaseにスライダーがアタッチされていません!");

        //CameraControllerを取得
        camera = GameObject.FindFirstObjectByType<CameraController>();
        //CameraControllerがアタッチされたオブジェクトがある場合rotation.yを取得
        if (camera)
        {
            /*
            initialRotation = camera.GetCameraRotationY();
            Debug.Log(initialRotation);
            transform.rotation = Quaternion.Euler(0f, initialRotation, 0f);
            */

            Vector3 forward = camera.transform.forward; //カメラの正面
            forward.y = 0f;
            transform.rotation = Quaternion.LookRotation(forward);
        }

        //ゲージ速度の初期値を取得
        currentGaugeSpeed = initialGaugeSpeed;
    }


    void FixedUpdate()
    {
        //プレイヤーの進む方向
        Vector3 targetVelocity;

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
                /*
                x = transform.right * Input.GetAxis("Horizontal");
                z = transform.forward * Input.GetAxis("Vertical");
                */

                //カメラ基準の方向
                x = camera.transform.right * Input.GetAxis("Horizontal");
                z = camera.transform.forward * Input.GetAxis("Vertical");

                x.y = 0f;
                z.y = 0f;
                x.Normalize();
                z.Normalize();
            }

            //ローカル座標に変換
            //targetVelocity = transform.TransformDirection(Vector3.ClampMagnitude(x + z, 1f)) * speed;

            //方向決定
            targetVelocity = Vector3.ClampMagnitude(x + z, 1f) * speed;

            //加減速処理
            float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);

            //移動
            if (!isModeChanged) rigidbody.MovePosition(rigidbody.position + currentVelocity * Time.fixedDeltaTime);
            else if (!isThrowed) rigidbody.linearVelocity = Vector3.zero;
            else
            {
                //rigidbody.linearVelocity = Vector3.Lerp(rigidbody.linearVelocity, Vector3.zero, deceleration / 100f * Time.fixedDeltaTime);
            }
            //固有の重力
            rigidbody.AddForce(new Vector3(0f, -gravity, 0f), ForceMode.Acceleration);
        }

        //右スティックで回転
        RstickX = lookVec.x * rotateSpeed * Time.fixedDeltaTime;
        transform.Rotate(0f, RstickX, 0f);

        //投擲関連
        if (Input.GetKeyDown(KeyCode.Return)) //停止
        {
            ThrowStandBy();
        }
        if (isModeChanged && !isThrowed && Input.GetKeyDown(KeyCode.Space)) //投擲
        {
            Throw();
            isThrowed = true;
        }
        //ゲージの増減
        if (isModeChanged && !isThrowed)
        {
            if (isGaugeIncreasing)
            {
                currentGaugeValue += currentGaugeSpeed * Time.fixedDeltaTime;
            }
            else if (!isGaugeIncreasing)
            {
                currentGaugeValue -= currentGaugeSpeed * Time.fixedDeltaTime;
            }

            if (currentGaugeValue < 0f)
            {
                currentGaugeValue = 0f;
                isGaugeIncreasing = true;
            }
            else if (currentGaugeValue > maxGaugeValue)
            {
                currentGaugeValue = maxGaugeValue;
                isGaugeIncreasing = false;
            }

            gauge.value = currentGaugeValue;
            throwPower = (maxThrowPower * (currentGaugeValue / maxGaugeValue) / 2f) + maxThrowPower / 2f;
        }
    }

    /// <summary>
    /// 投擲後Wallタグをもつオブジェクトに当たった場合反射
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player")
        {
            if (isThrowed && collision.contactCount > 0)
            {
                // 現在の進行方向
                Vector3 incomingVelocity = rigidbody.linearVelocity;

                // 衝突面の法線
                Vector3 normal = collision.contacts[0].normal;

                // 反射ベクトルの計算
                Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal);

                // 反射後の速度倍率(多分1以下だとスピードが下がりすぎる)
                float bounceDamping = 1.5f;
                rigidbody.linearVelocity = reflectVelocity * bounceDamping;
            }
        }
    }

    /// <summary>
    /// プレイヤーとカメラの回転を同期させる用
    /// </summary>
    /// <returns></returns>
    public float GetRstickX()
    {
        return RstickX;
    }

    /// <summary>
    /// 投擲待機
    /// </summary>
    public void ThrowStandBy()
    {
        switch (isModeChanged)
        {
            case true:
                isModeChanged = false;
                throwGauge.SetActive(false);
                break;
            case false:
                isModeChanged = true;
                currentGaugeValue = 0f;
                throwGauge.SetActive(true);
                throwPosition = transform.position;
                Debug.Log(throwPosition);
                break;
        }
        if (isThrowed)
        {
            isThrowed = false;
            throwGauge.SetActive(false);
            camera.RestartCameraMove();
            rigidbody.linearVelocity = Vector3.zero;
            transform.position = throwPosition;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, camera.GetCameraRotationY(), transform.eulerAngles.z);
        }
    }

    /// <summary>
    /// 投擲
    /// </summary>
    private void Throw()
    {
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, camera.GetCameraRotationY(), transform.eulerAngles.z);
        Vector3 forward = camera.transform.forward;
        forward.y = 0f;
        transform.rotation = Quaternion.LookRotation(forward);
        throwVelocity = transform.forward * speed * throwPower;
        rigidbody.linearVelocity = throwVelocity;
        camera.StopCameraMove();
        Debug.Log("投擲");
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