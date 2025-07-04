using NPC.StateAI;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerBase : MonoBehaviour
{
    //プレイヤー関係
    protected new Rigidbody rigidbody;
    protected new CameraController camera;
    protected PlayerClass player = new PlayerClass();
    public bool IsBot { get; private set; } = false;
    public string PlayerID = "UnKnown";
    public Color PlayerColor;
    public int Rank { get; private set; } = 0;
    protected ScoreManager scoreManager;
    private ColorAssigner colorAssigner;

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
    protected Vector3 incomingVelocity;
    protected bool isModeChanged = false;
    protected bool isGaugeIncreasing = true;
    protected bool isThrowTimerStarted = false;
    [SerializeField] protected float throwPower = 5f;
    protected bool isReflecting = false;
    protected bool isDecelerating = false;

    //攻撃関係
    protected bool isAttacking = false;
    protected bool isAttacked = false;
    [SerializeField] protected GameObject attackArea;
    protected GameObject AttackZone;

    ResetArea resetArea;
    GameManager gameManager;

    //移動関係
    Vector3 throwPosition;
    protected float RstickX;
    protected Vector3 x = Vector3.zero;
    protected Vector3 z = Vector3.zero;
    protected float rayDistance = 1.2f; //
    [SerializeField] protected float rotateSpeed = 100f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float deceleration = 5f;
    [SerializeField] protected float gravity = 20f; //
    protected Vector3 reflection = Vector3.zero;

    //衝突エフェクト
    [SerializeField] protected GameObject collisionEffectPrefab;
    [SerializeField] protected GameObject collisionEffectPrefab2;
    [SerializeField] protected float effectDuration = 2.0f;

    [SerializeField] protected GameObject crown;

    protected virtual void Start()
    {
        //Rigidbodyを取得
        rigidbody = GetComponent<Rigidbody>();
        resetArea = GameObject.FindFirstObjectByType<ResetArea>();
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        scoreManager = GameObject.FindFirstObjectByType<ScoreManager>();
        colorAssigner = FindFirstObjectByType<ColorAssigner>();
        //クラス内のステータスを初期化する
        player.InitializeStatus(speed, weight);
        PlayerColor = colorAssigner.AssignColorToObject(gameObject);
        Debug.Log($"Bot 初期カラー: {PlayerColor}, renderer.material.color: {GetComponent<Renderer>().material.color}");


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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("NPC") || collision.gameObject.CompareTag("Wall"))
        {
            SpawnEffect(collision.contacts[0].point, 1);
        }
        else if(collision.gameObject.CompareTag("Pin"))
        {
            SpawnEffect(collision.transform.position - new Vector3(0, 1, 0), 2);
        }
    }

    protected void SpawnEffect(Vector3 position, int type)
    {
        switch (type)
        {
            case 1:
                GameObject effect = Instantiate(collisionEffectPrefab, position, Quaternion.identity);
                Destroy(effect, effectDuration);
                break;
            case 2:
                GameObject effect2 = Instantiate(collisionEffectPrefab2, position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                Destroy(effect2, effectDuration);
                break;

        }
    }

    //ここに王冠を出すスクリプトを描く
    public void Crowned()
    {
        crown.SetActive(true);
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
    public Color GetPlayerColor() => PlayerColor;

    public void SetRank(int rank)
    {
        Rank = rank;
    }
    public class PlayerClass : Character
    {

    }
}