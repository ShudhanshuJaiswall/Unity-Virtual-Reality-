using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneZombies : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> clonePoints;
    public GameObject enemyPrefab, enemies;
    public float enemyBurstCount = 3, swanTime = 1;
    Transform location;
    float updateTime;
    void Start()
    {
        foreach (Transform child in transform)
            clonePoints.Add(child);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > updateTime)
        {
            updateTime = Time.time + swanTime;
            CloneEnemy();
        }

    }
    public void CloneEnemy()
    {
        if (enemies.transform.childCount < enemyBurstCount)
        {
            location = clonePoints[Random.Range(0, transform.childCount)];
            var enemyInstance = Instantiate(enemyPrefab, location);
            enemyInstance.transform.SetParent(enemies.transform);
            enemyInstance.transform.LookAt(Vector3.zero);
        }
    }
}

