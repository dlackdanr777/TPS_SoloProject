
public interface IState
{
    public void OnStart();
    public void OnExit();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void OnStateUpdate();
}

public interface IUpperState : IState
{

}

public interface ILowerState : IState
{

}
