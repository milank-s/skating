using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    [SerializeField] SpriteRenderer r;
    public float fadeSpeed;
    private float timer;

    private void Start()
    {
        timer = fadeSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        Color c = r.color;
        c.a = timer / fadeSpeed;
        r.color = c;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
    
    
}
