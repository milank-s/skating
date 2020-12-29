using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameByFrame : MonoBehaviour
{
    public bool randomize;
    public Sprite[] frames;
    public SpriteRenderer r;

    public float framerate = 6;

    int index;
    float timer;

void Start(){
  timer = Random.Range(0, 1f/framerate);
}
    void Update(){

        if(timer < 0){
          // r.flipX = index %2 == 0;
          timer = 1f/framerate;
          if(randomize){
            index = Random.Range(0, frames.Length);
          }else{
            index++;
          }
          r.sprite = frames[index%frames.Length];

        }

        timer -= Time.deltaTime;

    }
}
