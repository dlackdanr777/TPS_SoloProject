
/// <summary> FSM�� ���¸� ������ �������̽� </summary>
public interface IState
{
    /// <summary> �ش� ���°� ���۵Ǹ� �ѹ� �����ϴ� �Լ� </summary>
    public void OnStart();

    /// <summary> �ش� ���°� ����Ǹ� �ѹ� �����ϴ� �Լ� </summary>
    public void OnExit();

    /// <summary> �ش� �����ϰ�� �ֱ������� �����ϴ� �Լ� </summary>
    public void OnUpdate();

    /// <summary> �ش� �����ϰ�� �ֱ������� �����ϴ� �Լ� </summary>
    public void OnFixedUpdate();

    /// <summary> �ش� �����ϰ�� �ֱ������� �����ϸ� ���� ��ȭ�� Ȯ���ϴ� �Լ� </summary>
    public void OnStateUpdate();
}

/// <summary> �÷��̾� FSM�� ��ü ���¸� ������ �������̽� </summary>
public interface IUpperState : IState { }

/// <summary> �÷��̾� FSM�� ��ü ���¸� ������ �������̽� </summary>
public interface ILowerState : IState { }
