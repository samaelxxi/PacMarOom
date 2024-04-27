using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pacman : MonoBehaviour
{
    PacmanController _controller;

    void Awake()
    {
        _controller = GetComponent<PacmanController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
