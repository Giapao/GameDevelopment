using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueEvade : MonoBehaviour
{
    public GameObject character;
    public GameObject target;

    public bool evadeMode = false;

    private Vector3 velocity;
    public float Mass = 15;
    public float MaxVelocity = 3;
    public float MaxForce = 15;
    
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredVelocity = target.transform.position - character.transform.position;

        float pred = desiredVelocity.magnitude / MaxVelocity;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity * pred;

        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        velocity = Vector3.ClampMagnitude(velocity +steering, MaxVelocity);

        if (evadeMode == false)
            character.transform.position += velocity * Time.deltaTime;
        else
            character.transform.position += (-1 * velocity) * Time.deltaTime;

        character.transform.position = new Vector3 (character.transform.position.x, 1, character.transform.position.z);
        character.transform.forward = velocity.normalized;

    }
}
