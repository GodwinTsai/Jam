using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter : SingletonClassAuto<DataCenter>
{
    public float SpawnX = 3;
    public float GroundHeight = 1.5f;
    public int MaxHP = 50;
    public int HP = 50;
    public float PressSlderValue = 0.6f;
    public float ValidAreaValue = 0.6f;
    public float ValidAreaRange = 0.3f;

    public float mapMoveSpeed = 3;
    public float durationTime = 0;

    public GameConfigData configData;

    #region Score

    public int thumbUp = 30;
    public int love;

    #endregion

    public override void Init()
    {
        base.Init();
        GameObject config = GameObject.Find("GameConfigData");
        configData = config.GetComponent<GameConfigData>();
        ResetData();
    }

    public void ResetData()
    {
        thumbUp = configData.thumbUp;
        ValidAreaValue = configData.ValidAreaValue;
        ValidAreaRange = configData.ValidAreaRange;
    }

    public bool IsSmile
    {
        get
        {
            return PressSlderValue > ValidAreaValue - ValidAreaRange * 0.5f &&
                   PressSlderValue < ValidAreaValue + ValidAreaRange * 0.5f;
        }
    }

    public void ReTry()
    {
        HP = MaxHP;
    }

    public void Hurt(int damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            // EventManager.ExecuteEvent("GameLose");
        }

        HP = Mathf.Clamp(HP, 0, MaxHP);
    }

    public void AddPressValue(float value)
    {
        if (PressSlderValue <= 1)
        {
            PressSlderValue += value;
        }

        PressSlderValue = Mathf.Clamp01(PressSlderValue);
    }

    public void AddThumbUp()
    {
        thumbUp++;
        ThumbUpToolTipUtil.ShowThumbUpToolTipLocalPos(EnumScoreType.ThumbUp, EnumScoreNumType.Add);
    }

    public void AddLove()
    {
        // love++;
        thumbUp++;
        ThumbUpToolTipUtil.ShowThumbUpToolTipLocalPos(EnumScoreType.Love, EnumScoreNumType.Add);
    }

    public void MinusThumbUp()
    {
        if (thumbUp > 0)
        {
            thumbUp--;
            ThumbUpToolTipUtil.ShowThumbUpToolTipLocalPos(EnumScoreType.ThumbDown, EnumScoreNumType.Minus);
        }
        else
        {
            //End
            EventManager.ExecuteEvent(EventConst.EventGameOver);
        }
    }

    public void MinusLove()
    {
        if (thumbUp > 0)
        {
            thumbUp--;
            ThumbUpToolTipUtil.ShowThumbUpToolTipLocalPos(EnumScoreType.LoveBroken, EnumScoreNumType.Minus);
        }
        else
        {
            //End
        }
    }
}