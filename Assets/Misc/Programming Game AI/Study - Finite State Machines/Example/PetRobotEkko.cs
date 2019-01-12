using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kpable.AI.FSM;

public class PetRobotEkko : BaseGameEntity
{
    public delegate void OutputText(int id, string text);
    public event OutputText OutText;

    StateMachine<PetRobotEkko> stateMachine;

    public PetRobotEkko(int id) : base(id)
    {
        stateMachine = new StateMachine<PetRobotEkko>(this);

        stateMachine.SetCurrentState(RobotGlobalState.Instance);
        stateMachine.SetGlobalState(AmuseOwner.Instance);
    }

    public StateMachine<PetRobotEkko> GetFSM()
    {
        return stateMachine;
    }

    public void Update()
    {
        stateMachine.Update();
    }

    public void Output(string text)
    {
        if (OutText != null) OutText(ID, text);
    }
}

public class ComputateLife : SingletonState<ComputateLife, PetRobotEkko>
{
    public override void Enter(PetRobotEkko owner)
    {
        owner.Output("I... am.. a... Robot?");
    }

    public override void Execute(PetRobotEkko owner)
    {
        owner.Output("What does it mean to be a Robot?");
        owner.GetFSM().RevertToPreviousState();
    }

    public override void Exit(PetRobotEkko owner)
    {
        owner.Output("SpawnThread(ComputateLife);");
    }
}

public class AmuseOwner : SingletonState<AmuseOwner, PetRobotEkko>
{
    public override void Enter(PetRobotEkko owner)
    {
    }

    public override void Execute(PetRobotEkko owner)
    {
        owner.Output("I am a robot, are you amused master?");
    }

    public override void Exit(PetRobotEkko owner)
    {
    }
}

public class RobotGlobalState : SingletonState<RobotGlobalState, PetRobotEkko>
{
    public override void Enter(PetRobotEkko owner)
    {
    }

    public override void Execute(PetRobotEkko owner)
    {
        if (Random.value < 0.1f) owner.GetFSM().ChangeState(ComputateLife.Instance);
    }

    public override void Exit(PetRobotEkko owner)
    {
    }
}


