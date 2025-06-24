using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI
{
    public class IdleState : IState
    {
        //public Color MeshColor {  get; set; }
        private EnemyAI enemyAI;
        private GameStarter gameStarter;
        
        public IdleState(EnemyAI enemyAI, GameStarter gameStarter)
        {
            this.enemyAI = enemyAI;
            this.gameStarter = gameStarter /*?? throw new ArgumentNullException(nameof(gameStarter))*/;
            //MeshColor = Color.blue;
        }

        public void Enter()
        {
             //enemyAI.GetComponent<MeshRenderer>().material.color = MeshColor;
        }

        public void Update()
        {
            if (gameStarter.IsCountStopped())
            {
                enemyAI.EnemyStateMachine.TransitionTo(enemyAI.EnemyStateMachine.runState);
                Debug.Log("RunState�Ɉڍs");
            }
        }

        public void Exit()
        {
            
        }
    }
}