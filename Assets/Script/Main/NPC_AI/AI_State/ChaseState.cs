using NPC.StateAI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NPC.StateAI{
    public class ChaseState : IState
    {
        public Color MeshColor { get; set; }

        private GameObject target;
        private EnemyAI enemyAI;
        private Transform targetTransform;
        private float chaseTimer;

        [SerializeField] private float chaseTime = 3.0f;

        public ChaseState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.red;
        }

        public void Enter()
        {
            GameObject targetObject = GameObject.FindWithTag("Player");
            chaseTimer = chaseTime;
        }

        public void Update()
        {
            chaseTimer -= Time.deltaTime;
            if(chaseTimer <= 0)
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.runState);
            }

            Vector3 direction = (targetTransform.position - enemyAI.transform.position).normalized;
            float distance = Vector3.Distance(enemyAI.transform.position, targetTransform.position);

            enemyAI.transform.position += direction * enemyAI.ReturnNPCSpeed() * Time.deltaTime;
            enemyAI.transform.LookAt(targetTransform);
        }

        public void Exit()
        {

        }
    }
}
