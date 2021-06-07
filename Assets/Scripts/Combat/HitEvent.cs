using System.Collections;
using System.Collections.Generic;
using Kelo.Player;
using PathologicalGames;
using UnityEngine;

namespace Kelo.Combat
{

    public class HitEvent : MonoBehaviour
{
    PlayerAttack playerAttack;
    public Transform arrowPrefab;
 

    private void Start() {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

   void Hit()
   {
       playerAttack.DamageTarget();
   }

   void Arrow()
   {
       if(playerAttack.GetTarget() == null)
       {
           return;
       }
      playerAttack.arrowInHand.gameObject.SetActive(false);
      Transform porjectileInstance = PoolManager.Pools["Arrows"].Spawn(arrowPrefab,playerAttack.arrowInHand.transform.position,Quaternion.identity);
   
      porjectileInstance.GetComponent<Projectile>().SetTarget(playerAttack.GetTarget().transform,this.gameObject);
        
   }
}
}