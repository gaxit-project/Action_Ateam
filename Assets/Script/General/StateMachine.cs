using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC.StateAI
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public IdleState idleState;
        public RunState runState;
        public AttackState attackState;

        //コンストラクター
        public StateMachine(EnemyAI enemy)
        {
            this.idleState = new IdleState(/*enemy*/);
            this.runState = new RunState(/*enemy*/);
            this.attackState = new AttackState(/*enemy*/);
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
