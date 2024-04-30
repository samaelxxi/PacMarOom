using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CarterGames.Assets.AudioManager;

public class Final : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _pigText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _collectibleText;
    [SerializeField] List<GameObject> _pigs;
    AudioManager _audioManager;

    List<int> _pigColors = new List<int>();

    int GetEndPigs()
    {
        _pigColors = Game.Instance.Level.PigColors;
        // _pigColors = new List<int> { 0, 1, 2 };
        return 3;
    }

    void Start()
    {
        _scoreText.text = Game.Instance.EndScore.ToString();
        _collectibleText.text = Game.Instance.EndCoins.ToString();
        GetEndPigs();
        StartCoroutine(EnablePigs());
        _audioManager = Game.Instance.AudioManager;
    }

    IEnumerator EnablePigs()
    {
        _pigText.text = "";
        _scoreText.text = "";
        _collectibleText.text = "";

        float scoreT = 2;
        float shotTimer = 0;
        float dScore = Game.Instance.EndScore;
        float currentScore = 0;
        float totalTimer = 0;

        yield return new WaitForSeconds(1.5f);
        while (totalTimer < scoreT)
        {
            totalTimer += Time.deltaTime;
            currentScore += Time.deltaTime * dScore / scoreT;
            currentScore = Mathf.Min(currentScore, dScore);
            _scoreText.text = ((int)currentScore).ToString();
            shotTimer += Time.deltaTime;
            if (shotTimer > 0.085f)
            {
                _audioManager.Play("endShot");
                shotTimer = 0;
            }
            yield return new WaitForEndOfFrame();
        }
        _audioManager.Play("endSound");

        yield return new WaitForSeconds(0.75f);

        float currentCoins = 0;
        float dCoins = Game.Instance.EndCoins;
        int totalCoins = Game.Instance.TotalCoins;
        // totalCoins = 120;
        totalTimer = 0;
        shotTimer = 0;
        while (totalTimer < scoreT)
        {
            totalTimer += Time.deltaTime;
            currentCoins += Time.deltaTime * dCoins / scoreT;
            currentCoins = Mathf.Min(currentCoins, dCoins);
            _collectibleText.text = $"{((int)currentCoins).ToString()}/{totalCoins}";
            shotTimer += Time.deltaTime;
            if (shotTimer > 0.085f)
            {
                _audioManager.Play("endShot");
                shotTimer = 0;
            }
            yield return new WaitForEndOfFrame();
        }
        _audioManager.Play("endSound");


        yield return new WaitForSeconds(0.75f);


        int i = 0;
        foreach (var pig in _pigColors)
        {
            _pigText.text = $"{i}/3";
            yield return new WaitForSeconds(1.5f);
            _audioManager.Play("endSound");
            _pigs[pig].SetActive(true);
            i++;
        }
        _pigText.text = $"{i}/3";
    }
}
