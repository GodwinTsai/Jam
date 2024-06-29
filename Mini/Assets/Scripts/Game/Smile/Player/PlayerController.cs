using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    private BoxCollider2D Collider;

    public float moveSpeed = 50f; // 水平移动速度
    public float jumpForce = 20f; // 跳跃力
    public float jumpSpeed = 10f; // 跳跃速度

    private Vector2 _boxSizeV = new Vector2(0.5f, 0.01f); // 四边形的尺寸
    private Vector2 _boxSizeH = new Vector2(0.01f, 1.2f); // 四边形的尺寸
    public Transform bottomCastPoint; // 检查地面的位置
    public Transform topCastPoint; // 检查地面的位置
    public Transform rightCastPoint; // 检查地面的位置
    public Transform leftCastPoint; // 检查地面的位置
    public float _checkDis = 0.3f; // 地面检查的距离
    public LayerMask _groundLayer; // 地面层

    public bool isAutoMove = false;

    private Plat _stayPlat;

    enum JumpState
    {
        Grounded,
        Jump1,
        Jump2,
    }

    private JumpState _jumpState = JumpState.Grounded;

    private Vector3 _velocity; // 用于存储角色的速度

    public Vector3 Velocity
    {
        get { return _velocity; }
    }

    // Start is called before the first frame update
    void ControllerStart()
    {
        // Time.fixedDeltaTime = 0.01f;
        moveSpeed = DataCenter.Instance.configData.moveSpeed;
        jumpForce = DataCenter.Instance.configData.jumpForce;
        jumpSpeed = DataCenter.Instance.configData.jumpSpeed;
    }

    // Update is called once per frame
    void ControllerUpdate()
    {
        if (_started)
        {
            CustomUpdate();
        }
    }

    void FixedUpdate()
    {
        if (_started)
        {
            // CustomUpdate();
        }
    }

    void CustomUpdate()
    {
        RaycastHit2D bottomHit = Physics2D.BoxCast(bottomCastPoint.position, _boxSizeV, 0f, transform.up * -1,
            _checkDis, _groundLayer);
        RaycastHit2D topHit =
            Physics2D.BoxCast(topCastPoint.position, _boxSizeV, 0f, transform.up, _checkDis, _groundLayer);
        RaycastHit2D rightHit = Physics2D.BoxCast(rightCastPoint.position, _boxSizeH, 0f, transform.right, _checkDis,
            _groundLayer);
        RaycastHit2D leftHit = Physics2D.BoxCast(leftCastPoint.position, _boxSizeH, 0f, transform.right * -1, _checkDis,
            _groundLayer);
        // 在场景视图中绘制四边形射线
        DrawBoxCast(bottomCastPoint.position, _boxSizeV, transform.up * -1, _checkDis);
        DrawBoxCast(topCastPoint.position, _boxSizeV, transform.up, _checkDis);
        DrawBoxCast(rightCastPoint.position, _boxSizeH, transform.right, _checkDis);
        DrawBoxCast(leftCastPoint.position, _boxSizeH, transform.right * -1, _checkDis);

        _velocity.y += Physics2D.gravity.y * Time.deltaTime * jumpSpeed;
        if (bottomHit.collider == null && _stayPlat != null)
        {
            _stayPlat.OnPlayerExit(this);
            _stayPlat = null;
        }

        if (bottomHit.collider != null)
        {
            _jumpState = JumpState.Grounded;
            _velocity.y = 0;
            transform.position = new Vector3(transform.position.x, bottomHit.point.y, transform.position.z);
            if (_stayPlat == null)
            {
                _stayPlat = bottomHit.collider.GetComponent<Plat>();
                if (_stayPlat != null)
                {
                    _stayPlat.OnPlayerEnter(this);
                }
            }
            else
            {
                _stayPlat.OnPlayerStay(this);
            }
        }
        else if (topHit.collider != null)
        {
            _velocity.y = Mathf.Min(0, _velocity.y);
        }

        switch (_jumpState)
        {
            case JumpState.Grounded:
                if (CheckJump())
                {
                    GameFlowController.Instance.audioMgr.PlayJump();
                    _velocity.y = jumpForce;
                    _jumpState = JumpState.Jump1;
                }

                break;
            case JumpState.Jump1:
                if (CheckJump())
                {
                    GameFlowController.Instance.audioMgr.PlayJump();
                    _velocity.y = jumpForce;
                    _jumpState = JumpState.Jump2;
                }

                break;
            case JumpState.Jump2:
                break;
        }

        // 水平移动
        float moveInput = 1;
        if (!isAutoMove)
        {
            moveInput = Input.GetAxis("Horizontal");
        }

        if (rightHit.collider != null)
        {
            moveInput = Mathf.Min(0, moveInput);
        }
        else if (leftHit.collider != null)
        {
            moveInput = Mathf.Max(0, moveInput);
        }

        if (isDie)
        {
            moveInput = 0;
        }

        _velocity.x = moveInput * moveSpeed;
        transform.Translate(_velocity * Time.deltaTime);
        if (transform.position.y < 0.2f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
    }

    void DrawBoxCast(Vector2 origin, Vector2 size, Vector2 direction, float distance)
    {
        // 计算四个顶点的位置
        Vector2 halfSize = size / 2f;
        Vector2[] vertices = new Vector2[4];
        vertices[0] = origin + new Vector2(-halfSize.x, -halfSize.y);
        vertices[1] = origin + new Vector2(halfSize.x, -halfSize.y);
        vertices[2] = origin + new Vector2(halfSize.x, halfSize.y);
        vertices[3] = origin + new Vector2(-halfSize.x, halfSize.y);

        // 绘制四边形
        Debug.DrawLine(vertices[0], vertices[1], Color.red);
        Debug.DrawLine(vertices[1], vertices[2], Color.red);
        Debug.DrawLine(vertices[2], vertices[3], Color.red);
        Debug.DrawLine(vertices[3], vertices[0], Color.red);

        // 绘制四边形射线
        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.DrawLine(vertices[i], vertices[i] + direction * distance, Color.green);
        }
    }
    
    private bool CheckJump()
    {
#if UNITY_EDITOR
        return Input.GetButtonDown("Jump");
#else
        return CheckDeviceJump();
#endif
    }
    
    private bool CheckDeviceJump()
    {
        if (Input.touchCount <= 0)
        {
            return false;
        }

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x > Screen.width / 2f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}