using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigData : MonoBehaviour
{
    [Header("初始点赞数")]
    public int thumbUp = 30;
    
    [Header("玩家水平移动速度")]
    public float moveSpeed = 5f; // 水平移动速度
    [Header("玩家跳跃力")]
    public float jumpForce = 25f; // 跳跃力
    [Header("玩家跳跃速度")]
    public float jumpSpeed = 10f; // 跳跃速度
    
    
    [Header("嘻嘻条位置百分比")]
    public float ValidAreaValue = 0.6f;
    
    [Header("嘻嘻条宽度百分比")]
    public float ValidAreaRange = 0.3f;

    [Header("嘻嘻条变化速度")] 
    public float pressSpeed = 0.3f;
    
    
    [Header("不嘻嘻的时候，掉赞间隔（秒）")] 
    public int hurtInterval = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
