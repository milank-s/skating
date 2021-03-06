﻿using UnityEngine;
using System.Collections;

public class SkaterBoid : MonoBehaviour {

	public bool affectMovement = true;
	public float rotateSpeed;
	float distanceFromCenter;
	public float followCircleWeight = 5;
	public float minSeparationLength;
	public float separationSphereRadius;
	public float separationWeight;
	public float cohesionSphereRadius;
	public float cohesionWeight;
	public float alignmentSphereRadius;
	public float alignmentWeight;
	public float maxSpeed = 3;
	public float minSpeed = 0.5f;
	public float speedConst;
	public float angle;

	float curAngle;
	Vector2 desiredPosition;
	
	Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
		distanceFromCenter = Vector3.Distance(transform.position, Vector3.zero);
		speedConst = Mathf.Lerp(maxSpeed, minSpeed, distanceFromCenter/7);
		curAngle = GetAngle();
		rigid = transform.GetComponent<Rigidbody2D>();
		rigid.velocity = new Vector2(Random.Range(-speedConst, speedConst), Random.Range(-speedConst, speedConst));
	}

	bool checkAngle(Vector2 speedVect, Vector2 pos, Vector2 otherPos, float angle){
		bool ret = false;
		Vector2 target = otherPos-pos;

		if(Vector2.Angle(speedVect, target) <= angle){
			ret = true;
		}

		return ret;

	}

	Collider2D[] filterByAngle(float angle, Vector2 speedVect, Vector2 pos, Collider2D[] colls){
		Collider2D[] ret;

		ArrayList temp = new ArrayList();

		for(int i = 0; i < colls.Length; i++){
			if(checkAngle(speedVect, pos, colls[i].gameObject.transform.position, angle) && colls[i].GetComponent<Rigidbody2D>()){
				temp.Add(colls[i]);
			}
		}

		ret = (Collider2D[])temp.ToArray(typeof(Collider2D));

		return ret;

	}

	Vector2 calculateSeparation(Vector2 position, Collider2D[] sepColl, float minSepLen,float sepWeight){
		Vector2 ret = new Vector2(0f, 0f);

		for(int i = 0; i < sepColl.Length; i++){
			Vector2 distance = new Vector2(0f, 0f);
			distance = ((Vector2)(sepColl[i].gameObject.transform.position) - position);

			if(distance.magnitude <= minSepLen){
				distance = (distance - distance.normalized*minSepLen);
				ret += sepWeight*distance;
			}
		}

		return ret;
	}

	Vector2 calculateCohesion(Vector2 position, Collider2D[] cohColl, float cohWeight){
		Vector2 ret = new Vector2(0f, 0f);

		Vector2 avPos = new Vector2(0f, 0f);
		bool playerTouched = false;
		int playerVal = 0;

		for(int i = 0; i < cohColl.Length; i++){
			avPos += (Vector2)cohColl[i].gameObject.transform.position;
		}

		avPos = avPos/(float)cohColl.Length;
		ret = (avPos - position) * cohWeight;
		
		return ret;
	}

	Vector2 calculateAlignment(Vector2 position, Collider2D[] alColl, float alWeight){
		Vector2 ret = new Vector2(0f, 0f);

		bool playerTouched = false;
		bool wallTouched = false;
		int playerVal = 0;

		for(int i = 0; i < alColl.Length; i++){
			
			ret += alColl [i].gameObject.GetComponent<Rigidbody2D> ().velocity;

		}

		ret = ret/(float)alColl.Length;
		return ret*alWeight;
	}
	
	void OnDrawGizmos()
{
    Gizmos.color = new Color(0.5f, 0.5f, 0.5f);
    Gizmos.DrawSphere(desiredPosition, 0.1f);
	Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 0.25f));
	Gizmos.DrawLine(transform.position, desiredPosition);
}

      public float GetAngle()
     {
         Vector2 direction = (Vector2)transform.position - Vector2.zero;
         float angle = Mathf.Atan2(direction.y,  direction.x) * Mathf.Rad2Deg;
         if (angle < 0f) angle += 360f;
         return angle;
     }


	// Update is called once per frame
	void Update () {

		if(affectMovement){
		//TODO Add force based on curvature of rink

		curAngle = GetAngle();
 		curAngle = (curAngle + 1 * rotateSpeed * Time.deltaTime);
        Vector2 pos;
        pos.x = distanceFromCenter * Mathf.Cos(Mathf.Deg2Rad * curAngle);
        pos.y = distanceFromCenter * Mathf.Sin(Mathf.Deg2Rad * curAngle);
        desiredPosition = new Vector2(pos.x, pos.y);
		Vector2 toDesiredPos = (Vector2)desiredPosition - (Vector2)transform.position;

		Collider2D[] separationCollision = filterByAngle(angle, rigid.velocity, transform.position,
											Physics2D.OverlapCircleAll(transform.position, separationSphereRadius));
		Collider2D[] cohesionCollision = filterByAngle(angle, rigid.velocity, transform.position, 
											Physics2D.OverlapCircleAll(transform.position, cohesionSphereRadius));
		Collider2D[] alignmentCollision = filterByAngle(angle, rigid.velocity, transform.position,
											Physics2D.OverlapCircleAll(transform.position, alignmentSphereRadius));
		Vector2 newVel = new Vector2(0f, 0f);

		newVel += calculateSeparation(transform.position, separationCollision, minSeparationLength, separationWeight);
		newVel += calculateCohesion(transform.position, cohesionCollision, cohesionWeight);
		newVel += calculateAlignment(transform.position, alignmentCollision, alignmentWeight);
        newVel += toDesiredPos.normalized * followCircleWeight;

		rigid.velocity = (newVel.normalized+rigid.velocity.normalized).normalized*speedConst;
		//transform.up = rigid.velocity;
		}
	}
}
