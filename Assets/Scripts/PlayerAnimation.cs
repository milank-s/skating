using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class frame{
    public Sprite[] sprites; //list of sprites in various orientations
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] frame[] frames;
    public Billboard billboardScript;

    [SerializeField] float timeBetweenFrames = 0.05f;

    int orientation = 0;

    int targetFrame;
    int currentFrame;
    float lastFrameTimestamp;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentFrame = 0;
        targetFrame = 0;
        lastFrameTimestamp = Time.time;
        spriteRenderer.sprite = frames[currentFrame].sprites[orientation];
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetTargetFrame(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetTargetFrame(2);
        }

        int faceUp = billboardScript.deltaAverage.y > 0 ? 0 : 1;
        if(faceUp == 0){
            Debug.Log("Back");
        }else{
            Debug.Log("Forward");
        }

        SetOrientation(faceUp);
        spriteRenderer.sprite = frames[currentFrame].sprites[orientation];

        if (currentFrame != targetFrame)
        {
            if (Time.time- lastFrameTimestamp > timeBetweenFrames)
            {
                currentFrame = (currentFrame + 1) % frames.Length;
                lastFrameTimestamp = Time.time;
            }
       }
    }

    public void PumpLeg(){
        if(targetFrame == 0){
            SetTargetFrame(2);
        }else{
            SetTargetFrame(0);
        }
    }
    
    private void TargetFrameChanged()
    {
        if (targetFrame != currentFrame)
        {
            Debug.Log("frame changed!!");
            lastFrameTimestamp = Time.time;
        }
    }

    public void SetTargetFrame(int frame)
    {
        targetFrame = frame;
        TargetFrameChanged();
    }

    public void IncrementFrameBy(int increment)
    {
        targetFrame = (currentFrame + 2) % frames.Length;
        TargetFrameChanged();
    }

    public void SetOrientation(int o)
    {
        orientation = o;
    }
}
