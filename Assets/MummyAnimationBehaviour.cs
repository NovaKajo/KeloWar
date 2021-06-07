using Kelo.AI;
using UnityEngine;

public class MummyAnimationBehaviour : AIAnimator  //esta clase depende unicamente del animador y mover para rescatar valores de velocidad en navmesh, si no estan seteados vas a tener problemas 
{
    [SerializeField] private AIMover mover;   

    private int attackHash = Animator.StringToHash("Attack");
    private int forwardSpeedHash = Animator.StringToHash("forwardSpeed");
    private int isAliveHash = Animator.StringToHash("isAlive");
    

    // Start is called before the first frame update
    void Awake()
    {
        Init();
        mover = GetComponent<AIMover>();
    }

 

    private void Update() {
        if(!myAnim.GetBool(isAliveHash))
        {
            return;
        }
        if(mover.GetForwardSpeed() <= 0.1 + Mathf.Epsilon )
        {
            RandomStirr();
            return;
        }
        moveForward();        
    }
    
    public override void AttackAnimationAI()
    {
        myAnim.SetTrigger(attackHash);
    }

    public override void moveForward()
    {
        myAnim.SetFloat(forwardSpeedHash,mover.GetForwardSpeed());
    }

    public override void Die(bool value)
    {
       myAnim.SetBool(isAliveHash,value);
    }

    public void RandomStirr()
    {
        if(UnityEngine.Random.Range(0,1500)< 1)
        {            
            myAnim.SetBool("Stirr",randomBoolean());
        }
    }
    public bool randomBoolean()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

    public override void GetHit(float position)
    {
        switch (position)
        {
                case 0:
                 myAnim.SetTrigger("GetHitFront");
            break;
                case 1:
                 myAnim.SetTrigger("GetHitRight");
            break;
                case 2:
                 myAnim.SetTrigger("GetHitLeft");
            break;
                default:
                 myAnim.SetTrigger("GetHitFront");
            break;
        }
        StopStir();
    }

    private void StopStir()
    {
        myAnim.SetBool("Stirr", false);
    }
}
