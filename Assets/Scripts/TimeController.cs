using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static IEnumerator ScaleTimeAsync(this Time time, float timeScale, float speed = 1f)
    {
        //time...
        yield return null;
    }
} 

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; set; }
    float rate = 0.02f;
    IEnumerator coroutine;

    public float TimeScale
    {
        get { return Time.timeScale; }
        set
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = ScaleTimeAsync(value);
            StartCoroutine(coroutine);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TimeScale = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TimeScale = 0f;
        }

        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    IEnumerator _ScaleTimeAsync(float targetScale, float speed = 2f)
    {
        float initialScale = Time.timeScale;
        float totalTime = Mathf.Abs(targetScale - initialScale) / speed;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime > totalTime)
            {
                elapsedTime = totalTime;
            }

            Time.timeScale = Mathf.Lerp(initialScale, targetScale, elapsedTime / totalTime);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }
    }

    IEnumerator ScaleTimeAsync(float targetScale, float speed = 1f)
    {
        float initialScale = Time.timeScale;
        float totalDistance = Mathf.Abs(targetScale - initialScale);
        float elapsedTime = 0f;
        float currentDistance = 0f;

        while (currentDistance < totalDistance)
        {
            elapsedTime += Time.unscaledDeltaTime;
            currentDistance = (speed * elapsedTime);
            if (currentDistance > totalDistance)
            {
                currentDistance = totalDistance;
            }

            Time.timeScale = Mathf.Lerp(initialScale, targetScale, currentDistance / totalDistance);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }

        Debug.Log("elapsed: " + elapsedTime +" Scale: "+Time.timeScale);
    }
}
