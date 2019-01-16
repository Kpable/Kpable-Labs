using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kpable.AI;
using Kpable.AI.FSM;


public class PetRobotEkko : BaseGameEntity
{
    public delegate void OutputText(int id, string text);
    public event OutputText OutText;

    StateMachine<PetRobotEkko> stateMachine;

    public bool Sleeping { get; set; }

    public PetRobotEkko(int id) : base(id)
    {
        stateMachine = new StateMachine<PetRobotEkko>(this);

        stateMachine.CurrentState = AmuseOwner.Instance;
        stateMachine.GlobalState = RobotGlobalState.Instance;

        Sleeping = false;
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

    public override bool HandleMessage(Telegram message)
    {
        return stateMachine.HandleMessage(message);
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

    public override bool OnMessage(PetRobotEkko owner, Telegram message)
    {
        return false;
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

    public override bool OnMessage(PetRobotEkko owner, Telegram message)
    {
        return false;
    }
}

public class PrimeDevices : SingletonState<PrimeDevices, PetRobotEkko>
{
    public override void Enter(PetRobotEkko owner)
    {
        owner.Output("Welcome home human companion");
        MessageDispatcher.Instance.DispatchMessage(owner.ID, owner.ID, (int) MessageType.Msg_DevicesPrimed, 1.5f);
        owner.Output("I shall prime your devices");

    }

    public override void Execute(PetRobotEkko owner)
    {
    }

    public override void Exit(PetRobotEkko owner)
    {
        owner.Output("All devices are primed.");
    }

    public override bool OnMessage(PetRobotEkko owner, Telegram message)
    {
        bool messageHandled = false;

        switch ((MessageType)message.MessageType)
        {
            case MessageType.Msg_DevicesPrimed:

                MessageDispatcher.Instance.DispatchMessage(owner.ID, (int)Entity.Soft_Eng, (int)MessageType.Msg_DevicesPrimed);

                owner.GetFSM().ChangeState(AmuseOwner.Instance);
                messageHandled = true;
                break;
            default:
                messageHandled = false;
                break;
        }

        return messageHandled;
    }
}

public class Sleep : SingletonState<Sleep, PetRobotEkko>
{
    public override void Enter(PetRobotEkko owner)
    {
        owner.Output("Entering Standby Mode");
        owner.Sleeping = true;
    }

    public override void Execute(PetRobotEkko owner)
    {
    }

    public override void Exit(PetRobotEkko owner)
    {

    }

    public override bool OnMessage(PetRobotEkko owner, Telegram message)
    {
        return false;
    }
}


public class RobotGlobalState : SingletonState<RobotGlobalState, PetRobotEkko>
{
    public override void Enter(PetRobotEkko owner)
    {
    }

    public override void Execute(PetRobotEkko owner)
    {
        if (Random.value < 0.1f && !owner.Sleeping) owner.GetFSM().ChangeState(ComputateLife.Instance);
    }

    public override void Exit(PetRobotEkko owner)
    {

    }

    public override bool OnMessage(PetRobotEkko owner, Telegram message)
    {
        bool messageHandled = false;

        switch((MessageType)message.MessageType)
        {
            case MessageType.Msg_EkkoImHome:
                owner.GetFSM().ChangeState(PrimeDevices.Instance);
                messageHandled = true;
                break;
            case MessageType.Msg_SleepRobot:
                owner.Output("Very well Human Companion.");
                owner.GetFSM().ChangeState(Sleep.Instance);
                messageHandled = true;
                break;
            default:
                messageHandled = false;
                break;
        }

        return messageHandled;
    }
}


