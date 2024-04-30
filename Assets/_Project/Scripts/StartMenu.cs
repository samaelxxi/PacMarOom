using System.Collections;
using System.Collections.Generic;
using CarterGames.Assets.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    [SerializeField] AudioManager _audioManager;

    bool _isGameStarted = false;

    public void StartGame()
    {
        if (_isGameStarted)
            return;
        _isGameStarted = true;
        StartCoroutine(StartGameCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            StartGame();
    }

    IEnumerator StartGameCoroutine()
    {
        _audioManager.Play("intro", volume: 0.5f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TestLevel1");
        asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(4.7f);
        // SceneManager.LoadSceneAsync("TestLevel1", LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = true;
    }
}
