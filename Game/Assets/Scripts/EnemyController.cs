using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GameCharacter
{
    public int movementSpeed = 10; // default movement speed
    public GameObject TargetLocation { get => spawnPoint; set => spawnPoint = value; }
    public bool isHostile;

    private bool atTargetPoint;
    private GameObject spawnPoint;
    private GameObject defensePoint;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        defensePoint = GameObject.Find("DefensePoint");

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (!atTargetPoint)
        {
            this.MoveToTargetPoint();
        }
        else if (isHostile && GameObject.Find("DefensePoint") && GameObject.Find("Player") != null && GameObject.Find("Player").GetComponent<PlayerController>().IsAlive)
        {
            transform.LookAt(defensePoint.transform);
            // Handle combat
            UpdateCombatState();
            if (!attackOnCooldown)
            {
                Attack();
            }
        }
        else
        {
            this.TargetLocation = GameObject.Find("DistantEnemyFinish");
            this.atTargetPoint = false;
            this.isHostile = false; // this handles hostile enemy garbage collection after game over 
        }
    }

    private void MoveToTargetPoint()
    {
        float step = movementSpeed * Time.deltaTime; // calculate distance to move this frame
        transform.position = Vector3.MoveTowards(transform.position, TargetLocation.transform.position, step);
        transform.LookAt(TargetLocation.transform);

        if (transform.position.x == TargetLocation.transform.position.x &&
             transform.position.z == TargetLocation.transform.position.z)
        {
            atTargetPoint = true;

            if (!isHostile) // if the NPC has moved to target location and is not hostile, it is a background enemy and should be removed as no further action is required
            {
                Destroy(gameObject);
            }
        }
    }
}
