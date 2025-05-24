using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI
{
    public class AttackState : IState
    {
        public Color MeshColor { get; set; }

        private EnemyAI enemyAI;
        
        public AttackState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.red;
        }

        //AttackState‚É‘JˆÚ‚µ‚½‚Æ‚«‚É1‰ñŒÄ‚Ño‚³‚ê‚é
        void Enter()
        {
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
        }

        void Update()
        {

        }

        //RunState‚©‚ç•Ê‚Ìó‘Ô‚É‘JˆÚ‚·‚é‚Æ‚«‚É1‰ñŒÄ‚Ño‚³‚ê‚é
        void Exit()
        {

        }
    }
}