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
                time -= spawnTime;
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
        int side = Random.Range(0, 2);

        float xBound = (Camera.main.orthographicSize * 2) + 2;

        GameObject prefab = bounceEnemyPrefab;
        if (Random.Range(0, 10) == 0)
            prefab = stickEnemyPrefab;

        if (side == 0)
        {
            GameObject enemySpawn = GameObject.Instantiate(prefab, new Vector3(-xBound, 0.0f, 0.0f), Quaternion.identity);
            enemySpawn.transform.Rotate(Vector3.forward, Random.Range(-90, 90));
            enemySpawn.GetComponent<Enemy>().Side = 0;
        }
        else
        {
            GameObject enemySpawn = GameObject.Instantiate(prefab, new Vector3(xBound, 0.0f, 0.0f), Quaternion.identity);
            enemySpawn.transform.Rotate(Vector3.forward, Random.Range(-90, 90));
            enemySpawn.GetComponent<Enemy>().Side = 1;
        }
    }

}
