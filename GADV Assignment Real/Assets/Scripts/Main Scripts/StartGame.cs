///it finds button and call scene :)
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class StartGame : MonoBehaviour
{
    public Button playButton; 

    private void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(LoadScene);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Player1DraftScene");
    }
}

