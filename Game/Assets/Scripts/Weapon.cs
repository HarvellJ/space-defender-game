using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Weapon : MonoBehaviour
    {
        public GameObject bulletPrefab;

        public Transform bulletSpawn;

        public float bulletSpeed = 30;

        public float lifeTime = 3;

        protected AudioSource audioData;

        // Start is called before the first frame update
        void Start()
        {
            bulletSpawn = this.gameObject.transform.GetChild(0);
            audioData = GetComponent<AudioSource>();
        }

        public abstract void Fire();

        public abstract IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay);
    }
}
