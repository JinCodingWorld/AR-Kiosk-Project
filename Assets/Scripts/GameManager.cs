using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    AudioSource audioPlay;
    private void Start()
    {
        audioPlay = GetComponent<AudioSource>();
    }
    public void sceneTransition(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void gameQuit()
    {
        Application.Quit();
    }

    public void musicPlay()
    {
        audioPlay.Play();
    }


}
