
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonHandler<GameManager>
{

    public bool IsGameEnd
    {
        get { return _isGameEnd; }
        set { _isGameEnd = value; }
    }

    public Player Player;

    private bool _isGameEnd;
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
    }

    public void GameEnd()
    {
        CursorVisible();
        _isGameEnd = true;
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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        CursorVisible();
    }
}
