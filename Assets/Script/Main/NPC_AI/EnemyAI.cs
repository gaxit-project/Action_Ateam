using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.StateAI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float weight = 10f;
        [SerializeField] private float rayDistance = 2f;
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private float bounceSpeed = 30.0f;
        [SerializeField] private float bounceVectorMultiple = 2f;
        [SerializeField] private bool isGround = true;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;
        [SerializeField] private GameStarter gameStarter;
        [SerializeField] private Transform throwTarget;
        [SerializeField] private Animator animator;

        private StateMachine enemyStateMachine;
        private EnemyAI enemyAI;
        private Vector3 throwVelocity;
        private new Rigidbody rigidbody;

        public bool isGrounded => isGrounded;
        public NavMeshAgent Agent => agent;
        public Transform Target => target;
        public StateMachine EnemyStateMachine => enemyStateMachine;
        public Animator Animator => animator;

        private float throwPower = 2f;
        private float rotation = 0f;
        private Transform attackTarget;
        private Vector3 currentVelocity = Vector3.zero;
        private Animator attackAnimator;

        private void Awake()
        {
            enemyAI = GetComponent<EnemyAI>();
            enemyStateMachine = new StateMachine(this, gameStarter, throwTarget);
        }

        private void Start()
        {
            enemyStateMachine.Initialize(enemyStateMachine.idleState);
        }

        private void Update()
        {
            enemyStateMachine.Update();
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

        public float ReturnSpeed()
        {
            return speed;
        }

        public float ReturnThrowPower()
        {
            return throwPower;
        }

        public float ReturnNPCSpeed()
        {
            return speed;
        }

        public bool DetectPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Player"));
            foreach(var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Vector3 toPlayer = (hit.transform.position - transform.position).normalized;
                    Vector3 forward = transform.forward;
                    float angle = Vector3.Angle(forward, toPlayer);
                    if (angle <= 45.0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // デバッグ用：検知エリアの可視化
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Vector3 forward = transform.forward;
            Quaternion leftRay = Quaternion.Euler(0, -45f, 0);
            Quaternion rightRay = Quaternion.Euler(0, 45f, 0);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, leftRay * forward * detectionRadius);
            Gizmos.DrawRay(transform.position, rightRay * forward * detectionRadius);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("ThrowArea"))
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.throwState);
                Debug.Log("throwStateに遷移");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Vector3 normal = collision.contacts[0].normal;
                Vector3 velocity = collision.rigidbody.linearVelocity.normalized;
                velocity += new Vector3(-normal.x * bounceVectorMultiple, 0f, -normal.z * bounceVectorMultiple);
                collision.rigidbody.AddForce(velocity * bounceSpeed, ForceMode.Impulse);
            }
        }
    }
}
