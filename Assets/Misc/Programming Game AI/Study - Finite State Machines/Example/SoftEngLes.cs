using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.AI.FSM;
using System;

public enum LocationType { Home, Office, Break_Room, Wegs }



public class SoftEngLes : BaseGameEntity {

    public delegate void OutputText(int id, string text);
    public event OutputText OutText;

    public static int HoursBeforeBreak = 8;
    public static int HoursAtATime = 3;
    public static int CaffieneNeed = 5;
    public static int TirednessThreshold = 5;
    public static int TotalHoursToWork = 40;

    StateMachine<SoftEngLes> stateMachine;
    LocationType location = LocationType.Home;
    int hoursClocked = 0;
    int hoursInLog = 0;
    int caffiene = 0;
    int tiredness = 0;

    public LocationType Location { get { return location; } set { location = value; } }

    public int HoursClocked { get { return hoursClocked; } set { hoursClocked = value; } }
    public int HoursInLog { get { return hoursInLog; } set { hoursInLog = value; } }
    public int Tiredness { get { return tiredness; } set { tiredness = value; } }
    public int Caffiene { get { return caffiene; } set { caffiene = value; } }
    
    public bool NeedABreak { get { return hoursClocked >= HoursAtATime; } }
    public bool Tired { get { return tiredness > TirednessThreshold; } }
    public bool NeedCoffee { get { return caffiene >= CaffieneNeed; } }
    public bool WorkedEnoughForToday { get { return hoursInLog >= HoursBeforeBreak; } }
    public bool WorkedEnough { get { return HoursInLog >= TotalHoursToWork; } }


    public SoftEngLes(int id) : 
        base(id)
    {
        stateMachine = new StateMachine<SoftEngLes>(this);

        stateMachine.CurrentState = GoHomeAndRest.Instance;
        stateMachine.GlobalState = SoftEngGlobalState.Instance;

    }

    public StateMachine<SoftEngLes> GetFSM()
    {
        return stateMachine;
    }

    public void Update()
    {
        stateMachine.Update();
    }

    public void Output(string text)
    {
        if(OutText != null) OutText(ID, text);
    }

    public override bool HandleMessage(Telegram message)
    {
        return stateMachine.HandleMessage(message);
    }
}

public class GoToOfficeAndWork : SingletonState<GoToOfficeAndWork, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
        // If not at work go to work
        if (owner.Location != LocationType.Office)
        {
            owner.Output("Back To Work");
            owner.Location = LocationType.Office;
        }
    }

    public override void Execute(SoftEngLes owner)
    {
        // While at work, work some hours. 

        owner.HoursClocked++;
        owner.Tiredness++;

        owner.Output("Writing some code, making some bugs, fixing some bugs");

        if (owner.NeedABreak)
        {
            owner.GetFSM().ChangeState(LogHours.Instance);
        }

        if (owner.NeedCoffee)
        {
            owner.GetFSM().ChangeState(GetCoffee.Instance);
        }

    }

    public override void Exit(SoftEngLes owner)
    {
        owner.Output("Gotta step out and take a break");
    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {
        return false;
    }
}

public class GoHomeAndRest : SingletonState<GoHomeAndRest, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
        if (owner.Location != LocationType.Home)
        {
            owner.Output("Gotta go home");
            owner.Location = LocationType.Home;

            MessageDispatcher.Instance.DispatchMessage(owner.ID, (int)Entity.Pet_Robot, (int)MessageType.Msg_EkkoImHome);
        }
    }

    public override void Execute(SoftEngLes owner)
    {
        if (owner.Tired)
        {
            owner.Output("So tired must sleep");
            owner.Tiredness--;
        }
        else
        {
            owner.Output("Thats a good quick gaming session, time to go back to work");
            owner.GetFSM().ChangeState(GoToOfficeAndWork.Instance);
        }
    }

    public override void Exit(SoftEngLes owner)
    {
        owner.Output("Leaving Home");
    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {

        // Theres a chance this only triggers when the agent is sleeping here. If its not, message will be missed.
        // This adds an odd connection between the delay the other agent(petrobotekko) uses for its self message 
        // and the duration this agent (softengles) is sleeping for. 
        // If the message does not get handled here in this case, it could be handled in the global state as defined 
        // in the state machine class.

        // Debug proved the above to be true

        bool messageHandled = false;

        switch ((MessageType)message.MessageType)
        {
            case MessageType.Msg_DevicesPrimed:
                owner.Output("Wonderful! Thanks!");

                owner.GetFSM().ChangeState(UseDevices.Instance);
                messageHandled = true;
                break;
            default:
                messageHandled = false;
                break;
        }

        return messageHandled;
    }
}

public class GetCoffee : SingletonState<GetCoffee, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
        if (owner.Location != LocationType.Wegs)
        {
            owner.Output("Gonna get some coffee");
            owner.Location = LocationType.Wegs;
        }
    }

    public override void Execute(SoftEngLes owner)
    {
        if (owner.NeedCoffee)
        {
            owner.Output("Good ol' cup a joe");
            owner.Caffiene = 0;
        }
        else
            owner.Output("I dont want coffee, why am i here");
    }

    public override void Exit(SoftEngLes owner)
    {
        owner.Output("Heading out of Wegs");
    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {
        return false;
    }
}

public class LogHours : SingletonState<LogHours, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
        if (owner.Location != LocationType.Break_Room)
        {
            owner.Output("Gonna log my hours so far");
            owner.Location = LocationType.Break_Room;
        }
    }

    public override void Execute(SoftEngLes owner)
    {
        owner.HoursInLog += owner.HoursClocked;
        owner.HoursClocked = 0;
        owner.Output("Logging my hours so far. Total Hours: " + owner.HoursInLog);

        if(owner.WorkedEnough)
        {
            owner.Output("A week's work, time for the weekend!");
            owner.GetFSM().ChangeState(Weekend.Instance);

        }
        else if(owner.WorkedEnoughForToday)
        {
            owner.Output("A good day's work, time to head home");
            owner.GetFSM().ChangeState(GoHomeAndRest.Instance);
        }
        else
        {
            owner.GetFSM().ChangeState(GoToOfficeAndWork.Instance);
        }
    }

    public override void Exit(SoftEngLes owner)
    {
        owner.Output("Alright, enough logging");
    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {
        return false;
    }
}


public class SoftEngGlobalState : SingletonState<SoftEngGlobalState, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
    }

    public override void Execute(SoftEngLes owner)
    {
    }

    public override void Exit(SoftEngLes owner)
    {
    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {
        bool messageHandled = false;

        //switch ((MessageType)message.MessageType)
        //{
        //    case MessageType.Msg_DevicesPrimed:
        //        owner.Output("Wonderful! Thanks!");

        //        owner.GetFSM().ChangeState(UseDevices.Instance);
        //        messageHandled = true;
        //        break;
        //    default:
        //        messageHandled = false;
        //        break;
        //}

        return messageHandled;
    }
}

public class UseDevices : SingletonState<UseDevices, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
        owner.Output("Now which device shall i mess around with today?");
    }

    public override void Execute(SoftEngLes owner)
    {
        owner.Output("When in doubt, yes! I shall use them all!");
        owner.GetFSM().RevertToPreviousState();
    }

    public override void Exit(SoftEngLes owner)
    {
        owner.Output("Oh right i have other things to do.");

    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {
        return false;
    }
}

public class Weekend : SingletonState<Weekend, SoftEngLes>
{
    public override void Enter(SoftEngLes owner)
    {
        owner.Output("It's the weekend now Ekko, time to sleep buddy");

        MessageDispatcher.Instance.DispatchMessage(owner.ID, (int)Entity.Pet_Robot, (int)MessageType.Msg_SleepRobot);
    }

    public override void Execute(SoftEngLes owner)
    {

    }

    public override void Exit(SoftEngLes owner)
    {
    }

    public override bool OnMessage(SoftEngLes owner, Telegram message)
    {
        return false;
    }
}