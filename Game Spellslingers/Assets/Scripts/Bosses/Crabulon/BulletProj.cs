using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProj : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Transform player;
    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance.gameObject.transform;
        target = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player.instance.TakeDamage(40f);
            DestroyProjectile();
        }   
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
