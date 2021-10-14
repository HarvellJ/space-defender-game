using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameCharacter
{
    private Vector3 moveDirection = Vector3.zero;
    CharacterController characterController;

    //configurables
    public float speed = 10.0f;
    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    public GameObject shieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.IsOnShip())
        {
            this.IsAlive = false;
            this.GetComponent<Rigidbody>().isKinematic = false; // if player falls off ship, game over
            this.GetComponent<Rigidbody>().AddForce(this.transform.forward*0.2f);
        }
        else
        {
            // handle the player direction
            this.SetPlayerDirection();
            // handle player movement with keys
            float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            Vector3 RIGHT = transform.TransformDirection(Vector3.right);
            Vector3 FORWARD = transform.TransformDirection(Vector3.forward);
            transform.localPosition += RIGHT * h;
            transform.localPosition += FORWARD * v;

            // check for additional input
            if (Input.GetMouseButtonDown(0))
                this.Attack();
            if (Input.GetMouseButton(1))
                this.SpawnShield();
            if (Input.GetKeyDown("1"))
                this.ActivatePrimaryWeapon();
            if (Input.GetKeyDown("2"))
                this.ActivateSecondaryWeapon();
        }
    }

    private bool IsOnShip()
    {
       return Physics.Raycast(transform.position, - Vector3.up, this.GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    private void SpawnShield()
    {
        if (!ShieldAlreadyExists())
        {
            Vector3 playerPos = transform.position;
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            Vector3 spawnPosition = playerPos + playerDirection * 2;
            Instantiate(shieldPrefab, spawnPosition, transform.rotation);
        }
    }

    private bool ShieldAlreadyExists()
    {
        GameObject shield = GameObject.Find("Shield(Clone)");
        if (shield != null)
            return true;
        else
            return false;
    }

    private void SetPlayerDirection()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15 * Time.deltaTime);
        }
    }
}
