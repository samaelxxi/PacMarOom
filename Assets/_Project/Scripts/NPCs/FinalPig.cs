using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;

public class FinalPig : MonoBehaviour
{
    Tween _flyTween;
    [SerializeField] Transform _mesh;
    [SerializeField] float _verticalOffset = 1;

    // Start is called before the first frame update
    void Start()
    {
        _flyTween = _mesh.DOLocalMoveY(_verticalOffset, _verticalOffset).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        ScheduleLag();
    }

    void TeleportToRandomPos()
    {
        var pos = new Vector3(UnityEngine.Random.Range(-0.7f,0.7f), 
                            UnityEngine.Random.Range(-0.7f, 0.7f), 
                            UnityEngine.Random.Range(-0.7f, 0.7f));
        _mesh.transform.localPosition = pos;
        var scale = UnityEngine.Random.Range(0.8f, 1.6f);
        _mesh.localScale = new Vector3(scale, scale, scale);
    }

    IEnumerator LagPosition()
    {
        _flyTween.Pause();
        Vector3 oldPos = _mesh.transform.localPosition;
        int lagTimes = UnityEngine.Random.Range(4, 8);
        for (int i = 0 ; i < lagTimes; i++)
        {
            TeleportToRandomPos();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.02f, 0.04f));
        }
        _mesh.transform.localPosition = oldPos;
        _mesh.localScale = Vector3.one;
        _flyTween.Play();
        ScheduleLag();
    }

    void ScheduleLag()
    {
        float time = UnityEngine.Random.Range(8.0f, 12.0f);
        this.InSeconds(time, () => StartCoroutine(LagPosition()));
    }
}
