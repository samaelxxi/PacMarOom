using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Final : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _pigText;
    [SerializeField] List<GameObject> _pigs;

    List<int> _pigColors = new List<int>();

    int GetEndPigs()
    {
        _pigColors = Game.Instance.Level.PigColors;
        return 3;
    }

    void Start()
    {
        GetEndPigs();
        StartCoroutine(EnablePigs());
    }

    IEnumerator EnablePigs()
    {
        int i = 0;
        foreach (var pig in _pigColors)
        {
            _pigText.text = $"{i}/3";
            yield return new WaitForSeconds(2f);
            _pigs[pig].SetActive(true);
            i++;
        }
        _pigText.text = $"{i}/3";
    }
}
