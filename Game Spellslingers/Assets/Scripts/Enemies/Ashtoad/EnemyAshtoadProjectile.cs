using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAshtoadProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Transform player;
    private Vector2 target;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance.gameObject.transform;
        target = new Vector2(player.position.x, player.position.y);
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Hit", false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            anim.SetBool("Hit", true);
            Invoke("DestroyProjectile", 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player.instance.TakeDamage(10f);
            anim.SetBool("Hit", true);
            Invoke("DestroyProjectile", 0.5f);
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
