using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI
{
    public class AttackState : IState
    {
        public Color MeshColor { get; set; }
        private EnemyAI enemyAI;
        private bool isAnimationFinished;

        public AttackState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.red;
        }

        void Enter()
        {
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
            enemyAI.Agent.isStopped = true;
            isAnimationFinished = false;

            if (enemyAI.Animator != null)
            {
                enemyAI.Animator.SetTrigger("Attack");
                enemyAI.StartCoroutine(WaitForAnimation());
            }
            else
            {
                isAnimationFinished = true;
            }
        }

        void Update()
        {

        }

        void Exit()
        {

        }

        private System.Collections.IEnumerator WaitForAnimation()
        {
            float animationLength = 1f;
            AnimatorClipInfo[] clipInfo = enemyAI.Animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                animationLength = clipInfo[0].clip.length;
            }

            yield return new WaitForSeconds(animationLength);
            isAnimationFinished = true;
        }
    }
}