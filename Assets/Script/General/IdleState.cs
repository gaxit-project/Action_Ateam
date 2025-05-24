using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI
{
    public class IdleState : IState
    {
        public Color MeshColor {  get; set; }

        private EnemyAI enemyAI;
        private GameStarter gameStarter;
        
        public IdleState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            MeshColor = Color.blue;
        }

        //IdleState‚É‘JˆÚ‚µ‚½‚Æ‚«‚É1‰ñŒÄ‚Ño‚³‚ê‚é
        public void Enter()
        {
             enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
        }

        public void Update()
        {
            if (gameStarter.IsCountStopped())
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.runState);
            }
        }

        public void Exit()
        {
            
        }
    }
}