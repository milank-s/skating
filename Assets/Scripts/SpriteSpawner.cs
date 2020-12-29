using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpawner : MonoBehaviour
{
    public Sprite[] sprites;
    public float spawnFrequency;
    public float spawnDistance;
    private float timer;
    private int index;
    
    public GameObject spritePrefab;
    private SpriteRenderer curSprite;

    private Vector3 lastSpawnPos;
    private Vector3 curPos;

    int curIndex;
    private bool stopped = true;
   
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastSpawnPos, transform.position) > spawnDistance)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            SpawnSprite(sprites);
        }
    }


    void SpawnSprite(Sprite[] sprites)
    {
        
        curSprite = Instantiate(spritePrefab, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        
        
        int newIndex = Random.Range(0, sprites.Length);

        while (curIndex == newIndex)
        {
            newIndex = Random.Range(0, sprites.Length);
        }
        
        curIndex = newIndex;
        curSprite.sprite = sprites[newIndex];
        timer = spawnFrequency;
        lastSpawnPos = transform.position;
    }
}
