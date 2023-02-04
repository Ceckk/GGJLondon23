using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    void Start()
    {
        EventAggregator.Instance.AddListener<TicksManager.OnSpecialTick>(OnSpecialTick);
    }

    void OnDestroy()
    {
        EventAggregator.Instance.RemoveListener<TicksManager.OnSpecialTick>(OnSpecialTick);
    }

    private void OnSpecialTick(IEvent e)
    {
        HandlePlayerAttack();
        HandleEnemyBehaviour();
    }

    private void HandlePlayerAttack()
    {
        var pos = PlayerManager.Instance.transform.position;
        var tile = TilemapManager.Instance.GetTile(pos);

        if (tile != null)
        {
            switch (tile.name)
            {
                case "Horizontal":
                    TilemapManager.Instance.Attack(pos);
                    break;
                case "Vertical":
                    TilemapManager.Instance.Attack(pos);
                    break;
                case "Around":
                    TilemapManager.Instance.Attack(pos);
                    break;
            }
        }
    }

    private void HandleEnemyBehaviour()
    {
        // TODO enemy move / attack
    }
}
