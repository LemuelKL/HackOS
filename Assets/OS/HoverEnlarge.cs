using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEnlarge : MonoBehaviour
{
    public void Enlarge()
    {
        transform.localScale += new Vector3(0.1F, 0.1F, 0);
    }

    public void Reset()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
