
using UnityEngine;

public interface Iinteractive 
{
    KeyCode InputKey { get; }
    void Interact();
    void EnableInteraction();

    void DisableInteraction();
}
