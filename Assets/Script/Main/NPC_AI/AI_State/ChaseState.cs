using NPC.StateAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI{
    public class ChaseState : IState
    {
        //public Color MeshColor { get; set; }

        private GameObject target;
        private EnemyAI enemyAI;
        private Transform targetTransform;
        private float chaseTimer;
        private bool isFindPlayer = true;

        [SerializeField] private float chaseTime = 3.0f;
        [SerializeField] private float minDistance = 1.0f;

        public ChaseState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            //MeshColor = Color.red;
        }

        public void Enter()
        {
            enemyAI.StartCoroutine(FindPlayer());
        }

        private IEnumerator FindPlayer()
        {
            while (target == null)
            {
                targetTransform = enemyAI.DetectPlayer();
                if(targetTransform != null)
                {
                    target = targetTransform.gameObject;
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
            chaseTimer = chaseTime;
        }

        public void Update()
        {
            if (isFindPlayer)
            {
                chaseTimer -= Time.deltaTime;
                Vector3 direction = (targetTransform.position - enemyAI.transform.position).normalized;
                float distance = Vector3.Distance(enemyAI.transform.position, targetTransform.position);

                enemyAI.transform.LookAt(targetTransform);
                enemyAI.transform.position += direction * enemyAI.ReturnSpeed() * Time.deltaTime;

                if (enemyAI.AttackPlayer())
                {
                    enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.runState);
                    Debug.Log("AttackStateÇ…à⁄çs");
                }
            }
            else
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.runState);
            }

            if (chaseTimer <= 0 && isFindPlayer)
            {
                Debug.Log("í«ê’èIóπ");
                isFindPlayer = false;
                enemyAI.RandomTarget();
            }
        }

        public void Exit()
        {

        }
    }
}
