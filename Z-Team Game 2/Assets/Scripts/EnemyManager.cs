using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float time;
    private float spawnTime;
    public GameObject enemyPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        spawnTime = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<GameManager>().CurrentState == GameState.Playing)
        {
            time += Time.deltaTime;
            if (time >= spawnTime)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        float side = Random.Range(0.0f, 1.0f);

        if (side <= .499999f)
        {
            GameObject enemySpawn = GameObject.Instantiate(enemyPrefab, new Vector3(-11.5f, 0.0f, 0.0f),Quaternion.identity);
            enemySpawn.GetComponent<Enemy>().side = 0;
        }
        else
        {
            GameObject enemySpawn = GameObject.Instantiate(enemyPrefab, new Vector3(11.5f, 0.0f, 0.0f), Quaternion.identity);
            enemySpawn.GetComponent<Enemy>().side = 1;
        }
        time = 0.0f;
    }

}
