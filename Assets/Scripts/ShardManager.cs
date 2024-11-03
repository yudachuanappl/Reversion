using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardManager : MonoBehaviour
{
    public float TimeInterval;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = TimeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            timer = TimeInterval;
            this.GetComponent<Animator>().SetTrigger("Play");
        }
    }
}
