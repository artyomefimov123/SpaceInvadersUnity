using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Speed = 20f;
    public Vector3 Direction = Vector3.up;
    public Action<Gun> Destroyed;



    public void Update()
    {
        this.transform.position += this.Direction * this.Speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Destroyed != null)
        {
           Destroyed.Invoke(this);
        }
        Destroy(this.gameObject);
    }

}
