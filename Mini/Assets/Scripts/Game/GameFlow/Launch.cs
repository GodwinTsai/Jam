using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour
{
    public Canvas Canvas;
    public GameObject Prefab;
    public GameObject mapRoot;
    public static Launch Current;

    void Awake()
    {
        Current = this;
        GameObject ui = Instantiate(this.Prefab, Canvas.transform);
        GameFlow gf = ui.GetComponent<GameFlow>();
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localRotation = Quaternion.identity;
        ui.transform.localScale = Vector3.one;
        GameFlowController.Ins.Init(gf);
        // TablesHelper.Ins.InitConfig();

        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.A))
        {
            DataCenter.Ins.AddThumbUp();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            DataCenter.Ins.AddLove();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            DataCenter.Ins.MinusThumbUp();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            DataCenter.Ins.MinusLove();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            LevelMgr.Ins.EnterNextLevel();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            LevelMgr.Ins.RetryCurLevel();
        }
#endif
        if (Input.GetMouseButtonUp(0))
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EffectMgr.Ins.PlayEffect(worldPos, "EatProp");
        }
    }
}
