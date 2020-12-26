using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float radius;
    [SerializeField] Text text;

    float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle = (angle + 1) % 360;
        text.text = angle.ToString() + " " + Mathf.Cos(Mathf.Deg2Rad*angle);
        Vector2 pos;
        pos.x = center.position.x + radius * Mathf.Cos(Mathf.Deg2Rad*angle);
        pos.y = center.position.y + radius * Mathf.Sin(Mathf.Deg2Rad*angle);
        this.transform.position = new Vector3(pos.x, 0, pos.y);
    }
}
