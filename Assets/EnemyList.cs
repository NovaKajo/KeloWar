using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public static List<Enemy> enemies;
    public static Enemy closestEnemyToPlayer;

    private void Awake() {
        enemies = new List<Enemy>();
    }

    public static void FindClosestEnemy(Transform playerPosition)
    {

        float distance = Mathf.Infinity;
        Vector3 position = playerPosition.position;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeSelf == false)
            {
                continue;
            }
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestEnemyToPlayer = enemy;
                distance = curDistance;
            }

        }
    }

    public Enemy getClosestEnemy()
    {
        return closestEnemyToPlayer;
    }
    public static void addEnemyToList(Enemy toAdd)
    {
        enemies.Add(toAdd);
    }
}
