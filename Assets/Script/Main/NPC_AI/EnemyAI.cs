using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] public List<Transform> targetCandidates = new List<Transform>();
        [SerializeField] private float targetSwitchInterval = 3f;
        public Coroutine targetSwitchCoroutine;

        private StateMachine enemyStateMachine;
        private EnemyAI enemyAI;
        private Vector3 throwVelocity;
        private new Rigidbody rigidbody;

        public bool isGrounded => isGrounded;
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

            if(targetCandidates.Count > 0)
            {
                targetSwitchCoroutine = StartCoroutine(RandomlySwitchTarget());
            }
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

            if(nearestTarget != null)
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
            
            foreach(Vector3 direction in rayDirections)
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
                if(targetCandidates.Count > 0)
                {
                    int index = Random.Range(0, targetCandidates.Count);
                    target = targetCandidates[index];
                    Debug.Log("Targetを: " + Target.name + "に変更");
                }
            }
        }

        public void Attacked(Vector3 vec, float power)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Debug.Log(vec.magnitude + ", " + power);
            for (float timer = 0f; timer < 1f; timer += Time.deltaTime)
                rb.linearVelocity = vec * power;
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
