using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : SingleMonoBehaviour<TimeController>
{
    [SerializeField] [Range(0.02f, 10f)] float rateOfChange = 1f;
    static IEnumerator coroutine;

    public float TimeScale
    {
        get { return Time.timeScale; }
        set
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = ScaleTimeAsync(value, rateOfChange);
            StartCoroutine(coroutine);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimeScale = 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TimeScale = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TimeScale = 0.05f;
        }

        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    IEnumerator ScaleTimeAsync(float target, float rateOfChange = 1f)
    {
        float initial = Time.timeScale;
        float totalTime = Mathf.Abs(target - initial) / rateOfChange;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime > totalTime)
            {
                elapsedTime = totalTime;
            }

            Time.timeScale = Mathf.Lerp(initial, target, elapsedTime / totalTime);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }

        //Debug.Log($"{elapsedTime}s from {initial} to {Time.timeScale}");
    }


    public static IEnumerator InvokeRepeating(Func<bool> conditionMet, float interval = 0f)
    {
        var delay = new WaitForSeconds(interval);

        while (!conditionMet())
        {
            yield return delay;
        }
    }
}

public static class Extensions
{
    public static IEnumerator Async(this MonoBehaviour mb, Func<float, bool> conditionRun)
    {
        float elapsedTime = 0f;

        while (conditionRun(elapsedTime));
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }
    }

    public static void InvokeRepeating(this MonoBehaviour mb, Func<bool> conditionMet, float interval = 0f)
    {
        mb.StartCoroutine(TimeController.InvokeRepeating(conditionMet, interval));
    }
}