using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoryPage : MonoBehaviour, IPointerClickHandler
{
    public Story Story;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 当鼠标点击Image时调用此方法
    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查是否是左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 在这里添加你的处理逻辑
            this.Story.HandlePageClick();
        }
    }
}