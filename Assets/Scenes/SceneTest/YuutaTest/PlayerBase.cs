using NPC.StateAI;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    Run,
    Throwed,
    Dead
}

public class PlayerBase : MonoBehaviour
{
    //プレイヤー関係
    private new Rigidbody rigidbody;
    private new CameraController camera;
    private Player player = new Player();
    public bool IsBot { get; private set; } = false;
    private string PlayerID = "UnKnown";
    private ScoreManager scoreManager;

    //ステータス
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float weight = 10f;
    [SerializeField] protected float initialRotation = 0f;
    /*
    [SerializeField] protected float maxGaugeValue = 100f;
    [SerializeField] protected float initialGaugeSpeed = 200f;
    [SerializeField] protected float maxThrowPower = 10f;
    [SerializeField] private GameObject throwGauge;
    [SerializeField] private Slider gauge;
    [SerializeField] protected float throwTimer;
    protected float currentGaugeValue = 0f;
    protected float currentGaugeSpeed;
    */
    protected float rotation = 0f;
    protected Vector3 currentVelocity = Vector3.zero;
    protected Vector3 throwVelocity;
    protected bool isModeChanged = false;
    protected bool isGaugeIncreasing = true;
    protected bool isThrowTimerStarted = false;
    [SerializeField] protected float throwPower = 5f;
    protected PlayerState currentState = PlayerState.Idle;

    //攻撃関係
    protected bool isAttacking = false;
    protected bool isAttacked = false;
    [SerializeField] private GameObject attackArea;
    private GameObject AttackZone;

    ResetArea resetArea;
    GameManager gameManager;

    //移動関係
    Vector3 throwPosition;
    protected float RstickX;
    protected Vector3 x = Vector3.zero;
    protected Vector3 z = Vector3.zero;
    protected float rayDistance = 1.2f;
    [SerializeField] protected float rotateSpeed = 100f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float deceleration = 5f;
    [SerializeField] protected float gravity = 20f;

    void Start()
    {
        //Rigidbodyを取得
        rigidbody = GetComponent<Rigidbody>();
        resetArea = GameObject.FindFirstObjectByType<ResetArea>();
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        scoreManager = GameObject.FindFirstObjectByType<ScoreManager>();
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
        /*
        if (gauge)
        {
            gauge.maxValue = maxGaugeValue;
            throwGauge.SetActive(false);
        }
        else Debug.LogError("PlayerBaseにスライダーがアタッチされていません!");
        */

        //CameraControllerを取得
        camera = GameObject.FindFirstObjectByType<CameraController>();
        //CameraControllerがアタッチされたオブジェクトがある場合rotation.yを取得
        if (camera)
        {

            Vector3 forward = camera.transform.forward; //カメラの正面
            forward.y = 0f;
            transform.rotation = Quaternion.LookRotation(forward);
        }

        //ゲージ速度の初期値を取得
        //currentGaugeSpeed = initialGaugeSpeed;

    }

    private void Update()
    {
        /*
        if (isThrowTimerStarted)
        {
            throwTimer += Time.deltaTime;
        }
        if (resetArea.isPlayerOut == false && throwTimer > 10f)
        {
            gameManager.currentFrameResult();
        }
        */
    }

    void FixedUpdate()
    {
        //プレイヤーの進む方向
        Vector3 targetVelocity;

        //ステータスを取得
        speed = player.Speed;
        weight = player.Weight;
        rotation = player.Rotation;

        //右スティックで回転
        if (!isAttacking && !isAttacked) RstickX = Input.GetAxis("Horizontal2") * rotateSpeed * 2f * Time.fixedDeltaTime;
        else RstickX = 0f;
        transform.Rotate(0f, RstickX, 0f);

        //固有の重力
        rigidbody.AddForce(new Vector3(0f, -gravity, 0f), ForceMode.Acceleration);

        //現在のステートによって行動を変える
        switch (currentState)
        {
            case PlayerState.Idle:

                break;

            case PlayerState.Run:

                //移動関係
                if (rigidbody)
                {

                    //地面についている場合のみスティックまたはWASD,矢印キーの入力を受付
                    if (player.IsGrounded(transform.position, rayDistance))
                    {
                        //カメラ基準の方向
                        x = camera.transform.right * Input.GetAxis("Horizontal");
                        z = camera.transform.forward * Input.GetAxis("Vertical");

                        x.y = 0f;
                        z.y = 0f;
                        x.Normalize();
                        z.Normalize();
                    }

                    //方向決定
                    targetVelocity = Vector3.ClampMagnitude(x + z, 1f) * speed;
                    //加減速処理
                    float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
                    currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);
                    //移動
                    if (!isModeChanged && !isAttacking && !isAttacked) rigidbody.MovePosition(rigidbody.position + currentVelocity/*targetVelocity*/ * Time.fixedDeltaTime);
                    else if(!isAttacked) rigidbody.linearVelocity = Vector3.zero;

                }

                //Enterキーで状態を変化
                /*
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ChangeMode();
                }
                */
                /*
                //ゲージの増減
                if (isModeChanged)
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
                //スペースキーで投擲
                if (isModeChanged && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)))
                {
                    Throw();
                }
                */

                //スペースキーまたはAボタンで攻撃
                if (!isAttacking && player.IsGrounded(transform.position, rayDistance) && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)))
                {
                    Attack();
                }

                break;

            case PlayerState.Throwed:

                //Enterキーで状態を変化
                /*
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ChangeMode();
                }
                */

                if (player.IsGrounded(transform.position, rayDistance))
                {
                    //プレイヤー基準の方向
                    x = this.transform.right * Input.GetAxis("Horizontal");

                    x.y = 0f;
                    x.Normalize();

                    targetVelocity = Vector3.ClampMagnitude(x, 1f) * speed;
                    //加減速処理
                    float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
                    currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);

                    rigidbody.linearVelocity = throwVelocity + currentVelocity / 10f;
                }


                break;

            case PlayerState.Dead:

                camera.StopCameraMove();
                this.gameObject.SetActive(false);
                scoreManager.FrameSaveSystem();
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Run状態でDeathAreaタグを持つオブジェクトに触れた場合死亡
        if ((currentState == PlayerState.Run) && other.gameObject.CompareTag("DeathArea"))
        {
            currentState = PlayerState.Dead;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 投擲後 Wall または Player に当たった場合
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player")) &&
            currentState == PlayerState.Throwed && collision.contactCount > 0)
        {
            // 現在の進行方向（速度）
            Vector3 incomingVelocity = rigidbody.linearVelocity;

            // 衝突面の法線
            Vector3 normal = collision.contacts[0].normal;

            // 反射ベクトルの計算
            Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal).normalized;

            // 法線方向に少し押し返しを加える
            reflectVelocity += normal * 0.2f;

            reflectVelocity.Normalize();

            transform.forward = reflectVelocity;
            throwVelocity = reflectVelocity * speed * throwPower;
            rigidbody.linearVelocity = throwVelocity;

        }
    }


    public void StartMove()
    {
        currentState = PlayerState.Run;
        Debug.Log("スタート!");
    }

    /// <summary>
    /// プレイヤーとカメラの回転を同期させる用
    /// </summary>
    /// <returns></returns>
    public float GetRstickX()
    {
        return RstickX;
    }

    /*
    /// <summary>
    /// 投擲待機
    /// </summary>
    public void ChangeMode()
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

        if (currentState == PlayerState.Throwed)
        {
            currentState = PlayerState.Run;
            throwGauge.SetActive(false);
            camera.RestartCameraMove();
            rigidbody.linearVelocity = Vector3.zero;
            transform.position = throwPosition;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, camera.GetCameraRotationY(), transform.eulerAngles.z);
        }
    }
    */

    /// <summary>
    /// 投擲
    /// </summary>
    public void Throw()
    {
        currentState = PlayerState.Throwed;
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, camera.GetCameraRotationY(), transform.eulerAngles.z);
        Vector3 forward = camera.transform.forward;
        forward.y = 0f;
        transform.rotation = Quaternion.LookRotation(forward);
        throwVelocity = transform.forward * speed * throwPower;
        rigidbody.linearVelocity = throwVelocity;
        //camera.StopCameraMove();
        /*
        if (!isThrowTimerStarted)
        {
            isThrowTimerStarted = true;
            throwTimer = 0f;
        }
        */
        //Debug.Log("投擲");
        camera.ChangeCameraMode();
    }

    private void Attack()
    {
        isAttacking = true;
        AttackZone = Instantiate(attackArea, this.transform);
        AttackZone.transform.localPosition = new Vector3(0.6f, 0f, 0f);
        Invoke("StopAttack", 0.5f);
    }

    private void StopAttack()
    {
        isAttacking = false;
        Destroy(AttackZone);
    }

    public void Attacked(Vector3 attackedVelocity)
    {
        if(isAttacking) StopAttack();
        isAttacked = true;
        rigidbody.linearVelocity = attackedVelocity;
        Invoke("Restart", 1f);
    }

    private void Restart()
    {
        isAttacked = false;
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

    /// <summary>
    /// PlayerID初期化
    /// </summary>
    /// <param name="id">playerのID</param>
    /// <param name="isbot">Bot判定</param>
    public void Init(string id, bool isbot)
    {
        PlayerID = id;
        IsBot = isbot;
    }

    public string GetPlayerID() => PlayerID;
    public class Player : Character
    {

    }
}