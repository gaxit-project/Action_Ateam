using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //RunState‚É‘JˆÚ‚µ‚½‚Æ‚«‚É1‰ñŒÄ‚Ño‚³‚ê‚é
        public void Enter()
        {
            enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
        }

        public void Update()
        {

        }

        //RunState‚©‚ç•Ê‚Ìó‘Ô‚É‘JˆÚ‚·‚é‚Æ‚«‚É1‰ñŒÄ‚Ño‚³‚ê‚é
        public void Exit()
        {

        }
    }
}