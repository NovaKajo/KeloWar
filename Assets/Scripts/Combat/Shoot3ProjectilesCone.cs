
using System.Collections;
using System.Collections.Generic;
using Kelo.Combat;
using PathologicalGames;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Shoot3")]
public class Shoot3ProjectilesCone : AttackBehaviour
{
    public GameObject projectilePrefabu;

    public void Shoot3ProjectilesCones(Transform target, Transform spawnPosition,Quaternion rotation)
    {
        Transform porjectileInstance = PoolManager.Pools["EnemyProjectiles"].Spawn(projectilePrefabu, spawnPosition.position + Vector3.up + Vector3.forward / 2, rotation);
        Transform porjectileInstance1 = PoolManager.Pools["EnemyProjectiles"].Spawn(projectilePrefabu, spawnPosition.position + Vector3.left * 3 + Vector3.up + Vector3.forward / 2, rotation);
        Transform porjectileInstance2 = PoolManager.Pools["EnemyProjectiles"].Spawn(projectilePrefabu, spawnPosition.position + Vector3.right * 3 + Vector3.up + Vector3.forward / 2, rotation);
        porjectileInstance.GetComponent<Projectile>().SetTarget(target, spawnPosition.gameObject);
        porjectileInstance1.GetComponent<Projectile>().SetTarget(target, spawnPosition.gameObject);
        porjectileInstance2.GetComponent<Projectile>().SetTarget(target, spawnPosition.gameObject);

    }
    public override void Execute(Transform target, Transform spawnPosition, Quaternion rotation)
    {
        Shoot3ProjectilesCones(target, spawnPosition,rotation);
    }
}
