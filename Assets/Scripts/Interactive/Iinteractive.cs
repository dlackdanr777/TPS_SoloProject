using UnityEngine;


/// <summary> ��ȣ�ۿ��� ���� �������̽� </summary>
public interface Iinteractive 
{
    /// <summary> �ش� Ű�� ������ ��ȣ�ۿ��� ���� </summary>
    KeyCode InputKey { get; }

    /// <summary> ��ȣ�ۿ������ ����Ǵ� �Լ� </summary>
    void Interact();

    /// <summary> ��ȣ�ۿ��� �����Ҷ� ����Ǵ� �Լ� </summary>
    void EnableInteraction();

    /// <summary> ��ȣ�ۿ��� �Ұ����Ҷ� ����Ǵ� �Լ� </summary>
    void DisableInteraction();
}
