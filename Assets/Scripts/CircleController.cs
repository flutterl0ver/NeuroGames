using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleController : MonoBehaviour, IPointerClickHandler
{
    // Событие вызывается при клике по кругу
    public static event Action CircleClicked;
    // Событие вызывается если круг исчез по таймауту
    public static event Action CircleMissed;

    // Время жизни круга в секундах. Устанавливается MinigameManager при спавне.
    public float lifetimeSeconds = 2f;

    private bool _handled;
    private Coroutine _timeoutRoutine;

    private void OnEnable()
    {
        _handled = false;
        if (lifetimeSeconds > 0f)
            _timeoutRoutine = StartCoroutine(AutoTimeout());
    }

    private void OnDisable()
    {
        if (_timeoutRoutine != null)
        {
            StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_handled)
            return;

        _handled = true;
        if (_timeoutRoutine != null)
        {
            StopCoroutine(_timeoutRoutine);
            _timeoutRoutine = null;
        }

        CircleClicked?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator AutoTimeout()
    {
        yield return new WaitForSeconds(lifetimeSeconds);

        if (_handled)
            yield break;

        _handled = true;
        CircleMissed?.Invoke();
        Destroy(gameObject);
    }
}
