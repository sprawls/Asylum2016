using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressButtonToContinue : MonoBehaviour
{
    [SerializeField] private KeyCode _keycode;
    [SerializeField] private string _levelToLoad;

    [UsedImplicitly]
    private void Update()
    {
        if (Input.GetKeyDown(_keycode))
        {
            SceneManager.LoadScene(_levelToLoad);
        }
    }
}