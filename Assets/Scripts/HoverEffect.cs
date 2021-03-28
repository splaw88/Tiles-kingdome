using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float hoverAmount = .2f;

    private void OnMouseEnter()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - hoverAmount, transform.localScale.y + hoverAmount, transform.localScale.z + hoverAmount);
        }
        else
        {
            transform.localScale += Vector3.one * hoverAmount;
        }
    }

    private void OnMouseExit()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x + hoverAmount, transform.localScale.y - hoverAmount, transform.localScale.z - hoverAmount);
        }
        else
        {
            transform.localScale -= Vector3.one * hoverAmount;
        }
    }
}
