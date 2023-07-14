using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class ConsumingState : State<Creature>
    {
        public ConsumingState(CreatureLogicSM _sm) : base("Consuming", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
            // start animation for eating/drink
            _owner.GetComponentInChildren<Animator>().SetBool("isEating", true);
        }

        public override State<Creature> Execute(Creature _owner)
        {
            //Debug.Log(_owner.name + " is consuming " + _owner.consumptionTarget);

            // eat a plant
            if (_owner.consumptionTarget != null)
            {
                _owner.Eat(_owner.consumptionTarget);
                if(_owner.GetComponent<CreatureAttributes>().GetHungerPercent() < 1f && _owner.consumptionTarget.RemainingFood > 0f)
                {
                    return CreatureLogicSM.consumingState;
                }
            }
            // drink from a body of water
            /*else if (_owner.consumptionTarget.TryGetComponent<WaterBody>(out WaterBody _water))
            {
                _owner.Drink(_water);
                if (_owner.GetComponent<CreatureAttributes>().GetThirstPercent() < 1f)
                {
                    return CreatureLogicSM.consumingState;
                }
            }*/

            // return next state
            return CreatureLogicSM.idleState;
        }

        public override void Exit(Creature _owner)
        {
            _owner.consumptionTarget = null;
            // call _owner.function() when leaving this state
            _owner.GetComponentInChildren<Animator>().SetBool("isEating", false);
        }
    }
}
