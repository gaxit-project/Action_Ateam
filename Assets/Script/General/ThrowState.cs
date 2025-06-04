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
        private float lateralSpeed = 5f;
        private EnemyAI enemyAI;
        private Vector3 throwVelocity;
        private Rigidbody rb;
        private Transform throwTarget;

        public ThrowState(EnemyAI enemyAI, Transform throwTarget)
        {
            this.enemyAI = enemyAI;
            this.throwTarget = throwTarget;
            MeshColor = Color.yellow;
        }

        public void Enter()
        {
            enemyAI.Agent.enabled = false;
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
            enemyAI.GetComponent<Rigidbody>();
            rb = enemyAI.GetComponent<Rigidbody>();

            throwVelocity = enemyAI.transform.forward * enemyAI.ReturnSpeed() * enemyAI.ReturnThrowPower();
            rb.linearVelocity = throwVelocity;
        }

        public void Update()
        {
            if(throwTarget != null)
            {
                Vector3 directionToTarget = (throwTarget.position - enemyAI.transform.position).normalized;

                Vector3 forward = enemyAI.transform.forward;
                Vector3 lateralDirection = Vector3.Cross(Vector3.up, forward).normalized;

                float lateralDot = Vector3.Dot(directionToTarget, lateralDirection);
                Vector3 lateralForce = lateralDirection * lateralDot * lateralSpeed;

                rb.AddForce(lateralForce, ForceMode.VelocityChange);
            }
        }
            

        public void Exit()
        {
            enemyAI.Agent.enabled = true;
        }

    }
}