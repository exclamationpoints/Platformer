using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Transform objTransform;
    public Animator animator; /*code for animation*/
    public SpriteRenderer sprite; /*code for animation*/
    public Transform farthest; /*code for bg*/
    public Transform middle; /*code for bg*/
    public Transform near; /*code for bg*/
    public AudioSource jumpSound;

    public float horizV;
    private float maxHorizV;
    public float horizA;
    private float decelMultiplier;
    private float x;

    private bool isGrounded;
    public float vertV;
    private float jumpHeight;
    private float gravity;
    private float y;

    private int score;
    public Text displayText;

    void Start()
    {
        objTransform = this.gameObject.transform;

        horizV = 0;
        maxHorizV = 6;
        horizA = 0;
        decelMultiplier = 16;
        x = 0;

        isGrounded = true;
        vertV = 0;
        jumpHeight = 3;
        gravity = -6;
        y = 1;

        score = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            animator.SetInteger("HorizontalSpeed", 2); /*code for animation*/
            sprite.flipX = true;
            if (horizV > 0){
                horizA = 0;
                horizV = 0;
            }

            if (horizV < -maxHorizV){
                horizA = 0;
                horizV = -maxHorizV;
            } else {
                horizA = horizA - 8 * Time.deltaTime;
            }

        } else if (Input.GetKey(KeyCode.RightArrow)) {
            animator.SetInteger("HorizontalSpeed", 2); /*code for animation*/
            sprite.flipX = false;
            if (horizV < 0){
                horizA = 0;
                horizV = 0;
            } 

            if (horizV > maxHorizV){
                horizA = 0;
                horizV = maxHorizV;
            } else {
                horizA = horizA + 8 * Time.deltaTime;
            }

        } else {
            animator.SetInteger("HorizontalSpeed", 0); /*code for animation*/
            if(isGrounded){
                decelMultiplier = 16;
            } else {
                decelMultiplier = 0.5f;
            }

            horizA = horizA - (horizV * decelMultiplier) * Time.deltaTime;

            if (Mathf.Round(horizV) == 0){
                horizA = 0;
                horizV = 0;
            }
        }

        horizV = horizV + horizA * Time.deltaTime;
        x = x + horizV * Time.deltaTime;

        animator.SetBool("isGrounded", isGrounded); /*code for animation*/
        if (isGrounded){
            if (Input.GetKeyDown(KeyCode.Space)){
                // formula from unity documentation
                vertV += Mathf.Sqrt(jumpHeight * -3 * gravity);
                jumpSound.Play();

                isGrounded = false;
            }

            if(Mathf.Round(vertV) < 0)
                isGrounded = false;
        }

        vertV = vertV + gravity * Time.deltaTime;
        y = y + vertV * Time.deltaTime;

        objTransform.position = new Vector3(x, y, 0);

        displayText.text = score.ToString();

        if (horizV != 0){
            MakeParallaxEffect(horizV);
        }
    }

    /*for bg*/
    public void MakeParallaxEffect(float x){
        farthest.position = new Vector3(farthest.position.x,farthest.position.y,farthest.position.z);
        middle.position = new Vector3(middle.position.x+x*0.0008f,middle.position.y,middle.position.z);
        near.position = new Vector3(near.position.x+x*0.003f,near.position.y,near.position.z);
    }

    public void IsCollidingWithObject(Collidable other, float otherX, float otherY, float otherWidth, float otherHeight)
    {
        if(other.gameObject.tag.Equals("Wall")){
            float width = this.gameObject.transform.localScale.x;
            if (horizV > 0 && x - width / 2 < otherX - otherWidth / 2){
                horizA = 0;
                horizV = 0;
                x = otherX - otherWidth / 2 - width / 2;
            } else if (horizV < 0 && x + width / 2 > otherX + otherWidth / 2){
                horizA = 0;
                horizV = 0;
                x = otherX + otherWidth / 2 + width / 2;            
            }
        }

        if(other.gameObject.tag.Equals("Platform")){
            float height = this.gameObject.transform.localScale.y;
            if (vertV > 0){
                vertV = 0;
                y = otherY - otherHeight / 2 - height / 2 - 0.1f;
            } else if (vertV < 0){
                vertV = -1;
                y = otherY + otherHeight / 2 + height / 2;
                isGrounded = true;
            }
        }    
    }
    
    public void TeleportTo(float destinationX, float destinationY)
    {
        horizA = 0;
        horizV = 0;
        vertV = 0;

        x = destinationX;
        y = destinationY;
    }

    public void AddScore()
    {
        score++;
    }
}
