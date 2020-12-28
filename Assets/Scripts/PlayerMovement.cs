using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float baseRadius;
    [SerializeField] Vector2 radiusRange;
    [SerializeField] float easing;
    [SerializeField] float rotationSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [SerializeField] float stopSpeed;
    [SerializeField] float frequency = 2;
    [SerializeField] float amplitude = 2;
    
    [SerializeField] float velocity;
    [SerializeField] float turnSpeed;
    [SerializeField] Text text;

    [SerializeField] TrailRenderer trail;
    [SerializeField] SkateTrail skatingTrail;
    
    float lastSineVal;
    float angle;
    float radius;
    float radiusTarget;

    // Start is called before the first frame update
    void Start()
    {
        radius = baseRadius;
        radiusTarget = baseRadius;
    }

    // Update is called once per frame
    void Update()
    { 
       FreeMovement();
    }

    void FreeMovement(){
        float rotation = Input.GetAxis("Horizontal");
        float forwardMotion = Input.GetAxis("Vertical");
        float rotationAmount = Mathf.Abs(rotation);
        
        //float sineOffset = Mathf.Sin(Time.time * frequency);
        
        float sineOffset = Mathf.PingPong(Time.time * frequency, 2);
        sineOffset -= 1;
        float delta = sineOffset - lastSineVal;


        if(forwardMotion <= 0){
            if(!trail.emitting){
                RecordMark();
            }
        }else{
            if(delta < 0){
            if(sineOffset < 0 && !trail.emitting){
                RecordMark();
            }
            if(sineOffset > 0 && trail.emitting){
                LeaveMark();
            }
        }else{
            if(sineOffset > 0 && !trail.emitting){
                RecordMark();
            }
            if(sineOffset < 0 && trail.emitting){
                LeaveMark();
            }
        }
        }
        
        lastSineVal = sineOffset;


    if(forwardMotion > 0){
        velocity += acceleration * (1-rotationAmount) * Time.deltaTime * forwardMotion;
    }else{
        velocity -= rotationAmount * Time.deltaTime * decceleration;
        velocity -= stopSpeed * -forwardMotion * Time.deltaTime;
    }

        velocity = Mathf.Clamp(velocity, 0, moveSpeed);
        sineOffset = sineOffset * (1-rotationAmount) * amplitude;
        float turnSpeedTuned = Mathf.Lerp(turnSpeed/3, turnSpeed, 1-(velocity/moveSpeed) - Mathf.Clamp01(forwardMotion));
        transform.Rotate(0, 0, -rotation * turnSpeedTuned * Time.deltaTime);
        transform.Rotate(0, 0, sineOffset * Time.deltaTime * amplitude * Mathf.Clamp01(forwardMotion));

        transform.position += transform.up * velocity * Time.deltaTime;
    }

    void RecordMark(){
        trail.Clear();
        trail.emitting = true;
    }

    void LeaveMark(){
        trail.emitting = false;
        Vector3[] linePoints = new Vector3[trail.numPositions];
        trail.GetPositions(linePoints);
        SkateTrail newTrail = Instantiate(skatingTrail, transform.position, Quaternion.identity);
        newTrail.Initialise(linePoints, 5);
        trail.Clear();
    }

    void CircularMovement(){
         if (Input.GetKey(KeyCode.RightArrow))
        {
            radiusTarget = radiusRange.y;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            radiusTarget = radiusRange.x;
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            radiusTarget = baseRadius;
        }

        radius = Mathf.Lerp(radius, radiusTarget, easing);

        angle = (angle + 1 * rotationSpeed * Time.deltaTime) % 360;
        text.text = angle.ToString();
        Vector2 pos;
        pos.x = center.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        pos.y = center.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        this.transform.position = new Vector3(pos.x, 0, pos.y);

    }
}
