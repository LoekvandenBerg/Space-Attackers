using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;
    public float horizontalLimit = 2.5f;
    public float firingSpeed = 3f;
    public float firingCDDuration = 1f;
    public GameObject missilePrefab;
    public GameObject explosionPrefab;

    private bool fired = false;
    private float CDTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //moving player
        GetComponent<Rigidbody2D>().velocity = new Vector2(
            Input.GetAxis("Horizontal") * speed,
            0
            );

        //keep the plyer within bounds
        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector3(horizontalLimit, transform.position.y, transform.position.z);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }    
        if(transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        //fire missiles
        CDTimer -= Time.deltaTime;
        if (CDTimer <= 0 && Input.GetAxis("Fire1") == 1f)
        {
            CDTimer = firingCDDuration;

            fired = true;
            GameObject missileInstance = Instantiate(missilePrefab);
            missileInstance.transform.SetParent(transform.parent);
            missileInstance.transform.position = transform.position;
            missileInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, firingSpeed);
            Destroy(missileInstance, 2f);  
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyMissile" || collision.tag == "Enemy")
        {
            GameObject explosionInstance = Instantiate(explosionPrefab);
            explosionInstance.transform.SetParent(transform.parent);
            explosionInstance.transform.position = transform.position;
            Destroy(explosionInstance, 1.5f);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
