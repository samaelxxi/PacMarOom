using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent _onTriggerEnter;

    [SerializeField] LayerMask _layerMask;

    void OnTriggerEnter(Collider other)
    {
        if ((_layerMask.value & 1 << other.gameObject.layer) != 0)
            _onTriggerEnter.Invoke();
    }

    public void PacmanRespawn()
    {
        Game.Instance.RespawnPacman();
    }

    public void SetNewCheckpoint(Transform checkpoint)
    {
        Game.Instance.SetNewCheckpoint(checkpoint);
    }
}
