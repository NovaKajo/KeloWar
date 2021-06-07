using UnityEngine;

namespace Kelo.Enemies
{

public class Enemy : MonoBehaviour
{
    
    private void Start() {
        EnemyList.addEnemyToList(this);
    }

    public void RemoveFromList()
    {
            EnemyList.enemies.Remove(this);
    }
    
   
}
}
