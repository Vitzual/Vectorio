﻿using UnityEngine;

public class KillEnemies : MonoBehaviour
{
    // Carriers will destroy any building they touch
    // Additional logic requires a Rigibody component be attached to this unit
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            collision.collider.GetComponent<EnemyClass>().KillEntity();
    }
}