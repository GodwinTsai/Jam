using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float height = 1f;

    public float width = 0.5f;

    public bool hurted = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = DataCenter.Instance.mapMoveSpeed * Time.deltaTime;
        // 持续向左移动
        transform.Translate(Vector3.left * moveDistance);

        if (transform.localPosition.x < -5f)
        {
            Destroy(gameObject);
        }
    }
}