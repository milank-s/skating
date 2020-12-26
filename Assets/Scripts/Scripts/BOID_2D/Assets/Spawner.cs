using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public float speed;
	public int amount;
	public GameObject spawn;
	public string tag;
	public Color boidColor;
	public string boidName;
	private List<GameObject>boids;
	public List<GameObject>team1;
	public List<GameObject>team2;

	private static Spawner instance = null;

	public static Spawner Instance {
		get { 
			return instance;
		}
	}

	// Use this for initialization
	void Start () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		boids = new List<GameObject> ();
		team1 = new List<GameObject> ();
		team2 = new List<GameObject> ();
//		StartCoroutine (Spawn ());
	}
	
	// Update is called once per frame
	void Update () {
		if(boids.Count < amount){
			float sign = amount % 2 == 0 ? -1 : 1;
			Vector2 pos = transform.position;
			GameObject newBoid = GameObject.Instantiate(spawn, pos, new Quaternion())as GameObject;
			newBoid.tag = tag;
			newBoid.GetComponentInChildren<TextMesh> ().text = boidName;
			newBoid.GetComponentInChildren<TextMesh> ().color = boidColor;
			boids.Add (newBoid);
		}
	}

	IEnumerator Spawn(){
		while(boids.Count < amount){
			float sign = amount % 2 == 0 ? -1 : 1;
			Vector2 pos = Vector2.up * sign;
			GameObject newBoid = GameObject.Instantiate(spawn, pos, new Quaternion())as GameObject;
			newBoid.tag = tag;
			newBoid.GetComponentInChildren<TextMesh> ().text = boidName;
			newBoid.GetComponentInChildren<TextMesh> ().color = boidColor;
			boids.Add (newBoid);
			yield return new WaitForSeconds (speed);
		}
	}

	public void delete(GameObject g){
		boids.Remove (g);
		Destroy (g);
	}
}
