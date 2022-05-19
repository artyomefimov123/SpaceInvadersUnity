using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed = 5f;
    public Gun LaserPrefab;
    public Action Killed;
    public bool LaserActive;

    private void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= PlayerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += PlayerSpeed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!LaserActive)
        {
            LaserActive = true;

            Gun laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            laser.Destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed(Gun laser)
    {
        LaserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rocket") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            if (Killed != null)
            {
                Killed.Invoke();
            }
        }
    }
}
