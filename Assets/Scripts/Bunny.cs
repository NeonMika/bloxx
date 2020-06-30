using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Carrot currentTarget;
    public float speed = 1.0f;
    public float rotateSpeed = 20.0f;

    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            for (int radius = 1; radius <= 64; radius *= 2)
            {
                Collider[] collisions = Physics.OverlapSphere(rigidbody.transform.position, radius);
                foreach (Collider possibleFood in collisions)
                {
                    if (possibleFood.GetComponentInChildren<Carrot>() != null)
                    {
                        currentTarget = possibleFood.GetComponentInChildren<Carrot>();
                        goto found;
                    }
                }
            }
        }


        found:
        if (transform.position.y < -1000)
        {
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        if (currentTarget != null)
        {
            if (grounded && rigidbody.velocity.y <= 0.01f)
            {
                if (Physics.Raycast(transform.position, transform.forward, transform.lossyScale.z * 2.0f))
                {
                    rigidbody.AddForce(Vector3.up * 15f, ForceMode.Impulse);
                    rigidbody.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
                }
            }

            if (Vector3.Distance(transform.position, currentTarget.transform.position) > 1.0f)
            {
                //if (rigidbody.velocity.magnitude < 0.0001f)
                //{
                //    rigidbody.AddForce(-rigidbody.transform.forward * 10.0f, ForceMode.Impulse);
                //    Debug.Log("BACK!");
                //}
                //else
                {
                    if (grounded)
                    {
                        Vector3 heightlessPos = new Vector3(rigidbody.transform.position.x, 0,
                            rigidbody.transform.position.z);
                        Vector3 heightlessTarget = new Vector3(currentTarget.transform.position.x, 0,
                            currentTarget.transform.position.z);

                        Quaternion targetRotation = Quaternion.LookRotation(heightlessTarget - heightlessPos);
                        rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation,
                            rotateSpeed * Time.deltaTime));
                        if (rigidbody.velocity.magnitude < speed)
                        {
                            rigidbody.AddForce(rigidbody.transform.forward * 20f, ForceMode.Force);
                        }
                    }
                }
            }
            else
            {
                Destroy(currentTarget.gameObject);
                currentTarget = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (currentTarget != null)
        {
            Gizmos.DrawLine(rigidbody.transform.position, currentTarget.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        grounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        grounded = false;
    }
}