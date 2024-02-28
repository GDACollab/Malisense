using UnityEngine;

public class SBProtoStateMachine : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Alert,
        Chasing
    }

    public State currentState;

    public StateBaseClass patrol;
    public StateBaseClass chasing;
    public StateBaseClass alert;

    void Start()
    {
        SwitchState(currentState);
    }

    public void SwitchState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Patrolling:
                if (patrol) patrol.Init();
                break;

            case State.Alert:
                if (alert) alert.Init();
                break;

            case State.Chasing:
                if (chasing) chasing.Init();
                break;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Patrolling:
                if (patrol) patrol.On_Update();
                break;

            case State.Alert:
                if (alert) alert.On_Update();
                break;

            case State.Chasing:
                if (chasing) chasing.On_Update();
                break;
        }
    }
}
