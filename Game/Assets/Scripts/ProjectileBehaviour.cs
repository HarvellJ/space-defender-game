using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public int damage = 1; // default to 1

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag.ToUpper().Equals("DAMAGINGPROJECTILE"))
        {
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(ActivateCollider());
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
    }

    public IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("hit " + other.name);

        if (other.tag.Contains("Enemy") || other.name.Equals("DefensePoint") || other.name.Contains("Shield"))
            Destroy(gameObject);
    }
}
