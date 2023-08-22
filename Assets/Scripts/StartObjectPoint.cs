using UnityEngine;

public class StartObjectPoint : MonoBehaviour
{
    public GameObject fixedObject;

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

            GameObject newObject = (GameObject)Instantiate(fixedObject, position, rotation);
            newObject.name = "Carbon" + ChainMaster.Instance.counter;
            ChainMaster.Instance.carbonGameObjects.Add(newObject);
            ChainMaster.Instance.counter++;
            Carbon temp = new Carbon();
            temp.moleculeName = newObject.name;
            ChainMaster.Instance.carbons.Add(temp);
        }
    }
}

