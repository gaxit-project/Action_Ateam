using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace NPC.StateAI
{
    public class RunState : IState
    {
        public Color MeshColor {  get; set; }
        
        private EnemyAI enemyAI;
        private Sensor sensor;

        public RunState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.green;
        }

        public void Enter()
        {
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
        }

        public void Update()
        {
            enemyAI.Agent.SetDestination(enemyAI.Target.position);
            if (enemyAI.DetectPlayer())
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.attackState);
                Debug.Log("AttackState‚É‘JˆÚ");
            }
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
            Debug.Log("throwState‚É‘JˆÚ");
        }
    }
}