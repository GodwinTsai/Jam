using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public DefenseArt Art;
    public DefenseAttr Damage;
    public DefenseAttr MoveSpeed;

    /// <summary>
    /// 发射次数
    /// </summary>
    private int _fireNum;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}