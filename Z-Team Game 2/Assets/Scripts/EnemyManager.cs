using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject BounceEnemyPrefab { get => bounceEnemyPrefab; }
    public GameObject StickEnemyPrefab { get => stickEnemyPrefab; }

    [SerializeField]
    private GameObject bounceEnemyPrefab;
    [SerializeField]
    private GameObject stickEnemyPrefab;
   
    private float time;
    private float spawnTime;
    private AudioSource enemyEnter1;
    private AudioSource enemyEnter2;
    private AudioSource enemyEnter3;
    AudioSource[] audioManager;

    private void Awake()
    {
        enemyEnter1 = GameObject.Find("enemyEnter1").GetComponent<AudioSource>();
        enemyEnter2 = GameObject.Find("enemyEnter2").GetComponent<AudioSource>();
        enemyEnter3 = GameObject.Find("enemyEnter3").GetComponent<AudioSource>();
        audioManager = new AudioSource[3];
        audioManager[0] = enemyEnter1;
        audioManager[1] = enemyEnter2;
        audioManager[2] = enemyEnter3;
    }


    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        spawnTime = 1.5f;
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
                Destroy(deadEnemy.transform.parent.gameObject);
            }
        }
    }

    private void SpawnEnemy()
    {
        int side = Random.Range(0, 2);
        int enterSound = Random.Range(0, 3);

        float xBound = (Camera.main.orthographicSize) * ((float)Screen.width / Screen.height) + 2;

        GameObject prefab = bounceEnemyPrefab;
        if (Random.Range(0, 4) == 0)
            prefab = stickEnemyPrefab;

        if (side == 0)
        {
            GameObject enemySpawn = GameObject.Instantiate(prefab, new Vector3(-xBound, 0.0f, 0.0f), Quaternion.identity);
            enemySpawn.transform.Rotate(Vector3.forward, Random.Range(-90, 90));
            enemySpawn.GetComponentInChildren<Enemy>().Side = 0;
        }
        else
        {
            GameObject enemySpawn = GameObject.Instantiate(prefab, new Vector3(xBound, 0.0f, 0.0f), Quaternion.identity);
            enemySpawn.transform.Rotate(Vector3.forward, Random.Range(-90, 90));
            enemySpawn.GetComponentInChildren<Enemy>().Side = 1;
        }

        audioManager[enterSound].Play();
    }

}
