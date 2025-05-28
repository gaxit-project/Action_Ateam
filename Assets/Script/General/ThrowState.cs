using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

namespace NPC.StateAI
{
    public class ThrowState : IState
    {
        public Color MeshColor {  get; set; }

        private float throwPower = 10f;
        private EnemyAI enemyAI;
        private Vector3 throwVelocity;
        private Rigidbody rb;

        public ThrowState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.yellow;
        }

        public void Enter()
        {
            enemyAI.Agent.enabled = false;
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
            enemyAI.GetComponent<Rigidbody>();
            rb = enemyAI.GetComponent<Rigidbody>();

            Vector3 throwVelocity = enemyAI.transform.forward * enemyAI.ReturnSpeed() * enemyAI.ReturnThrowPower();
            rb.linearVelocity = throwVelocity;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            enemyAI.Agent.enabled = true;
        }

    }
}