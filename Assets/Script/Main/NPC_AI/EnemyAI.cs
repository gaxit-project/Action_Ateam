using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace NPC.StateAI
{
    public class EnemyAI : PlayerBase
    {
        //[SerializeField] private float speed = 5f;
        //[SerializeField] private float weight = 10f;
        //[SerializeField] private float rayDistance = 2f;
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float bounceSpeed = 30.0f;
        [SerializeField] private float bounceVectorMultiple = 2f;
        [SerializeField] private bool isGround = true;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;
        [SerializeField] private GameStarter gameStarter;
        [SerializeField] private Transform throwTarget;
        [SerializeField] private Animator animator;

        [SerializeField] public GameObject[] targetCandidates;
        [SerializeField] private float targetSwitchInterval = 1f;
        public Coroutine targetSwitchCoroutine;

        private StateMachine enemyStateMachine;
        private EnemyAI enemyAI;
        //private Vector3 throwVelocity;

        public bool isGrounded => isGround;
        public NavMeshAgent Agent => agent;
        public Transform Target
        {
            get => target;
            set => target = value;
        }

        public StateMachine EnemyStateMachine => enemyStateMachine;
        public Animator Animator => animator;

        public float avoidAngle = 45.0f;
        public LayerMask obstacleLayer;

        //private float throwPower = 5f;
        //private float rotation = 0f;
        private Transform attackTarget;
        //private Vector3 currentVelocity = Vector3.zero;
        private Vector3 targetDirection = Vector3.zero;
        private Animator attackAnimator;
        Rigidbody rb;

        private void Awake()
        {
            enemyAI = GetComponent<EnemyAI>();
            rb = GetComponent<Rigidbody>();
            enemyStateMachine = new StateMachine(this, gameStarter, throwTarget);
        }

        protected override void Start()
        {
            //クラス内のステータスを初期化する
            player.InitializeStatus(speed, weight);
            enemyStateMachine.Initialize(enemyStateMachine.idleState);
            StartCoroutine("GetTargets");
            PlayerColor = ColorAssigner.Instance.GetAssignedColor(gameObject);
            arrowUIName = "Arrow4";
            arrowUI = transform.Find(arrowUIName).gameObject;
            if (arrowUI == null) Debug.LogError("UIが見つかりません！");
        }

        protected override void Update()
        {
            base.Update();
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            enemyStateMachine.Update();
            //if (!IsGrounded() && enemyAI.agent.enabled) enemyAI.agent.enabled = false;
            //else if (IsGrounded() && !enemyAI.agent.enabled && enemyAI.EnemyStateMachine.CurrentState != enemyAI.EnemyStateMachine.throwState) enemyAI.agent.enabled = true;

            //反射準備
            bool isApproaching = false;
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.CompareTag("Wall")) isApproaching = true;
            }

            Collider[] obj = Physics.OverlapSphere(transform.position, 1.5f, LayerMask.GetMask("Player"));
            if (obj.Length > 1) isApproaching = true;

            if (!isApproaching) incomingVelocity = rb.linearVelocity;
        }

        private void FixedUpdate()
        {
            rb.AddForce(new Vector3(0f, -20f, 0f), ForceMode.Acceleration); 

            if (targetDirection != Vector3.zero && enemyAI.EnemyStateMachine.CurrentState != enemyAI.EnemyStateMachine.throwState)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 50f * Time.fixedDeltaTime);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            }
        }

        private void LateUpdate()
        {
            CalculateVertical();
            Run();
        }

        private void Run()
        {

        }

        private void CalculateVertical()
        {

        }

        public float ReturnSpeed() => speed;
        public float ReturnThrowPower() => throwPower;

        public Transform DetectPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Player"));
            Transform nearestTarget = null;
            float minDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Vector3 toPlayer = (hit.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, toPlayer);
                    if (angle <= 45.0f)
                    {
                        float distance = Vector3.Distance(transform.forward, toPlayer);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestTarget = hit.transform;
                        }
                    }
                }
            }

            if (nearestTarget != null)
            {
                target = nearestTarget;
            }
            return nearestTarget;
        }

        public Transform AttackPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius / 3, LayerMask.GetMask("Player"));
            Transform nearestTarget = null;
            float minDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Vector3 toPlayer = (hit.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, toPlayer);
                    if (angle <= 45.0f)
                    {
                        float distance = Vector3.Distance(transform.forward, toPlayer);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestTarget = hit.transform;
                        }
                    }
                }
            }

            if (nearestTarget != null)
            {
                target = nearestTarget;
            }
            return nearestTarget;
        }


        // デバッグ用：検知エリアの可視化
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Vector3 forwardOut = transform.forward;
            Quaternion leftRay = Quaternion.Euler(0, -45f, 0);
            Quaternion rightRay = Quaternion.Euler(0, 45f, 0);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, leftRay * forwardOut * detectionRadius);
            Gizmos.DrawRay(transform.position, rightRay * forwardOut * detectionRadius);
            Vector3 forwardIn = transform.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, leftRay * forwardIn * (detectionRadius / 3));
            Gizmos.DrawRay(transform.position, rightRay * forwardIn * (detectionRadius / 3));
        }

        public Vector3 CalculateAvoidance()
        {
            Vector3 avoidance = Vector3.zero;
            RaycastHit hit;

            Vector3[] rayDirections = { transform.forward, Quaternion.Euler(0, avoidAngle, 0) * transform.forward, Quaternion.Euler(0, -avoidAngle, 0) * transform.forward };

            foreach (Vector3 direction in rayDirections)
            {
                if (Physics.Raycast(transform.position, direction, out hit, rayDistance, obstacleLayer))
                {
                    float distanceFactor = (rayDistance - hit.distance) / rayDistance;
                    Vector3 hitNormal = hit.normal;
                    avoidance += hitNormal * distanceFactor;
                }
            }

            return avoidance.magnitude > 0 ? avoidance.normalized : Vector3.zero;
        }

        public void RandomTarget()
        {
            targetSwitchCoroutine = StartCoroutine(RandomlySwitchTarget());
        }

        public IEnumerator RandomlySwitchTarget()
        {
            while (true)
            {
                yield return new WaitForSeconds(targetSwitchInterval);
                if (targetCandidates.Length > 0 && enemyAI.EnemyStateMachine.CurrentState != enemyAI.EnemyStateMachine.throwState)
                {
                    int index = Random.Range(0, targetCandidates.Length);
                    target = targetCandidates[index].transform;
                    Vector3 toTarget = (target.position - transform.position).normalized;
                    toTarget.y = 0f;
                    Quaternion rndQuat = Quaternion.Euler(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                    targetDirection = rndQuat * toTarget;
                    Debug.Log("Targetを: " + Target.name + "に変更");
                }
            }
        }

        public async void Attacked(Vector3 vec, float power)
        {
            if (enemyAI.EnemyStateMachine.CurrentState == enemyAI.EnemyStateMachine.runState)
            {
                rb.linearVelocity += vec * power;
                await Task.Delay(300); StartCoroutine(Decelerate());
            }
            else if (enemyAI.EnemyStateMachine.CurrentState == enemyAI.EnemyStateMachine.throwState) rb.AddForce(vec * power);
        }

        private IEnumerator Decelerate()
        {
            Vector3 v = rb.linearVelocity;
            while (rb.linearVelocity.magnitude > 0.5f)
            {
                v *= 0.9f;
                rb.linearVelocity = v;
                yield return null;
            }

            rb.linearVelocity = Vector3.zero;
        }

        private bool IsGrounded()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            return Physics.Raycast(ray, 1.2f);
        }

        private void GetTargets()
        {
            int i = 0;
            GameObject[] tar = GameObject.FindGameObjectsWithTag("Target");
            if (tar.Length > 0)
            {
                foreach (GameObject t in tar)
                {
                    Vector3 dis = t.transform.position - transform.position;
                    dis.y = 0;
                    if (dis.magnitude <= 300f) targetCandidates[i++] = t;
                }
                if (targetCandidates.Length > 0)
                {
                    target = targetCandidates[0].transform;
                    transform.LookAt(target);
                    targetSwitchCoroutine = StartCoroutine(RandomlySwitchTarget());
                }
                else Debug.LogError("範囲内にtargetタグを持つオブジェクトが存在しません！");
            }
            else if(SceneManager.GetActiveScene().buildIndex == 1) StartCoroutine("GetTargets");
        }

        public new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.gameObject.CompareTag("ThrowArea"))
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.throwState);
                Debug.Log("throwStateに遷移");
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);
            if (collision.gameObject.CompareTag("Wall") && collision.contactCount > 0)
            {
                //Debug.Log("当たった");

                // 衝突面の法線
                Vector3 normal = collision.contacts[0].normal;

                // 反射ベクトルの計算
                Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal).normalized;
                Debug.DrawRay(transform.position, Vector3.ClampMagnitude(reflectVelocity, 3f), Color.cyan, 3f, false);

                // 法線方向に少し押し返しを加える
                //reflectVelocity += normal * 0.2f;

                reflectVelocity.Normalize();

                transform.forward = reflectVelocity;
                throwVelocity = reflectVelocity * speed * throwPower;
                rb.linearVelocity = throwVelocity;
                Invoke(nameof(ChangeIncomingVelocity), 0.01f);
            }
            else if (collision.gameObject.CompareTag("NPC") || collision.gameObject.CompareTag("Player"))
            {
                Vector3 v = Vector3.zero;
                if (collision.gameObject.CompareTag("NPC"))
                {
                    EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                    v = enemy.GetIncomingVelocity();
                }
                else if (collision.gameObject.CompareTag("Player"))
                {
                    Player player = collision.gameObject.GetComponent<Player>();
                    v = player.GetIncomingVelocity();
                }
                //Debug.DrawRay(transform.position, incomingVelocity + new Vector3(v.x, 0f, v.z * 5f), Color.cyan, (incomingVelocity + new Vector3(v.x, 0f, v.z * 5f)).magnitude);
                transform.forward = Vector3.ClampMagnitude(incomingVelocity + new Vector3((v.x + incomingVelocity.x) * 2f, 0f, v.z * 5f), 1f);
                rb.linearVelocity = transform.forward * speed * throwPower;
                Invoke(nameof(ChangeIncomingVelocity), 0.01f);
            }
        }

        public Vector3 GetIncomingVelocity()
        {
            return incomingVelocity;
        }

        private void ChangeIncomingVelocity()
        {
            incomingVelocity = rb.linearVelocity;
        }

        /// <summary>
        /// 強制反射
        /// </summary>
        protected override void ForcedReflection()
        {
            Vector3 reflectVelocity = new Vector3(incomingVelocity.x, incomingVelocity.y, -incomingVelocity.z).normalized;
            transform.forward = reflectVelocity;
            throwVelocity = reflectVelocity * speed * throwPower;
            rb.linearVelocity = throwVelocity;
            Invoke(nameof(ChangeIncomingVelocity), 0.01f);
        }

        /// <summary>
        /// バフによる加速処理
        /// </summary>
        /// <param name="buff"></param>
        public override void ApplyBuff(BuffItem buff)
        {
            base.ApplyBuff(buff);
            throwVelocity = transform.forward * speed * throwPower;
            rb.linearVelocity = throwVelocity;
        }
    }
}
