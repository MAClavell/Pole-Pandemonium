using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    TMPro.TMP_Dropdown difficultySelect;
    public AudioSource BGM;

    public AudioClip easy;
    public AudioClip medium;
    public AudioClip hard;

    // Start is called before the first frame update
    void Start()
    {
        difficultySelect = GameObject.FindGameObjectWithTag("DifficultySelect").GetComponent<TMPro.TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        difficultySelect.onValueChanged.AddListener(delegate {
            ChangeMusic(difficultySelect.value);
        });
    }

    public void ChangeMusic(int difficulty){
        if (difficulty == 0)
        {
            //Easy
            BGM.Stop();
            BGM.clip = easy;
            BGM.Play();
        }
        else if (difficulty == 1)
        {
            //Medium
            BGM.Stop();
            BGM.clip = medium;
            BGM.Play();
        }
        else if (difficulty == 2)
        {
            //Hard
            BGM.Stop();
            BGM.clip = hard;
            BGM.Play();
        }
    }
}
