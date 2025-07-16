using UnityEngine;

public class mover : MonoBehaviour
{
    public float distance = 0.2f;
    public float speed = 0.2f;
    bool up = true;
    float currentdistance;
    Vector3 position;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (up)
        {
            Vector3 newPos = transform.position;
            newPos.y += speed * Time.deltaTime;
            transform.position = newPos;
        }
        else
        {
            Vector3 newPos = transform.position;
            newPos.y -= speed * Time.deltaTime;
            transform.position = newPos;
        }

        if (position.y + distance < transform.position.y)
        {
            up = false;
        }

        if (position.y > transform.position.y) {
            up = true;
        }

    }
}
