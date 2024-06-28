using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseAttr : MonoBehaviour
{
    public float BaseValue;

    public float AdditionValue;

    public float FinalValue
    {
        get { return this.BaseValue + this.AdditionValue; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}