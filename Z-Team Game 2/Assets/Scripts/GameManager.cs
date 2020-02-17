using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // The force due to gravity
    public const float GRAVITY = 10f;

    public Pole pole;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        pole.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
