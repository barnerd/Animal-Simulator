using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Creatures.States;

public class CreatureLogicSM : StateMachine<Creature>
{
    public static IdleState idleState;
    public static ConsumingState consumingState;
    public static MoveToPositionState moveToPositionState;
    public static FleeingState fleeingState;
    public static MatingState matingState;
    public static BirthingState birthingState;
    public static CombatState combatState;

    public override void Initialize(Creature _owner)
    {
        if (idleState == null) idleState = new IdleState(this);
        if (consumingState == null) consumingState = new ConsumingState(this);
        if (moveToPositionState == null) moveToPositionState = new MoveToPositionState(this);
        if (fleeingState == null) fleeingState = new FleeingState(this);
        if (matingState == null) matingState = new MatingState(this);
        if (birthingState == null) birthingState = new BirthingState(this);
        if (combatState == null) combatState = new CombatState(this);

        base.Initialize(_owner);
    }

    protected override State<Creature> GetInitialState()
    {
        return idleState;
    }
}
