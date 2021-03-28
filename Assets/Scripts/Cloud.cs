using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float minSpeed, maxSpeed, minX, maxX;
    private float speed;


    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
    }
}
