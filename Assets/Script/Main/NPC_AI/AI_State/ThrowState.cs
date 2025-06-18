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
        public Vector3 throwVelocity;
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
            //enemyAI.Agent.enabled = false;
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
            enemyAI.GetComponent<Rigidbody>();
            rb = enemyAI.GetComponent<Rigidbody>();
        }

        public void Update()
        {
            throwVelocity = enemyAI.transform.forward * enemyAI.ReturnSpeed() * enemyAI.ReturnThrowPower();
            rb.linearVelocity = throwVelocity;

            if (throwTarget != null)
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

        /*private void OnCollisionEnter(Collision collision)
        {
            // 投擲後 Wall または Player に当たった場合
            if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player")) && collision.contactCount > 0)
            {
                // 現在の進行方向（速度）
                Vector3 incomingVelocity = rigidbody.linearVelocity;

                // 衝突面の法線
                Vector3 normal = collision.contacts[0].normal;

                // 反射ベクトルの計算
                Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity.normalized, normal).normalized;

                // 法線方向に少し押し返しを加える（チューニング可能）
                reflectVelocity += normal * 0.2f;

                reflectVelocity.Normalize();

                enemyAI.transform.forward = reflectVelocity;
                throwVelocity = reflectVelocity * speed * throwPower;
                rigidbody.linearVelocity = throwVelocity;

            }
        }*/

    }
}