using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemySpawn : MonoBehaviour
{
    public enemyList enemyList;
    public string choice;
    public int _choiceIndex;
    public float ZLayer;
    public static Transform EnemiesCont;
    public float Vision;
    // Start is called before the first frame update
    void Start()
    {
        if (!EnemiesCont)
        {
            GameObject ItemCont = Instantiate(GameManager.getShared().EmptyObj);
            ItemCont.transform.position = Vector3.zero;
            ItemCont.name = "Enemies";
            EnemiesCont = ItemCont.transform;
        }

        GameObject enemy = Instantiate(enemyList.Enemies[_choiceIndex], EnemiesCont);
        enemy.transform.position = new Vector3(transform.position.x,transform.position.y, ZLayer);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Vision);
    }
}
