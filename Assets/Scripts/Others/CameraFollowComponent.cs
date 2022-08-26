using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowComponent : MonoBehaviour
{
    private Transform objectToFollow;
    [SerializeField] private float followDistance;
    [SerializeField] private float followSpeed;

    void Start()
    {
        objectToFollow = ShipManager.Main.transform;

        // Vector3 direction = objectToFollow.position - transform.position + new Vector3(0, 0, transform.position.z);
        // StartCoroutine(Follow(direction));
    }

    private IEnumerator Follow(Vector3 direction)
    {
        while(!Mathf.Approximately(direction.magnitude, 0))
        {
            transform.position += direction * followSpeed;
            yield return new WaitForSeconds(.01f);
            direction = objectToFollow.position - transform.position + new Vector3(0, 0, transform.position.z);
        }

        StopCoroutine("Follow");
    }

    void Update()
    {
        var position = objectToFollow.position;
        position.z = transform.position.z;

        transform.position = position;

        // Vector3 direction = objectToFollow.position - transform.position + new Vector3(0, 0, transform.position.z);
        // if(direction.magnitude >= followDistance && !following)
        // {
        //     following = true;
        //     StartCoroutine(Follow(direction));
        // }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
