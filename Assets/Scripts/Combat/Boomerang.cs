using PathologicalGames;
using UnityEngine;

namespace Kelo.Combat{

    [CreateAssetMenu(menuName = "Attacks/Boomerang")]
    public class Boomerang : AttackBehaviour
    {
        public GameObject projectilePrefabu;
    
        public void BoomerangMovement(Transform target,Transform spawnPosition)
        { // revisar bien este codigo
        
        Transform porjectileInstance = PoolManager.Pools["EnemyProjectiles"].Spawn(projectilePrefabu);

        if(porjectileInstance.TryGetComponent(out Projectile projectile))
        {
            projectile.SetTarget(target, spawnPosition.gameObject);
        }
        
            if (!porjectileInstance.TryGetComponent(out ParabolaController parabolaController))
            {
                Debug.Log("no hay parabola Controller");
                return;
            }
            if (spawnPosition.GetComponentInChildren<Root>())
            {
                parabolaController.ParabolaRoot = spawnPosition.GetComponentInChildren<Root>().gameObject;
            }else{
                Debug.Log("No Hay parabola Root");
                return;
            }
        parabolaController.Move(target);

        }
        public override void Execute(Transform target, Transform spawnPosition, Quaternion rotation)
        {
            BoomerangMovement(target,spawnPosition);
        }
    }
}