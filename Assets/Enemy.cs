using System.Collections;
using System.Collections.Generic;
using Kelo.Stats;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private void Start() {
        EnemyList.addEnemyToList(this);
    }

    public void RemoveFromList()
    {
            EnemyList.enemies.Remove(this);
    }
}
