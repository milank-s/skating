﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    
[Header("Movement")]
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
    [SerializeField] float maxDistFromCenter = 7;
    [SerializeField] float returnToRinkSpeed = 10;
    [SerializeField] float velocity;
    [SerializeField] float turnSpeed;
    [SerializeField] Text text;

[Header("FX")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] SkateTrail skatingTrail;
    [SerializeField] ParticleSystem snowSpray;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip[] skateSFX;
    ParticleSystem.EmissionModule snowSprayEmission;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] PlayerAnimation animation;

    float trailTimer;
    float lastSineVal;
    float angle;
    float radius;
    float radiusTarget;

    // Start is called before the first frame update
    void Start()
    {
        snowSprayEmission = snowSpray.emission;
        radius = baseRadius;
        radiusTarget = baseRadius;
        velocity = 1;
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
            }else{
                trailTimer += Time.deltaTime;
                if(trailTimer > 5){
                    LeaveMark();
                    RecordMark();
                }
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
            
             snowSpray.Stop();
        }else{
            if(forwardMotion < -0.5f && velocity > 0.1f){
                if(!audio.isPlaying){
                   
                    
                    audio.PlayOneShot(skateSFX[Random.Range(0, skateSFX.Length)]);
                }

            if(!snowSpray.isPlaying){
                snowSpray.Play();
            }
                
            }else{
                
                snowSpray.Stop();
            }
            velocity -= rotationAmount * Time.deltaTime * decceleration;
            velocity -= stopSpeed * -forwardMotion * Time.deltaTime;
        }

        velocity = Mathf.Clamp(velocity, 0, moveSpeed);
        sineOffset = sineOffset * (1-rotationAmount) * amplitude;
        float turnSpeedTuned = Mathf.Lerp(turnSpeed/3, turnSpeed, 1-(velocity/moveSpeed) - Mathf.Clamp01(forwardMotion));

        Vector3 toPlayer = transform.position;
        float distanceFromCenter = toPlayer.magnitude;
        Vector3 perp;
        toPlayer.z = 0;
       
        perp = Vector3.Cross(Vector3.forward, toPlayer.normalized);
        perp.z = 0;

        if(distanceFromCenter > maxDistFromCenter){
            Quaternion toMiddleRot = Quaternion.LookRotation(Vector3.forward, -toPlayer);
            Quaternion perpRot = Quaternion.LookRotation(Vector3.forward, perp);
            Quaternion newDir = Quaternion.Lerp(perpRot, toMiddleRot, (distanceFromCenter-maxDistFromCenter) + 0.25f);
            transform.rotation = Quaternion.Lerp(transform.rotation, newDir, Time.deltaTime * returnToRinkSpeed);

        }else{
            transform.Rotate(0, 0, -rotation * turnSpeedTuned * Time.deltaTime);
            transform.Rotate(0, 0, sineOffset * Time.deltaTime * amplitude * (Mathf.Clamp01(forwardMotion) + 0.2f));
        }

        Vector3 deltaPos = transform.position;
        transform.position += transform.up * velocity * Time.deltaTime;
        deltaPos = transform.position - deltaPos;

    }

    void RecordMark(){
        trail.Clear();
        trail.emitting = true;
        trailTimer = 0;
        audio.PlayOneShot(skateSFX[Random.Range(0, skateSFX.Length)]);
    }

    void LeaveMark(){
        animation.PumpLeg();
        trailTimer = 0;
        trail.emitting = false;
        Vector3[] linePoints = new Vector3[trail.positionCount];
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
