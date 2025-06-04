using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace NPC.StateAI
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        public IdleState idleState;
        public RunState runState;
        public AttackState attackState;
        public ThrowState throwState;

        //コンストラクター
        public StateMachine(EnemyAI enemy, GameStarter gameStarter, Transform throwTarget)
        {
            this.idleState = new IdleState(enemy, gameStarter);
            this.runState = new RunState(enemy);
            this.attackState = new AttackState(enemy);
            this.throwState = new ThrowState(enemy, throwTarget);
        }

        //初期状態の指定
        public void Initialize(IState state)
        {
            CurrentState = state;
            state.Enter();
        }

        //状態遷移
        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }

        public void Update()
        {
            if(CurrentState != null)
            {
                CurrentState.Update();
            }
        }
    }
}
