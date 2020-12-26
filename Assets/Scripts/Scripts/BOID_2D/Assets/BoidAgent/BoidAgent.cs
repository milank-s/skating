using UnityEngine;
using System.Collections;

public class BoidAgent : MonoBehaviour {

	public float minSeparationLength;
	public float separationSphereRadius;
	public float separationWeight;
	public float cohesionSphereRadius;
	public float cohesionWeight;
	public float alignmentSphereRadius;
	public float alignmentWeight;
	public float speedConst;
	public float angle;

	Rigidbody2D rigid;

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
			if (cohColl [i].gameObject.tag == "Player") {
				playerTouched = true;
				playerVal = i;
			}
			avPos += (Vector2)cohColl[i].gameObject.transform.position;
		}

		avPos = avPos/(float)cohColl.Length;
		if (playerTouched) {
			avPos = Vector2.Lerp (avPos, cohColl [playerVal].transform.position, 0.8f);
		}
		if (playerTouched) {
			ret = (avPos - position) * cohWeight * 2;
		} else {
			ret = (avPos - position) * cohWeight;
		}

		return ret;
	}

	Vector2 calculateAlignment(Vector2 position, Collider2D[] alColl, float alWeight){
		Vector2 ret = new Vector2(0f, 0f);


		bool playerTouched = false;
		bool wallTouched = false;
		int playerVal = 0;

		for(int i = 0; i < alColl.Length; i++){
			if (alColl [i].gameObject.tag == "Player") {
				playerTouched = true;
				playerVal = i;
			}else{
				ret += alColl [i].gameObject.GetComponent<Rigidbody2D> ().velocity;
			}
			if (alColl [i].gameObject.tag == "gameWall") {
				wallTouched = true;
			}
		}

		ret = ret/(float)alColl.Length;
		if (playerTouched) {
			ret = Vector2.Lerp (ret, alColl [playerVal].gameObject.GetComponent<Rigidbody2D> ().velocity, 0.8f);
		}
		if (wallTouched) {
			ret = Vector2.zero - (Vector2)transform.position;
		}
		return ret*alWeight;
	}

	// Use this for initialization
	void Start () {
		rigid = transform.GetComponent<Rigidbody2D>();
		rigid.velocity = new Vector2(Random.Range(-speedConst, speedConst), Random.Range(-speedConst, speedConst));
	}
	
	// Update is called once per frame
	void Update () {
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
		rigid.velocity = (newVel.normalized+rigid.velocity.normalized).normalized*speedConst;
		transform.up = rigid.velocity;
	}
}
