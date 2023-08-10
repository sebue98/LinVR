using UnityEngine;

public class StartObjectPoint : MonoBehaviour
{
    public GameObject replacementObject;

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
        if (collision.gameObject.CompareTag("Molecule"))
        {

            Vector3 position = gameObject.transform.position;
            Quaternion rotation = gameObject.transform.rotation;

            Destroy(gameObject);
            Destroy(collision.gameObject);

            GameObject newObject = Instantiate(replacementObject, position, rotation);
            ChainMaster.Instance.carbons.Add(new Carbon());
        }
    }
}

