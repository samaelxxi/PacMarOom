using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PigCatcher : MonoBehaviour, IWeapon
{
    PacmanStats _stats;

    float _cooldownTimer = 0f;

    TriggerObserver _triggerObserver;
    SphereCollider _catchZone;
    SplineAnimate _splineAnimate;

    public void Setup(PacmanStats stats)
    {
        _stats = stats;
    }

    void Awake()
    {
        _triggerObserver = GetComponentInChildren<TriggerObserver>();
        _triggerObserver.SetTestPredicate((Collider other) => other.gameObject.layer == LayerMask.NameToLayer("Pig"));
        _triggerObserver.OnTriggerEnterEvent += OnPiggyCatched;
        _triggerObserver.SetOnlyOnce(true);
        _catchZone = GetComponentInChildren<SphereCollider>();
        _splineAnimate = GetComponentInChildren<SplineAnimate>();
    }

    void OnPiggyCatched(Collider pigCollider)
    {
        Debug.Log("Piggy catched");
        Game.Instance.CatchPig();
        if (pigCollider.transform.parent.TryGetComponent(out FlyingPig pig))
        {
            pig.BeCatched();
        }
    }

    public void TryShoot()
    {
        if (_cooldownTimer > 0)
            return;

        _cooldownTimer = _stats.PigCatchCooldown;
        StartCatch();
    }

    void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }

    void StartCatch()
    {
        _triggerObserver.Reset();
        _catchZone.gameObject.SetActive(true);
        _splineAnimate.Duration = _stats.CatchDuration;
        _splineAnimate.Restart(autoplay: true);
        Game.Instance.AudioManager.PlayRange(Sounds.Net, pitch: UnityEngine.Random.Range(0.9f, 1.1f), volume: 0.7f);
        this.InSeconds(_stats.CatchDuration, () => _catchZone.gameObject.SetActive(false));
        this.InSeconds(_stats.CatchDuration, () => _splineAnimate.Restart(autoplay: false));
    }

    public void Equip()
    {
        _catchZone.gameObject.SetActive(false);
        _splineAnimate.Duration = _stats.CatchDuration;
        _splineAnimate.Restart(autoplay: false);
    }
}
