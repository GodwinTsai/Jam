// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class Plat : MonoBehaviour
{
    public GameObject fg;
    
    private bool _isStay = false;
    private bool _isTriggered;

    private void Awake()
    {
        if (fg != null)
        {
            fg.SetActive(false);
        }
    }

    public void OnPlayerEnter(Player player)
    {
        if (!_isStay)
        {
            _isStay = true;
            this.HandlePlayerEnter(player);
        }
    }

    public void OnPlayerStay(Player player)
    {
        _isStay = true;
        // Debug.LogError("Plat staying with: " + player.gameObject.name);
    }

    public void OnPlayerExit(Player player)
    {
        if (_isStay)
        {
            _isStay = false;
            // Debug.LogError("Plat exited with: " + player.gameObject.name);   
        }
    }

    private void HandlePlayerEnter(Player player)
    {
        if (_isTriggered)
        {
            return;
        }

        _isTriggered = true;
        // MTDebug.LogError(
        //     $"Plat OnTriggerEnter2D {player.gameObject.name}, tag:{player.gameObject.tag}, {player.transform.position.y}, {transform.position.y}");

        if (fg != null)
        {
            fg.SetActive(true);
        }

        OnTriggerAction();
    }

    protected virtual void OnTriggerAction()
    {
        DataCenter.Instance.AddThumbUp();
    }
}