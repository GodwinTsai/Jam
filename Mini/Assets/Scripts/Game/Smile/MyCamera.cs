using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public float offsetX;

    // private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // GameObject obj = GameObject.Find("Player");
        // if (obj != null)
        // {
        //     player = obj.transform;
        // }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var player = GameMgr.Ins.player;
        if (player == null)
        {
            return;
        }
        transform.position = new Vector3(player.transform.position.x + offsetX, 5, -10) ;
    }
}