using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platform2D { 
    public class TriggerJump : MonoBehaviour
    {
        public bool isJump;
    
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Equals("mapBody"))
            {
                isJump = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name.Equals("mapBody"))
            {
                isJump = true;
            }
        }
    }
}
