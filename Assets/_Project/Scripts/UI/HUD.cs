using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] TextMeshProUGUI _worldText;
    [SerializeField] TextMeshProUGUI _pigsText;
    [SerializeField] Image _overlay;
    [SerializeField] Image _blinkingOverlay;
    [SerializeField] Transform _head;


    float _levelStartTime;
    int _curSeconds;

    void Awake()
    {
        Game.Instance.SetHUD(this);
    }

    void Start()
    {
        _levelStartTime = Time.time;
        _worldText.text = Game.Instance.Level.WorldName;
        SetNewScore(0);
        SetNewPigs(0);
        StartCoroutine(MoveHead());
    }

    void Update()
    {
        float time = Time.time - _levelStartTime;
        if ((int)time != _curSeconds)
        {
            _curSeconds = (int)time;
            _timeText.text = ((int)time).ToString();
        }
    }

    IEnumerator MoveHead()
    {
        while (true)
        {
            Vector3 newRotation = new(Random.Range(-10, 10), -180 + Random.Range(-10, 10), Random.Range(-5, 5));
            _head.DORotate(newRotation, 3f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(2f);
        }
    }

    public void SetNewScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void SetNewWieirdAmmo(int ammo)
    {
        _ammoText.text = ammo.ToString();
    }

    public void SetNewWorld(string world)
    {
        _worldText.text = world;
    }

    public void SetNewPigs(int pigs)
    {
        _pigsText.text = pigs.ToString();
    }

    public void SetNewHealth(int health)
    {
        _healthText.text = health.ToString();
    }

    public void GetDamaged()
    {
        StartCoroutine(DamageOverlay());
    }

    public void GetBonus()
    {
        StartCoroutine(BonusOverlay());
    }

    public void GetPig()
    {
        StartCoroutine(PigOverlay());
    }

    public void GetHeal()
    {
        StartCoroutine(HealOverlay());
    }


    Tween _blinkTween;
    public void OnInvulnerable()
    {
        Debug.Log("On invulnerable");
        _blinkingOverlay.enabled = true;
        _blinkTween = _blinkingOverlay.DOFade(0.25f, 0.25f).From(0).SetLoops(-1, LoopType.Yoyo).SetDelay(0.5f);
    }

    public void OnVulnerable()
    {
        Debug.Log("On vulnerable");
        _blinkingOverlay.enabled = false;
        _blinkTween.Kill();
    }

    enum OverlayType
    {
        Damage, Bonus, Pig, Heal
    }

    Queue<OverlayType> _overlayQueue = new();
    bool _isOverlayActive;
    IEnumerator ShowOverlay(Color color)
    {
        // Debug.Log("Show overlay");
        if (_isOverlayActive)
        {
            _overlayQueue.Enqueue(OverlayType.Damage);
            yield break;
        }
        // Debug.Log($"Show overlay {color}");

        _isOverlayActive = true;
        _overlay.color = color;
        _overlay.enabled = true;
        yield return new WaitForSeconds(0.2f);
        _overlay.enabled = false;
        // Debug.Log("Overlay disabled");

        if (_overlayQueue.Count > 0)
        {
            // Debug.Log("Next overlay");
            yield return new WaitForSeconds(0.2f);
            var overlay = _overlayQueue.Dequeue();
            if (overlay == OverlayType.Damage)
                yield return DamageOverlay();
            else if (overlay == OverlayType.Bonus)
                yield return BonusOverlay();
            else if (overlay == OverlayType.Pig)
                yield return PigOverlay();
            else if (overlay == OverlayType.Heal)
                yield return HealOverlay();
        }
        else
        {
            // Debug.Log("Overlay queue empty");
            _isOverlayActive = false;
        }
    }

    IEnumerator DamageOverlay()
    {
        yield return ShowOverlay(_damageColor);
    }

    IEnumerator BonusOverlay()
    {
        yield return ShowOverlay(_bonusColor);
    }

    IEnumerator PigOverlay()
    {
        yield return ShowOverlay(_pigColor);
    }

    IEnumerator HealOverlay()
    {
        yield return ShowOverlay(_healColor);
    }


    Color _bonusColor = new(1, 0.85f, 0, 0.3f);
    Color _damageColor = new(1, 0, 0, 0.3f);
    Color _pigColor = new(1, 0, 0.6f, 0.3f);
    Color _healColor = new(0, 1, 0, 0.3f);
}
