using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Transform transform;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Start()
    {
        this.InvokeRepeating(() => {
            transform.localScale += Vector3.one * Time.deltaTime;
            return transform.localScale.y > 10f;
        }, 0.02f);
        /*
        StartCoroutine(TimeController.InvokeRepeating(() => {
            transform.localScale += Vector3.one;
            return transform.localScale.y > 10f;
        }, 0.5f));*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Input.GetAxisRaw("Horizontal") * speed * Time.unscaledDeltaTime;
    }
}