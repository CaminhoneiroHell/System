using UnityEngine;
using System.Collections.Generic;


public class EnemiesPool : MonoBehaviour
{
    public List<PollItem> pooledEnemyList = new List<PollItem>();

    public void CreatePool()
    {
        
    }

    private GameObject GetEnemyFromPool(string enemyName)
    {
        for(int i = 0; i < pooledEnemyList.Count; i++)
        {
            if(!pooledEnemyList[i].enemyGameObj.activeInHierarchy && pooledEnemyList[i].enemyName == enemyName)
            {
                return pooledEnemyList[i].enemyGameObj;
            }
        }

        return null;
    }


}
