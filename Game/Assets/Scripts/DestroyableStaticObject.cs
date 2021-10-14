using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableStaticObject : MonoBehaviour
{
    public int MaxHp;
    public GameObject explosionPrefab;
    public GameObject smokePreab;
    public GameObject healPrefab;

    private int hp;

    public int Hp { get => hp; }

    // Start is called before the first frame update
    void Start()
    {
        this.hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.ToUpper().Equals("DAMAGINGPROJECTILE"))
        {
            // handle damage
            this.hp -= collider.GetComponent<ProjectileBehaviour>().damage;
                //DamageCalculator.GetDamageByAmmoType(collider.name);

            // handle impact particle effects
            Vector3 SpawnHere = collider.transform.position;
            Instantiate(explosionPrefab, SpawnHere, collider.transform.rotation);

            if (this.hp < (MaxHp * 0.2)) // show smoke when HP below 20% to indicate damage
                Instantiate(smokePreab, SpawnHere, collider.transform.rotation).transform.parent = transform; // add to parent so it's destroyed along with it

            // check it's still alive...
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (collider.tag.ToUpper().Equals("HEAL"))
        {
            this.hp += collider.GetComponent<ProjectileBehaviour>().damage;
          
            // handle impact particle effects
            Vector3 SpawnHere = collider.transform.position;
            Instantiate(healPrefab, SpawnHere, collider.transform.rotation);
        }
    }
}
