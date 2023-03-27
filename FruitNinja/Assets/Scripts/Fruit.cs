using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    private Collider fruitCollider;
    private Rigidbody fruitRigidbody;
    private ParticleSystem juiceEffect;
    private int fallFruitNum;
    public int point = 1;
    private void Awake()
    {
        fruitCollider = GetComponent<Collider>();
        fruitRigidbody = GetComponent<Rigidbody>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
    }
    /// <summary>
    /// Ƭ
    /// </summary>
    private void Slices(Vector3 direction,Vector3 position,float force)
    {
        FindObjectOfType<GameManager>().InCreaseScore(point);
        whole.SetActive(false);
        sliced.SetActive(true);
        fruitCollider.enabled = false;
        juiceEffect.Play();
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0,0,angle);
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
        foreach (var slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var audio = transform.GetComponent<AudioSource>();
            audio.time += 0.3f;
            audio.Play();
            Blade blade = other.GetComponent<Blade>();
            Slices(blade.direction,blade.transform.position,blade.sliceForce);
        }
        else if (other.CompareTag("FruitSpawner"))
        {
            FindObjectOfType<GameManager>().InFallFruitNum();
        }
    }
}
