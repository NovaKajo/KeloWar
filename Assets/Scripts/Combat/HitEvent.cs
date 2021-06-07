using System.Collections;
using System.Collections.Generic;
using Kelo.Player;
using UnityEngine;

namespace Kelo.Combat
{

    public class HitEvent : MonoBehaviour
{
    PlayerAttack playerAttack;
    public Projectile arrowPrefab;
 

    private void Start() {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

   void Hit()
   {
       playerAttack.DamageTarget();
   }

   void Arrow()
   {
       if(playerAttack.targetGJ == null)
       {
           return;
       }
      playerAttack.arrowInHand.gameObject.SetActive(false);
      Projectile porjectileInstance = Instantiate(arrowPrefab,playerAttack.arrowInHand.transform.position,Quaternion.identity);
      porjectileInstance.SetTarget(playerAttack.targetGJ.transform,-5,this.gameObject);
        
   }
}
}