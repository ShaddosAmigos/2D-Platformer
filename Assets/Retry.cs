using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public string sceneName;
    public void OpenScene ()
    {
        SceneManager.LoadScene(sceneName);
    }
}
