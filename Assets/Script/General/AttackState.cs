using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.StateAI
{
    public class AttackState : IState
    {
        private Color meshColor = Color.red;
        public Color MeshColor { get => meshColor; set => meshColor = value; }

        void Update()
        {

        }
    }
}