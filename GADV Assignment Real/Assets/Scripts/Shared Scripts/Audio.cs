///background music yes yes. also kills itself upon entering the GameScene cuz i have difference music for it in a different audiosource.
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPersistUntilGame : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            Destroy(gameObject);
        }
    }
}

