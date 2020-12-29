using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{


    float count = 0;
    float average = 10;
    [SerializeField] SpriteRenderer spriteRenderer;
    Vector3 lastPos;
    Vector3 deltaAverage;
    [SerializeField] Transform lookTarget;
    
    void Start(){
        lookTarget = Camera.main.transform;
    }

    void Update()
    {
        count++;
    
        Vector3 deltaPos = transform.position - lastPos;

        if(count < average){
            deltaAverage += deltaPos;
        }else{
            deltaAverage = deltaAverage + (deltaPos-deltaAverage)/(average+1);
            if(count == average){
                deltaAverage/= count;
            }
        }

        if(deltaAverage.x <= 0){
            spriteRenderer.flipX = true;
        }else{
            spriteRenderer.flipX = false;
        }

        lastPos = transform.position;
        transform.rotation = Quaternion.LookRotation(lookTarget.forward, Vector3.up);
    }
}
