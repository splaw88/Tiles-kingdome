using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageIcons;

    public float lifetime;

    public GameObject destroyEffect;

    private void Start()
    {
        Invoke("Destruction", lifetime);
    }

    public void Setup(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = damageIcons[damage - 1];
    }

    void Destruction()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
