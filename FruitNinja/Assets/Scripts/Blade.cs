using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private bool slicing;
    private Collider bladeCollider;
    private Camera mainCamera;
    public Vector3 direction { get; private set; }
    public float minSlicingVelocity = 0.01f;
    private TrailRenderer bladeTrail;
    public float sliceForce = 6;
    private Vector3 curPosition;
    void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            StartSlicing();
            transform.GetComponent<AudioSource>().Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if (slicing)
        {
            ContinueSlicing();
        }
#elif UNITY_ANDROID
       if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            StartSlicing();
            transform.GetComponent<AudioSource>().Play();
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            StopSlicing();
        }
        else if (slicing)
        {
            ContinueSlicing();
        }
#endif

    }
    private void OnEnable()
    {
        //StartSlicing();
    }
    private void OnDisable()
    {
        StopSlicing();
    }
    private void StartSlicing()
    {
#if UNITY_EDITOR
        curPosition = Input.mousePosition;
#elif UNITY_ANDROID
        curPosition = Input.GetTouch(0).position;
#endif
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(curPosition);
        newPosition.z = 0f;
        transform.position = newPosition;
        slicing = true;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false ;
        bladeTrail.Clear();
    }
    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }
    private void ContinueSlicing()
    {
#if UNITY_EDITOR
        curPosition = Input.mousePosition;
#elif UNITY_ANDROID
        curPosition = Input.GetTouch(0).position;
#endif
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(curPosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSlicingVelocity;
        bladeTrail.enabled = bladeCollider.enabled;
        transform.position = newPosition;
    }
}
