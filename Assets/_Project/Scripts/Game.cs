using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Singleton;
using UnityEngine;

public class Game : Singleton<Game>
{

    public Pacman Pacman => _pacman;

    Pacman _pacman;
    HUD _hud;

    public void SetPacman(Pacman pacman) => _pacman = pacman;
    public void SetHUD(HUD hud) => _hud = hud;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
