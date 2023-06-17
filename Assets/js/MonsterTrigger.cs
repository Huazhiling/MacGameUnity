using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platform2D
{
    public class MonsterTrigger : MonoBehaviour
    {
        public PlayerControll playerGameObj;
        public Animator monsterAnimator;
        private int health = 100;
        private bool isDestory;
        public AudioSource audio;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (health <= 0)
            {
                monsterAnimator.SetFloat("health", health);
            }
            bool isKill = monsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("kill");
            if (isKill && !isDestory)
            {
                isDestory = true;
                Invoke("destoryGameObj", 0.7f);
               
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (playerGameObj.isFighting && collision.name.Equals("ArmsTriggerObj") && health > 0)
            {
                audio.Play();
                Invoke("reducHP", 0.5f);
            }
        }

        private void reducHP()
        {
            health -= 40;
        }
        private void destoryGameObj()
        {
            Destroy(gameObject);
        }

    }

}
