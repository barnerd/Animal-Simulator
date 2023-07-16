using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Creatures.States;

public class CreatureLogicSM : StateMachine<Creature>
{
    public static IdleState idleState;
    public static WanderState wanderState;
    public static ConsumingState consumingState;
    public static MoveToTransformState moveToTransformState;
    public static FleeingState fleeingState;
    public static MatingState matingState;
    public static BirthingState birthingState;
    public static CombatState combatState;
    public static CombatAttackState combatAttackState;
    public static CombatDefendState combatDefendState;
    public static CombatSupportState combatSupportState;

    public override void Initialize(Creature _owner)
    {
        if (idleState == null) idleState = new IdleState(this);
        if (wanderState == null) wanderState = new WanderState(this);
        if (consumingState == null) consumingState = new ConsumingState(this);
        if (moveToTransformState == null) moveToTransformState = new MoveToTransformState(this);
        if (fleeingState == null) fleeingState = new FleeingState(this);
        if (matingState == null) matingState = new MatingState(this);
        if (birthingState == null) birthingState = new BirthingState(this);
        if (combatState == null) combatState = new CombatState(this);
        if (combatAttackState == null) combatAttackState = new CombatAttackState(this);
        if (combatDefendState == null) combatDefendState = new CombatDefendState(this);
        if (combatSupportState == null) combatSupportState = new CombatSupportState(this);

        base.Initialize(_owner);
    }

    protected override State<Creature> GetInitialState()
    {
        return idleState;
    }
}
