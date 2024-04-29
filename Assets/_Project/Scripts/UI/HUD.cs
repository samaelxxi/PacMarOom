using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    void Awake()
    {
        Game.Instance.SetHUD(this);
    }

    public void SetNewScore(int score)
    {
        Debug.Log("Score: " + score);
    }
}
