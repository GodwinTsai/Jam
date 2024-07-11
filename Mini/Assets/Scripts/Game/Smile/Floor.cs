using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = DataCenter.Ins.mapMoveSpeed * Time.deltaTime;
        // 持续向左移动
        // transform.Translate(Vector3.left * moveDistance);

        if (transform.localPosition.x < -5.94f)
        {
            // transform.localPosition = new Vector3(8.34f, transform.localPosition.y, transform.localPosition.z);
        }
    }
}