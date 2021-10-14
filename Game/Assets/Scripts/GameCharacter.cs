using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameCharacter : MonoBehaviour
    { 
        // Geeneric
        public float gameTime;

        // Combat
        public float timeOfLastAttack;
        public bool attackOnCooldown;
        public int MaxHp;
        public bool IsAlive { get; set; }
        public List<GameObject> primaryWeaponSlots;
        public List<GameObject> secondaryWeaponSlots;
        public GameObject explosionPrefab;
        public GameObject smokePreab;
        public int attackCooldownTime = 3; // default to 3
        protected int hp;
        private int activeWeapon; // 1 is primary, 2 is secondary

        // Audio
        protected AudioSource audioData;

        public int Hp { get => hp; }


        protected void Start()
        {
            audioData = GetComponent<AudioSource>();
            this.hp = MaxHp;
            this.IsAlive = true;
            this.attackOnCooldown = false;
            this.InitialiseWeapons(this.transform);
        }

        protected void Update()
        {
            gameTime += Time.deltaTime;
        }

        protected void Attack()
        {
            if (activeWeapon == 1)
            {
                for (int i = 0; i < primaryWeaponSlots.Count; i++)
                {
                    primaryWeaponSlots[i].GetComponent<Weapon>().Fire();
                }
            }
            else if (activeWeapon == 2)
            {
                for (int i = 0; i < secondaryWeaponSlots.Count; i++)
                {
                    secondaryWeaponSlots[i].GetComponent<Weapon>().Fire();
                }
            }
            timeOfLastAttack = gameTime;
        }

        protected void UpdateCombatState()
        {
            if (gameTime < timeOfLastAttack + attackCooldownTime)
            {
                this.attackOnCooldown = true;
            }
            else
            {
                this.attackOnCooldown = false;
            }
        }

        private void InitialiseWeapons(Transform parent)
        {
            this.activeWeapon = 1;

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.tag.Equals("Weapon"))
                {
                    primaryWeaponSlots.Add(child.gameObject);
                }
                else if (child.tag.Equals("WeaponSecondary"))
                {
                    secondaryWeaponSlots.Add(child.gameObject);
                }
            }
        }

        protected void ActivatePrimaryWeapon()
        {
            this.activeWeapon = 1;
        }

        protected void ActivateSecondaryWeapon()
        {
            this.activeWeapon = 2;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag.ToUpper().Equals("DAMAGINGPROJECTILE"))
            {
                // handle damage
                this.hp -= collider.GetComponent<ProjectileBehaviour>().damage;

                // handle impact particle effects
                Vector3 SpawnHere = collider.transform.position;
                Instantiate(explosionPrefab, SpawnHere, collider.transform.rotation);
                Instantiate(smokePreab, SpawnHere, collider.transform.rotation).transform.parent = transform; // add to parent so it's destroyed along with it

                // check it's still alive...
                if (Hp <= 0)
                {
                    if (gameObject.name != "Player") // only destroy player objects as we need player object after death.
                    {
                        Destroy(gameObject);
                    }
                    this.IsAlive = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
