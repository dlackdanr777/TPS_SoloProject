using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : SingletonHandler<GameManager>
{
    public Player Player;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 60;
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            Player.Inventory.AddItemByID(1, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Player.HpDepleteHp(Player, 10);
    }

    public void CursorVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorHidden()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
