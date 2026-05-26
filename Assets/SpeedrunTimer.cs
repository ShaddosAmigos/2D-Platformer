using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Wymagane, jeœli u¿ywasz TextMeshPro do wyœwietlania czasu

public class SpeedrunTimer : MonoBehaviour
{
    public static SpeedrunTimer instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText; // Przeci¹gnij tutaj swój obiekt tekstowy

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        // Singleton - upewniamy siê, ¿e istnieje tylko jeden taki obiekt w grze
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // To sprawia, ¿e licznik nie znika miêdzy scenami!
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Subskrypcja zdarzenia zmiany sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Odsubskrybowanie zdarzenia (dobra praktyka programistyczna)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sprawdzamy nazwê sceny
        if (scene.name == "Level1")
        {
            // Resetujemy czas i odpalamy licznik, gdy gracz zaczyna od Level1
            elapsedTime = 0f;
            isRunning = true;
        }
        else if (scene.name == "EndLevel")
        {
            // Zatrzymujemy licznik na scenie koñcowej
            isRunning = false;
        }
        // Jeœli scena to coœ pomiêdzy (np. Level2, Level3), licznik po prostu dzia³a dalej
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Formatowanie czasu do postaci MM:SS.ms (Minuty:Sekundy.Milisekundy)
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);

            timerText.text = string.Format("{0:00}:{1:00}.{2:03}", minutes, seconds, milliseconds);
        }
    }
}