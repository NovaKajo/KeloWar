using System.Collections;
using System.Collections.Generic;
using Kelo.Combat;
using Kelo.Player;
using PathologicalGames;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Shooter")]
public class Shooter : AttackBehaviour
{
   public GameObject projectilePrefabu;
    

   public void ShootProjectile(Transform target,Transform spawnPosition,Quaternion rotation)
   {
        Transform porjectileInstance = PoolManager.Pools["EnemyProjectiles"].Spawn(projectilePrefabu, spawnPosition.position, rotation);
        porjectileInstance.GetComponent<Projectile>().SetTarget(target, spawnPosition.gameObject);
   }

 
   public override void Execute(Transform target,Transform spawnPosition,Quaternion rotation)
   {
       ShootProjectile(target,spawnPosition,rotation);
   }
}
