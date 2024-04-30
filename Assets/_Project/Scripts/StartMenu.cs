using System.Collections;
using System.Collections.Generic;
using CarterGames.Assets.AudioManager;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    [SerializeField] AudioManager _audioManager;

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        _audioManager.Play("intro", volume: 0.5f);
        yield return new WaitForSeconds(4.8f);
        SceneManager.LoadScene("TestLevel1");
    }
}
