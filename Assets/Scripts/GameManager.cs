using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia.Internal.NativeBridge;

public class GameManager : MonoBehaviour
{
    AudioSource audioPlay;
    public GameObject introText;
    private void Start()
    {
        audioPlay = GetComponent<AudioSource>();
        StartCoroutine(deleteIntro());
    }

    IEnumerator deleteIntro()
    {
        while (true)
        {
            float waitTime = 3f;
            yield return new WaitForSeconds(waitTime);

            introText.SetActive(false);
        }
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
