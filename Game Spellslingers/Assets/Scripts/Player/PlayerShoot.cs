using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * A class that is resposible for shooting.
 * </summary>
 */
public class PlayerShoot : Shoot
{
    private int projectileCount;
    private float rate;
    private WaitForSeconds wait;

    private Vector3 target;
    private GameObject playerObject;

    public override void Start()
    {
        base.Start();
        this.playerObject = Player.instance.gameObject;
        this.rate = 1.8f;
        this.wait = new WaitForSeconds(this.rate);
        this.projectileCount = 1;
        StartCoroutine(Fire());
    }

    /**
     * <summary>
     * Fire a projectile based on where the cursor
     * is pointing at.
     * </summary>
     */
    public override IEnumerator Fire()
    {
        while (true)
        {
            Dictionary<GameObject, bool> seen = new Dictionary<GameObject, bool>();
            for (int i = 0; i < this.projectileCount; i++)
            {
                //GameObject projectile = this.projectilePool.Get();
                GameObject projectile = GetProjectile();
                
                if (projectile != null)
                {
                    while (seen.ContainsKey(projectile))
                    {
                        //projectile = this.projectilePool.Get();
                        projectile = GetProjectile();
                    }
                    // Position, direction and speed variables to fire projectile.
                    projectile.transform.position = playerObject.transform.position;
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    float speed = projectile.GetComponent<Projectile>().Speed;

                    // Arranging the position of projectiles to be fired all at once.
                    float coordinate = (float) (Math.Ceiling((double) i / 2));
                    coordinate = i % 2 == 0 ? coordinate : -1 * coordinate;

                    target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, transform.position.z));

                    // add rotation for additional arrows
                    Vector3 difference = target - playerObject.transform.position;
                    float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                    rotationZ += (float)coordinate * 20;

                    float distance = difference.magnitude;
                    
                    // rotate direction of force added to additional arrows
                    Vector2 direction = difference / distance;
                    direction = Rotate(direction, coordinate * 20 * Mathf.Deg2Rad);
                    direction.Normalize();
                    projectile.transform.rotation = Quaternion.Euler(0, 0, rotationZ + 180);
                    rb.AddForce(direction * speed, ForceMode2D.Impulse);
                }
                seen.Add(projectile, true);
            }
            yield return this.wait;
        }
    }

    public void AddProjectiles()
    {
        this.projectileCount++;
        playerObject.GetComponent<Archer>().Projectiles += 1;
    }

    public void IncreaseRate(float secs)
    {
        this.rate -= secs;
        this.wait = new WaitForSeconds(this.rate);
    }

    public void DecreaseRate(float secs)
    {
        this.rate += secs;
        this.wait = new WaitForSeconds(this.rate);
    }

    private Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}