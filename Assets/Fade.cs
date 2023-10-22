using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color tmp = sprite.color;
        if(tmp.a > 0){
            tmp.a -= 0.1f * Time.deltaTime;
        }
        sprite.color = tmp;
    }
}
