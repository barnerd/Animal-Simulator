using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class MoveToPositionState : State<Creature>
    {
        public MoveToPositionState(CreatureLogicSM _sm) : base("Move to Position", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
            // set walk animation
            //Debug.Log("State: MoveToPosition: " + _owner.name + " is walking to " + _owner.positionTarget);
        }

        public override State<Creature> Execute(Creature _owner)
        {
            // TODO: Check for positionTarget == Vector3.zero

            Vector2 creaturePosition2D = new Vector2(_owner.transform.position.x, _owner.transform.position.z);
            Vector2 targetPosition2D = new Vector2(_owner.positionTarget.x, _owner.positionTarget.z);

            float distance = Vector3.Distance(creaturePosition2D, targetPosition2D);

            // if consumptionTarget.transform is within consumingRange, go to ConsumingState
            if (distance <= _owner.creatureData.consumingRange)
            {
                //Debug.Log("State: MoveToPosition: " + _owner.name + " is moving onto the next state: " + _owner.nextStateAfterMoving);

                return (_owner.nextStateAfterMoving != null) ? _owner.nextStateAfterMoving : CreatureLogicSM.idleState;
            }

            // else, move to target
            //Debug.Log(_owner.name + " is not close enough (" + distance + ") and moving towards: " + _owner.consumptionTarget.name);

            Vector2 moveDirection2D = targetPosition2D - creaturePosition2D;
            Vector3 moveDirection = new Vector3(moveDirection2D.x, 0, moveDirection2D.y);

            _owner.GetComponent<CreatureMotor>().MoveDirection(moveDirection.normalized);

            // return next state
            return CreatureLogicSM.moveToPositionState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
            _owner.GetComponent<CreatureMotor>().MoveDirection(Vector3.zero);
            _owner.positionTarget = Vector3.zero;
            _owner.nextStateAfterMoving = null;
        }
    }
}
