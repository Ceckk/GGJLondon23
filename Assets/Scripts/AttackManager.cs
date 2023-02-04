using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public enum AttakType
    {
        None,
        Vertical,
        Horizontal,
        Around
    }

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
        var attackType = GetAttackType(PlayerManager.Instance.transform.position);
        Debug.Log(attackType);
    }

    private AttakType GetAttackType(Vector3 position)
    {
        var tile = TilemapManager.Instance.GetTile(position);

        if (tile != null)
        {
            switch (tile.name)
            {
                case "Horizontal":
                    return AttakType.Horizontal;
                case "Vertical":
                    return AttakType.Vertical;
                case "Around":
                    return AttakType.Around;
            }
        }

        return AttakType.None;
    }

    private void HandleEnemyBehaviour()
    {
        // TODO enemy move / attack
    }
}
