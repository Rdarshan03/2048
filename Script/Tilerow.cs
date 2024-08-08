using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilerow : MonoBehaviour
{
    public Tilecell[] cells {  get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<Tilecell>();
    }
}
