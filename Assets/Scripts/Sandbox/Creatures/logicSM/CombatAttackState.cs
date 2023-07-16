using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class CombatAttackState : State<Creature>
    {
        public CombatAttackState(CreatureLogicSM _sm) : base("Combat Attack", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
            Debug.Log("Creature " + _owner.name + " (" + _owner.creatureData.name + ") is entering combat attack");
        }

        public override State<Creature> Execute(Creature _owner)
        {
            CreatureCombat creatureCombat = _owner.GetComponent<CreatureCombat>();
            CreatureSkills creatureSkills = _owner.GetComponent<CreatureSkills>();

            /*
             * check if should flee, then return CreatureLogicSM.fleeingState;
             * 
             * check abilities and pick one base on distance with target
             * 
             * if not close enough for ability, move within distance
             * 
             */

            float distanceToTarget = Vector3.Distance(_owner.transform.position, creatureCombat.Target.Transform.position);
            float maxDistance = 0f;
            Debug.Log("Creature " + _owner.name + " (" + _owner.creatureData.name + ") is targetting " + creatureCombat.Target.Transform.name + " (" + creatureCombat.Target.Transform.GetComponent<Creature>().creatureData.name + "), " + distanceToTarget + "m away");

            List<Skills.AttackAbilityData> abilitiesAtDistance = new List<Skills.AttackAbilityData>();

            foreach (var ability in creatureSkills.AttackAbilities)
            {
                if (ability.distance > maxDistance)
                {
                    maxDistance = ability.distance;
                }
                if (ability.distance > distanceToTarget)
                {
                    // viable ability
                    abilitiesAtDistance.Add(ability);
                }
            }

            if (abilitiesAtDistance.Count == 0)
            {
                // move closer
                _owner.PrepareMoveToTransform(creatureCombat.Target.Transform, maxDistance, CreatureLogicSM.combatAttackState);
                return CreatureLogicSM.moveToTransformState;
            }
            else
            {
                // randomly pick ability
                int chance = Random.Range(0, abilitiesAtDistance.Count);
                Skills.AttackAbilityData attack = abilitiesAtDistance[chance];

                // perform ability
                creatureCombat.CurrentCombat.ExecuteAttackAbility(_owner, attack);
            }


            // creature.PrepareMoveToTransform(c.transform, meleeDistance, CreatureLogicSM.combatState);

            // If combat is over, go to IdleState

            // return next state
            return CreatureLogicSM.combatState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
            Debug.Log("Creature " + _owner.name + " (" + _owner.creatureData.name + ") is exiting combat attack");
        }
    }
}
