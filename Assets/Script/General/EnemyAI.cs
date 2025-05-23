using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.StateAI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float weight = 10f;
        [SerializeField] private float initialRotation = 0f;
        [SerializeField] private float rayDistance = 2f;

        [SerializeField] private bool isGround = true;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;

        private StateMachine enemyStateMachine;
        private EnemyAI enemyAI;

        public bool isGrounded => isGrounded;

        public StateMachine EnemyStateMachine => enemyStateMachine;

        private float rotation = 0f;
        private Transform attackTarget;
        private Vector3 currentVelocity = Vector3.zero;
        private Animator attackAnimator;

        private void Awake()
        {
            enemyAI = GetComponent<EnemyAI>();
            enemyStateMachine = new StateMachine(this);
        }

        private void Start()
        {
            enemyStateMachine.Initialize(enemyStateMachine.idleState);
        }

        private void Update()
        {
            enemyStateMachine.Update();
            agent.SetDestination(target.position);
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


//---------------------------------------------------------------------------------------------------


        /*enum EnemyState
        {
            Idle,
            Run,
            Throwed,
            Attacking,
            Dead
        }

        private void FixedUpdate()
        {
            switch (currentState)
            {
                case EnemyState.Idle:

                    break;
                case EnemyState.Run:

                    break;
                case EnemyState.Throwed:

                    break;
                case EnemyState.Attacking:

                    break;
                case EnemyState.Dead:
                    this.gameObject.SetActive(false);
                    break;
            }
        }

        public void Idle()
        {

        }

        

        public void Throwed()
        {

            GetComponent<NavMeshAgent>().enabled = false;
        }

        public void Attacking()
        {
            attackAnimator.SetBool("Attack", true);
        }

        public void Dead()
        {

        }*/
    }
}
