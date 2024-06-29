using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public float width = 0.6f;
    public MonsterFactory monsterFactory;
    public GameObject idle;
    public GameObject smile;
    public GameObject jump;
    public GameObject die;
    private bool isDie = false;

    private float pressSpeed = 0.3f;
    private int idleDamage = 1;
    private int hurtInterval = 1;
    private float hurtTimer = 0;

    private bool _started = false;

    private void Awake()
    {
        EventManager.RegisterEvent(EventConst.EventGameOver, Die);
        EventManager.RegisterEvent<Vector3>(EventConst.EventLevelStart, LevelStart);
    }

    private void OnDestroy()
    {
        EventManager.UnregisterEvent<Vector3>(EventConst.EventLevelStart, LevelStart);
    }

    private void LevelStart(Vector3 spawnPoint)
    {
        // this.transform.position = spawnPoint;
        _started = true;
        isDie = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isDie = false;
        pressSpeed = DataCenter.Instance.configData.pressSpeed;
        hurtInterval = DataCenter.Instance.configData.hurtInterval;
        this.ControllerStart();
        // transform.position = new Vector3(DataCenter.Instance.SpawnX, DataCenter.Instance.GroundHeight);
    }

    // Update is called once per frame
    void Update()
    {
        this.ControllerUpdate();
        if (isDie)
        {
            idle.SetActive(false);
            smile.SetActive(false);
            jump.SetActive(false);
            die.SetActive(true);
        }
        else
        {
            DataCenter.Instance.durationTime += Time.deltaTime;
            UpdateInput();
            // UpdateJump();
            bool isSmile = DataCenter.Instance.IsSmile;
            die.SetActive(false);
            switch (_jumpState)
            {
                case JumpState.Grounded:
                    idle.SetActive(!isSmile);
                    smile.SetActive(isSmile);
                    jump.SetActive(false);
                    break;
                case JumpState.Jump1:
                case JumpState.Jump2:
                    idle.SetActive(false);
                    smile.SetActive(false);
                    jump.SetActive(true);
                    break;
            }

            if (!isSmile)
            {
                hurtTimer += Time.deltaTime;
                if (hurtTimer > hurtInterval)
                {
                    hurtTimer = 0;
                    ThumbUpMgr.Instance.AddThumbDown(5);
                }
            }
        }

        // CheckCollideMonsters();
    }

    void UpdateInput()
    {
        // 检查是否按住了空格键
        if (IsInputValid())
        {
            DataCenter.Instance.AddPressValue(pressSpeed * Time.deltaTime);
        }
        else
        {
            DataCenter.Instance.AddPressValue(-pressSpeed * Time.deltaTime);
        }
    }

    private bool IsInputValid()
    {
#if UNITY_EDITOR
        return Input.GetKey(KeyCode.Space);
#else
        return IsDeviceInputValid();
#endif
    }

    private bool IsDeviceInputValid()
    {
        if (Input.touchCount <= 0)
        {
            return false;
        }

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (touch.position.x <= Screen.width / 2f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Die()
    {
        isDie = true;
    }

    void CheckCollideMonsters()
    {
        List<Monster> monsters = monsterFactory.Monsters;
        foreach (Monster monster in monsters)
        {
            if (monster == null)
            {
                continue;
            }

            if (monster.hurted)
            {
                continue;
            }

            Rect playerRect = new Rect(transform.localPosition.x - width * 0.5f, transform.localPosition.y, width, 1);
            Rect monsterRect = new Rect(monster.transform.localPosition.x - width * 0.5f,
                monster.transform.localPosition.y, monster.width, monster.height);

            if (playerRect.Overlaps(monsterRect))
            {
                Debug.Log("碰撞");
                DataCenter.Instance.Hurt(10);
                monster.hurted = true;
                break;
            }
        }
    }
}