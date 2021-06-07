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
    [Header("Pools")]
    [SerializeField] string poolName = "";
    [SerializeField] string tagToHit = "";

    [Header("Projectile Properties")]
    [SerializeField] float speed = 35f;
    [SerializeField] bool homing;

    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject[] destroyOnHit = null;

    [Header("Lifetime")]
    [SerializeField] float maxLifeTime = 3f;
    [SerializeField] float lifeAfterImpact = 2f;

    [Header("Damage")]
    [SerializeField] int projectileDamage = 20;
    [SerializeField] bool stayOnTarget = false;

    private float speedStart;

    GameObject instigator;

    private Transform target = null;

    private Health targetHealth;

    bool doDamageOnce = false;
    Vector3 direction;
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
        if(!targetHealth.IsDead() && homing)
        {
        direction = ((target.transform.position+Vector3.up/2) - this.transform.position).normalized;
        transform.position += direction *Time.deltaTime * speed;
        this.transform.LookAt(GetAimLocation());

        }else{
                direction = ((target.transform.position + Vector3.up / 2) - this.transform.position).normalized;
                transform.position += transform.forward * Time.deltaTime * speed;
        }
       

    }

    public void OnSpawned()
    {
            if(target !=null && homing)
            {            
            this.transform.LookAt(GetAimLocation());

            }
            this.speed = speedStart;
            doDamageOnce = false;
    }

    public void SetTarget(Transform newtarget, GameObject instigator)
    {
        target = newtarget;
        targetHealth = target.GetComponent<Health>();
        this.instigator = instigator;
        PoolManager.Pools[poolName].Despawn(gameObject.transform, maxLifeTime);
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
        
        if (other.CompareTag(tagToHit) && !other.gameObject != instigator)
        {
            if (targetHealth.IsDead())
            {
                PoolManager.Pools[poolName].Despawn(gameObject.transform, lifeAfterImpact);                   
                return;
            }
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            if(!doDamageOnce)
            {
                if(other.TryGetComponent(out Health helth))
                {
                    helth.TakeDamage(GetDamage());
                }
                doDamageOnce = true;
            }
                if(stayOnTarget)
                this.transform.parent = other.transform;

            speed = 0;
            ProjectileSounds.Invoke();

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            PoolManager.Pools[poolName].Despawn(gameObject.transform, lifeAfterImpact);

        }   
    }


}
}