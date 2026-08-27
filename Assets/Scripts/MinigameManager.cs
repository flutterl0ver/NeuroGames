using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject targetWindow;
    [SerializeField] private GameObject targetField;

    // Префаб круга (UI Image с RectTransform). Назначается в инспекторе
    public GameObject circlePrefab;
    // Экран поражения (показывается при промахе)
    public GameObject looseScreen;
    public TextMeshProUGUI counter;

    public System.Random rand = new();
    public float spawnInterval { get { return rand.Next(2, 11) / 10f; } }
    public float circleLifetimeSeconds = 2.0f;
    public int maxActiveCircles = 5;
    public int targetScore = 40;
    public float looseScreenDelay = 2;
    public int Score { get { return score; } set { counter.text = $"Счёт: {value}/{targetScore}"; score = value; } }

    private int score;
    private static MinigameManager _instance;
    private Coroutine _spawnRoutine;
    private Coroutine _looseRoutine;
    private readonly List<GameObject> _activeCircles = new List<GameObject>();

    public static MinigameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<MinigameManager>();
            }

            if (_instance == null)
            {
                GameObject go = new GameObject("MinigameManager");
                _instance = go.AddComponent<MinigameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void OnEnable()
    {
        // Подписываемся на событие клика по кругу, чтобы удалить его из списка
        CircleController.CircleClicked += OnCircleClicked;
        CircleController.CircleMissed += OnCircleMissed;
    }

    private void OnDisable()
    {
        CircleController.CircleClicked -= OnCircleClicked;
        CircleController.CircleMissed -= OnCircleMissed;
        StopSpawn();
    }

    public void OpenWindow()
    {
        if (targetWindow == null)
        {
            Debug.LogWarning("MinigameManager: targetWindow не назначен в Inspector.");
            return;
        }

        Score = 0;
        targetWindow.SetActive(true);
        PlayerController.SetMovementLocked(true);
        StartSpawn();
    }

    public void CloseWindow()
    {
        if (targetWindow == null)
            return;

        targetWindow.SetActive(false);
        PlayerController.SetMovementLocked(false);
        StopSpawn();
        ClearAllCircles();
    }

    public bool IsWindowOpen()
    {
        return targetWindow != null && targetWindow.activeSelf;
    }

    private void StartSpawn()
    {
        if (circlePrefab == null)
        {
            Debug.LogWarning("MinigameManager: circlePrefab не назначен.");
            return;
        }

        if (_spawnRoutine == null)
            _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (targetWindow != null && targetWindow.activeInHierarchy)
        {
            if (_activeCircles.Count < maxActiveCircles)
                SpawnCircle();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCircle()
    {
        if (targetField == null || circlePrefab == null)
            return;

        RectTransform fieldRt = targetField.GetComponent<RectTransform>();
        if (fieldRt == null)
        {
            Debug.LogWarning("MinigameManager: targetField не имеет RectTransform.");
            return;
        }

        GameObject go = Instantiate(circlePrefab, targetField.transform);
        RectTransform rt = go.GetComponent<RectTransform>();

        // Вычисляем случайную позицию внутри RectTransform targetWindow, учитывая размер префаба
        if (rt != null)
        {
            float parentWidth = fieldRt.rect.width;
            float parentHeight = fieldRt.rect.height;

            float prefabWidth = rt.rect.width;
            float prefabHeight = rt.rect.height;

            float minX = -fieldRt.pivot.x * parentWidth + prefabWidth * rt.pivot.x;
            float maxX = (1 - fieldRt.pivot.x) * parentWidth - prefabWidth * (1 - rt.pivot.x);

            float minY = -fieldRt.pivot.y * parentHeight + prefabHeight * rt.pivot.y;
            float maxY = (1 - fieldRt.pivot.y) * parentHeight - prefabHeight * (1 - rt.pivot.y);

            float x = UnityEngine.Random.Range(minX, maxX);
            float y = UnityEngine.Random.Range(minY, maxY);

            rt.anchoredPosition = new Vector2(x, y);
        }

        _activeCircles.Add(go);
        go.SetActive(true);

        // Передаём время жизни в контроллер круга (контроллер сам вызовет CircleMissed при таймауте)
        CircleController cc = go.GetComponent<CircleController>();
        if (cc != null)
            cc.lifetimeSeconds = circleLifetimeSeconds;
    }

    private void OnCircleClicked()
    {
        // CircleController удаляет объект самостоятельно; просто очищаем список тех, что уже уничтожены
        _activeCircles.RemoveAll(item => item == null);
        Score++;
        if (Score >= targetScore)
        {
            CloseWindow();
            OnWin();
        }
    }

    private void OnCircleMissed()
    {
        // Круг был удалён по таймауту — очищаем список уничтоженных
        _activeCircles.RemoveAll(item => item == null);

        // Закрываем окно миниигры
        CloseWindow();

        // Показываем экран поражения на 2 секунды
        if (looseScreen != null)
        {
            if (_looseRoutine != null)
                StopCoroutine(_looseRoutine);

            looseScreen.SetActive(true);
            _looseRoutine = StartCoroutine(HideLooseScreenAfterDelay(looseScreenDelay));
        }
    }

    private IEnumerator HideLooseScreenAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        if (looseScreen != null)
            looseScreen.SetActive(false);
        _looseRoutine = null;
    }

    private void ClearAllCircles()
    {
        for (int i = _activeCircles.Count - 1; i >= 0; i--)
        {
            if (_activeCircles[i] != null)
                Destroy(_activeCircles[i]);
        }

        _activeCircles.Clear();
    }

    private void OnWin()
    {

    }
}
