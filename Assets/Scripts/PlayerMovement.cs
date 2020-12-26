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
    [SerializeField] float speed;
    [SerializeField] Text text;

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

        angle = (angle + 1 * speed * Time.deltaTime) % 360;
        text.text = angle.ToString();
        Vector2 pos;
        pos.x = center.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        pos.y = center.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        this.transform.position = new Vector3(pos.x, 0, pos.y);

    }
}
