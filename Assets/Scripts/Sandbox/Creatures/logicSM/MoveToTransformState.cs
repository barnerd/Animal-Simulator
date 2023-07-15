using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Creatures.States
{
    public class MoveToTransformState : State<Creature>
    {
        public MoveToTransformState(CreatureLogicSM _sm) : base("Move to Position", _sm) { }

        public override void Enter(Creature _owner)
        {
            // call _owner.function() when entering this state
            // set walk animation
            //Debug.Log("State: MoveToPosition: " + _owner.name + " is walking to " + _owner.positionTarget);
        }

        public override State<Creature> Execute(Creature _owner)
        {
            if(_owner.moveToTransformTarget == null)
            {
                return (_owner.moveToTransformNextState != null) ? _owner.moveToTransformNextState : CreatureLogicSM.idleState;
            }

            Vector2 creaturePosition2D = new Vector2(_owner.transform.position.x, _owner.transform.position.z);
            Vector2 targetPosition2D = new Vector2(_owner.moveToTransformTarget.position.x, _owner.moveToTransformTarget.position.z);

            float distance = Vector3.Distance(creaturePosition2D, targetPosition2D);

            if (distance <= _owner.moveToTransformClosingDistance)
            {
                //Debug.Log("State: MoveToPosition: " + _owner.name + " is moving onto the next state: " + _owner.nextStateAfterMoving);

                return (_owner.moveToTransformNextState != null) ? _owner.moveToTransformNextState : CreatureLogicSM.idleState;
            }

            // else, move to target
            //Debug.Log(_owner.name + " is not close enough (" + distance + ") and moving towards: " + _owner.consumptionTarget.name);

            Vector2 moveDirection2D = targetPosition2D - creaturePosition2D;
            Vector3 moveDirection = new Vector3(moveDirection2D.x, 0, moveDirection2D.y);

            _owner.GetComponent<CreatureMotor>().MoveDirection(moveDirection.normalized);

            // return next state
            return CreatureLogicSM.moveToTransformState;
        }

        public override void Exit(Creature _owner)
        {
            // call _owner.function() when leaving this state
            _owner.GetComponent<CreatureMotor>().MoveDirection(Vector3.zero);
            _owner.moveToTransformTarget = null;
            _owner.moveToTransformNextState = null;
        }
    }
}
