using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateTrail : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    
    public float lifeTime;
    float life;
    public void Initialise(Vector3[] points, float time){
        life = time;
        lifeTime = time;
        line.numPositions = points.Length;
        line.SetPositions(points);
    }

    void Update(){
        life -= Time.deltaTime;
        Color c = Color.white;
        c.a = (life/lifeTime);
        line.SetColors(c,c);
        if(lifeTime < 0){
            Destroy(gameObject);
        }
    }
}
