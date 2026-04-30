using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string sceneName;
    public void OpenScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
