using System.Collections;
using System.Collections.Generic;
using Kelo.Stats;
using PathologicalGames;
using UnityEngine;
using UnityEngine.Events;

namespace Kelo.Combat
{


public class Projectile : MonoBehaviour
{
    [SerializeField] UnityEvent ProjectileSounds;
    [SerializeField] float speed = 35f;
    [SerializeField] bool homing;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTime = 3f;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2f;
    [SerializeField] int projectileDamage = 20;
    private float speedStart;
    GameObject instigator;
    private Transform target = null;
    private Health targetHealth;

    bool doDamageOnce = false;
    // Start is called before the first frame update
    void Awake()
    {   
        speedStart = speed;
    }
    public int GetDamage()
    {
        return projectileDamage;
    }

    public int addDamage(int addedDmg)
    {
        return projectileDamage+addedDmg;
    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            return;
        }
        if(!targetHealth.IsDead())
        {
        Vector3 dir = ((target.transform.position+Vector3.up/2) - this.transform.position).normalized;
        transform.position += dir *Time.deltaTime * speed;

        }else{
                transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (homing && !targetHealth.IsDead())
        {
            this.transform.LookAt(GetAimLocation());
            
        }

    }

  
    public void OnSpawned()
    {
            if(target !=null)
            this.transform.LookAt(GetAimLocation());
            this.speed = speedStart;
            doDamageOnce = false;
    }

    public void SetTarget(Transform newtarget, GameObject instigator)
    {

        target = newtarget;
        targetHealth = target.GetComponent<Health>();
        this.instigator = instigator;
        PoolManager.Pools["Arrows"].Despawn(gameObject.transform, maxLifeTime);

        }

    private Vector3 GetAimLocation()
    {       
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
          
            return target.transform.position;
        }
        else
        {
            
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
      

    }
    private void OnTriggerEnter(Collider other)  //Cambiar a cualquier enemigo que choque
    {
        if(other.CompareTag("Wall"))
        {
            speed = 0;
            return;
        }
        
        if (target != null && target.gameObject == other.gameObject && !other.gameObject.CompareTag("Player"))
        {
            if (targetHealth.IsDead())
            {
                PoolManager.Pools["Arrows"].Despawn(gameObject.transform, lifeAfterImpact);                   
                    return;
            }
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            if(!doDamageOnce)
            {
            targetHealth.TakeDamage(GetDamage());
            doDamageOnce = true;
            }
            this.transform.parent = other.transform;
            speed = 0;
            ProjectileSounds.Invoke();
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            PoolManager.Pools["Arrows"].Despawn(gameObject.transform, lifeAfterImpact);

        }   
    }


}
}