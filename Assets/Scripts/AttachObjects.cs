using UnityEngine;

public class AttachObjects : MonoBehaviour
{
    public GameObject collidedObject;
    public GameObject existingCube; // Reference to the existing cube in the scene
    public GameObject newCubePrefab; // The new cube prefab to instantiate

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AttachPoint"))
        {
            existingCube = collision.gameObject;
            // Get the collision point and normal vector
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 collisionNormal = collision.contacts[0].normal;

            // Determine which side of the existing cube was hit based on the collision normal
            Vector3 spawnPosition = existingCube.transform.position;
            if (Mathf.Abs(collisionNormal.y) > Mathf.Abs(collisionNormal.x) && Mathf.Abs(collisionNormal.y) > Mathf.Abs(collisionNormal.z))
            {
                // Collision with the top or bottom side of the existing cube
                //Für später der offset kann erreich werden in dem wir die localscale mit z.b. 1.3 multiplizerein
                spawnPosition += collisionNormal.y > 0 ? existingCube.transform.up * (existingCube.transform.localScale.y) : -existingCube.transform.up * (existingCube.transform.localScale.y);
            }
            else if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y) && Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.z))
            {
                // Collision with the left or right side of the existing cube
                spawnPosition += collisionNormal.x > 0 ? existingCube.transform.right * (existingCube.transform.localScale.x) : -existingCube.transform.right * (existingCube.transform.localScale.x);
            }
            else
            {
                // Collision with the front or back side of the existing cube
                spawnPosition += collisionNormal.z > 0 ? existingCube.transform.forward * (existingCube.transform.localScale.z) : -existingCube.transform.forward * (existingCube.transform.localScale.z);
            }

            // Instantiate the new cube at the determined position
            Instantiate(newCubePrefab, spawnPosition, Quaternion.identity);
        }
    }
}

