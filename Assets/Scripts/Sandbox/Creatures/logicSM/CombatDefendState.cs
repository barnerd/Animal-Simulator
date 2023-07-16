using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class CombatDefendState : State<Creature>
    {
        public CombatDefendState(CreatureLogicSM _sm) : base("Combat Defend", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
        }

        public override State<Creature> Execute(Creature _owner)
        {
            CreatureCombat creatureCombat = _owner.GetComponent<CreatureCombat>();
            CreatureSkills creatureSkills = _owner.GetComponent<CreatureSkills>();

            // return next state
            return CreatureLogicSM.combatState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
        }
    }
}
