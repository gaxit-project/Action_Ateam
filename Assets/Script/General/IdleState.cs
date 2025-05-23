using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI
{
    public class IdleState : IState
    {
        private Color meshColor = Color.gray;
        public Color MeshColor { get => meshColor; set => meshColor = value; }

        public void Enter()
        {

        }

        public void Update()
        {

        }

        public void Exit()
        {

        }
    }
}