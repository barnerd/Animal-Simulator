using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class FleeingState : State<Creature>
    {
        public FleeingState(CreatureLogicSM _sm) : base("Fleeing", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
        }

        public override State<Creature> Execute(Creature _owner)
        {
            // return next state
            return CreatureLogicSM.idleState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
        }
    }
}
