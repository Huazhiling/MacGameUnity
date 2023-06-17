using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace platform2D
{
    public class PlayerControll : MonoBehaviour
    {
        private float moveSpeed = 3f;
        private Rigidbody2D rb2D;
        private BoxCollider2D bc2D;
        private Camera mGameCamera;
        //定义玩家移动的范围
        private float minX ;
        private float maxX;
        private float minY;
        private float maxY;
        //拿到摄像机实际的显示区域宽高，用来限制玩家的移动
        private float cameraHeight;
        private float cameraWidth;
        private float jumpDefaultHeight = 8f;
        private Animator playAnimator;
        public LayerMask terrainMask;
        //当前角色的跳跃状态
        private TriggerJump playJumpChildObj;
        public bool isFighting;
        public TilemapCollider2D tilemapCollider2;

        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            mGameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            playAnimator = GetComponent<Animator>();
            playJumpChildObj = transform.Find("TriggerObj").GetComponent<TriggerJump>();
            bc2D = GameObject.Find("ArmsTriggerObj").GetComponent<BoxCollider2D>();
            cameraHeight = 2f * mGameCamera.orthographicSize;
            cameraWidth = cameraHeight * mGameCamera.aspect;
            minX = mGameCamera.transform.position.x - cameraWidth / 2f;
            maxX = mGameCamera.transform.position.x + cameraWidth / 2f;
            minY = mGameCamera.transform.position.y - cameraHeight / 2f;
            maxY = mGameCamera.transform.position.y + cameraHeight / 2f;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !playJumpChildObj.isJump)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpDefaultHeight);
                playAnimator.SetBool("isJump", true);
                playAnimator.SetBool("isFalling", false);
            }else if (Input.GetKeyDown(KeyCode.J))
            {
                if (!isFighting)
                {
                    playAnimator.SetTrigger("isFighting");
                }
            }else if (Input.GetKeyDown(KeyCode.K)){
                Vector2 position;
                if (transform.localScale.x == -1)
                {
                    position = new Vector2(Mathf.Clamp(rb2D.position.x - 3, minX, maxX), rb2D.position.y);
                }else
                {
                   position = new Vector2(Mathf.Clamp(rb2D.position.x + 3, minX, maxX), rb2D.position.y);
                }
                position = checkTargetPositionIsTrigger(position);
                rb2D.position = position;
            }

            checkCurrentJumpStatus();
        }

        /**
         * 判断到达点是否在内部
         */
        private Vector2 checkTargetPositionIsTrigger(Vector2 position)
        {
            Debug.Log("准备到达的点：" + position);
            bool isOverlap = tilemapCollider2.OverlapPoint(position);
            if (isOverlap)
            {
              RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.up);
                while(raycastHit2D.collider != null && raycastHit2D == tilemapCollider2)
                {
                    position += Vector2.up;
                    raycastHit2D = Physics2D.Raycast(position, Vector2.up);
                }
            }
            return position;
        }

        private void checkCurrentJumpStatus()
        {
            if (playAnimator.GetBool("isJump")){
                if (rb2D.velocity.y < 0)
                {
                    playAnimator.SetBool("isJump", false);
                    playAnimator.SetBool("isFalling", true);
                }
            }else if (playAnimator.GetBool("isFalling"))
            {
                if (!playJumpChildObj.isJump)
                {
                    playAnimator.SetBool("isFalling", false);
                }
            }
        }

        private void FixedUpdate()
        {
            playMove();
        }

        private void playMove()
        {
            float horizontalX = Input.GetAxis(ConstantProject.InputType.HORIZONTAL);
            float faceDirection = Input.GetAxisRaw(ConstantProject.InputType.HORIZONTAL);
            float verticalY = Input.GetAxis(ConstantProject.InputType.VERTICAL);
           
            if (horizontalX != 0)
            {
                Vector2 position = new Vector2(Mathf.Clamp(rb2D.position.x + (horizontalX * moveSpeed * Time.deltaTime), minX, maxX), rb2D.position.y);
                // 在玩家移动的逻辑中，将玩家的位置限制在摄像机视野范围内
                rb2D.position = position;
                playAnimator.SetBool("isRunning", true);
                playAnimator.SetBool("isIdle", false);
            }
            else
            {
                playAnimator.SetBool("isIdle", true);
                playAnimator.SetBool("isRunning", false);
            }
            if (faceDirection != 0)
            {
                transform.localScale = new Vector2(faceDirection, 1);
            }

            isFighting = playAnimator.GetCurrentAnimatorStateInfo(0).IsName("fighting");
            bc2D.enabled = isFighting;
        }
    }

}