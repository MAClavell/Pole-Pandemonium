using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bounceEnemyPrefab;
    [SerializeField]
    private GameObject stickEnemyPrefab;
   
    private float time;
    private float spawnTime;
    

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

        if (transform.GetComponent<GameManager>().CurrentState == GameState.GameOver)
        {
            foreach (var deadEnemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(deadEnemy);
            }
        }
    }

    private void SpawnEnemy()
    {
        float side = Random.Range(0.0f, 1.0f);

        GameObject prefab = bounceEnemyPrefab;
        if (Random.Range(0, 10) == 0)
            prefab = stickEnemyPrefab;

        if (side <= .499999f)
        {
            GameObject enemySpawn = GameObject.Instantiate(prefab, new Vector3(-11.5f, 0.0f, 0.0f),Quaternion.identity);
            enemySpawn.transform.Rotate(Vector3.forward, Random.Range(-90, 90));
            enemySpawn.GetComponent<Enemy>().Side = 0;
        }
        else
        {
            GameObject enemySpawn = GameObject.Instantiate(prefab, new Vector3(11.5f, 0.0f, 0.0f), Quaternion.identity);
            enemySpawn.transform.Rotate(Vector3.forward, Random.Range(-90, 90));
            enemySpawn.GetComponent<Enemy>().Side = 1;
        }
        time = 0.0f;
    }

}
