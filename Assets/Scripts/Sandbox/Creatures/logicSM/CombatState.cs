using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class CombatState : State<Creature>
    {
        public CombatState(CreatureLogicSM _sm) : base("Combat", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
            Debug.Log("Creature " + _owner.name + " (" + _owner.creatureData.name + ") is entering combat");
        }

        public override State<Creature> Execute(Creature _owner)
        {
            // I know I'm in combat, but I don't know what to do
            // do I have a reason to do something "special",
            // like heal if low health, taunt if ally is low health, etc
            // if not, use creatureData.chances
            // Choose to go to CombatAttack, CombatDefense, CombatSupport, Fleeing
            float totalChance = _owner.creatureData.chanceToAttack + _owner.creatureData.chanceToDefend + _owner.creatureData.chanceToSupport;
            float chance = Random.Range(0, totalChance);

            if (chance < _owner.creatureData.chanceToAttack)
            {
                return CreatureLogicSM.combatAttackState;
            }
            chance -= _owner.creatureData.chanceToAttack;
            if (chance < _owner.creatureData.chanceToDefend)
            {
                return CreatureLogicSM.combatDefendState;
            }
            chance -= _owner.creatureData.chanceToDefend;
            if (chance < _owner.creatureData.chanceToSupport)
            {
                return CreatureLogicSM.combatSupportState;
            }
            chance -= _owner.creatureData.chanceToSupport;

            // return next state
            return CreatureLogicSM.combatState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
            Debug.Log("Creature " + _owner.name + " (" + _owner.creatureData.name + ") is exiting combat");
        }
    }
}
