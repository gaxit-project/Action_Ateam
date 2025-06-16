using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

namespace NPC.StateAI
{
    public class RunState : IState
    {
        public Color MeshColor {  get; set; }
        
        private EnemyAI enemyAI;

        public RunState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.green;
        }

        public void Enter()
        {
            enemyAI.Agent.enabled = true;
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
        }

        public void Update()
        {
            Vector3 avoidDirection = enemyAI.CalculateAvoidance();
            if (enemyAI.Agent.enabled && enemyAI.Agent.isOnNavMesh) enemyAI.Agent.SetDestination(enemyAI.Target.position);
            if (enemyAI.DetectPlayer())
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.chaseState);
                Debug.Log("ChaseState�Ɉڍs");
            }
            /*if (avoidDirection != Vector3.zero)
            {
                moveDirection = Vector3.Lerp(moveDirection, avoidDirection, 0.5f).normalized;
            }*/
        }

        public void Exit()
        {

        }

        private void OnTriggerEnter(Collision other)
        {
            if (other.gameObject.CompareTag("ThrowArea"))
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.throwState);
            }
            Debug.Log("throwState�Ɉڍs");
        }
    }
}