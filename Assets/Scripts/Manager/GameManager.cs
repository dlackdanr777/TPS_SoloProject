
using UnityEngine;
public class GameManager : SingletonHandler<GameManager>
{
    public Player Player;

    public ObstacleBuild ob;

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
            Player.DepleteHp(Player, 10);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ObjectPoolManager.Instance.SpawnZombie(ZombieType.Basic, new Vector3(10, 2.6f, 73), Quaternion.identity);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            ObjectPoolManager.Instance.SpawnZombie(ZombieType.Women, new Vector3(10, 2.6f, 75f), Quaternion.identity);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            ob.ShowObstacle();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            ob.HiddenObstacle();
        if (Input.GetKeyDown(KeyCode.Alpha7))
            ob.BuildObstacle();

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
