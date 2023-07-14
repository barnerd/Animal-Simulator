using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class IdleState : State<Creature>
    {
        public IdleState(CreatureLogicSM _sm) : base("Idle", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
            // set idle animation
        }

        public override State<Creature> Execute(Creature _owner)
        {
            // if x time has past, switch to idle2 animation


            CreatureAttributes attributes = _owner.GetComponent<CreatureAttributes>();

            float currentHunger = attributes.GetHungerPercent();
            float currentThirst = attributes.GetThirstPercent();

            // if hunger, thirst, threat, or mate, set target and mode, then return CreatureLogicSM.searchingState;
            if (currentHunger < ((AIController)_owner.currentController).hungerSearchThreshold)
            {
                //Debug.Log(_owner.name + " is now hungry: " + currentHunger);

                IFood target = _owner.FindClosestFood(((AIController)_owner.currentController).remainingFoodPercent * _owner.creatureData.eatRate);
                if (target != null)
                {
                    //Debug.Log("Check: next food target is: " + target);
                    _owner.consumptionTarget = target;
                    _owner.nextStateAfterMoving = CreatureLogicSM.consumingState;

                    _owner.positionTarget = target.Position;
                    return CreatureLogicSM.moveToPositionState;
                }
                else
                {
                    // Try blinking
                    //_owner.Blink();
                }
            }
            if (currentThirst < ((AIController)_owner.currentController).thirstSearchThreshold)
            {
                //Debug.Log(_owner.name + " is now thirsty: " + currentThirst);

                //_owner.consumptionTarget = target;
                _owner.nextStateAfterMoving = CreatureLogicSM.consumingState;

                //_owner.positionTarget = target.Position;
                return CreatureLogicSM.moveToPositionState;
            }

            // if nothing else, wander around a bit (add wander state)
            // stand there for a while, then move to a random spot
            if (Time.time > _owner.nextTimeForAIUpdate)
            {
                _owner.nextTimeForAIUpdate = Time.time + ((AIController)_owner.currentController).updateInterval;

                // wander for food
                float x = Random.Range(-_owner.creatureData.sightRadius, _owner.creatureData.sightRadius);
                float z = Random.Range(-_owner.creatureData.sightRadius, _owner.creatureData.sightRadius);
                _owner.positionTarget = _owner.transform.position + new Vector3(x, 0, z);
                return CreatureLogicSM.moveToPositionState;
            }

            // return next state
            return CreatureLogicSM.idleState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
        }
    }
}
