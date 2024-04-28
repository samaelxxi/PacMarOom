using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Singleton;
using UnityEngine;

public class Game : Singleton<Game>
{
    public Pacman Pacman => _pacman;

    Pacman _pacman;
    HUD _hud;
    Level _level;

    public void SetPacman(Pacman pacman) => _pacman = pacman;
    public void SetHUD(HUD hud) => _hud = hud;
    public void SetLevel(Level level) => _level = level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RespawnPacman()
    {
        if (_level == null)
        {
            Debug.LogError("Create Level and setup spawn point");
            return;
        }

        _pacman.Respawn(_level.PlayerSpawnPoint);
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        _level.SetNewCheckpoint(checkpoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
