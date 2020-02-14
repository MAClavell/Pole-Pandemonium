using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The force due to gravity
    public static readonly float GRAVITY = 9.8f;

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
        pole.Update();
    }
}
