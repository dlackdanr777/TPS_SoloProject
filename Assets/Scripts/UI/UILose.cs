using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILose : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() => GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name));
        _exitButton.onClick.AddListener(() => Debug.Log("´­¸²"));
    }
}
