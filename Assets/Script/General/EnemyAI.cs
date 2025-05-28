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
        [SerializeField] private bool isGround = true;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;
        [SerializeField] private GameStarter gameStarter;

        private StateMachine enemyStateMachine;
        private EnemyAI enemyAI;
        private Vector3 throwVelocity;

        public bool isGrounded => isGrounded;
        public NavMeshAgent Agent => agent;
        public Transform Target => target;
        public StateMachine EnemyStateMachine => enemyStateMachine;

        private float throwPower = 2f;
        private float rotation = 0f;
        private Transform attackTarget;
        private Vector3 currentVelocity = Vector3.zero;
        private Animator attackAnimator;

        private void Awake()
        {
            enemyAI = GetComponent<EnemyAI>();
            enemyStateMachine = new StateMachine(this, gameStarter);
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

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("ThrowArea"))
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.throwState);
                Debug.Log("throwState‚É‘JˆÚ");
            }
        }
    }
}
