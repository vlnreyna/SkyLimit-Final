using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("Cerrando juego...");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
