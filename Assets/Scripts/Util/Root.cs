using Kelo.AI;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public Transform position3;
    [SerializeField] AIFighter fighter;
    // Start is called before the first frame update
    void Awake()
    {
    target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(fighter.GetTarget())
        position3.position = fighter.GetTarget().transform.position;
        
    }
}
