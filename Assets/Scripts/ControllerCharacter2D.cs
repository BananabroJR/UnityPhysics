using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControllerCharacter2D : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] float turnRate;
	[SerializeField] float jumpHeight;
	[SerializeField] float doubleJumpHeight;
	[SerializeField] float hitForce;
    [SerializeField, Range(1, 5)] float fallRateMultiplier;
    [SerializeField, Range(1, 5)] float lowJumpRateMultiplier;
    [Header("Ground")]
	[SerializeField] Transform groundTransform;
	[SerializeField] LayerMask groundLayerMask;

	Rigidbody2D rb;
	Vector2 velocity = Vector2.zero;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

    

    void Update()
	{
        //check if character is on the ground :)
        bool onGround = Physics2D.OverlapCircle(groundTransform.position, 0.2f, groundLayerMask);

        // get direction input
        Vector2 direction = Vector2.zero;
		direction.x = Input.GetAxis("Horizontal");
		

		velocity.x = direction.x * speed;
		
		// set velocity
		if (onGround)
		{
			if(velocity.y < 0) velocity.y = 0;
			
			if(Input.GetButtonDown("Jump"))
			{
				velocity.y += Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
				
                StartCoroutine(DoubleJump());
            }
		}
		velocity.y += Physics.gravity.y * Time.deltaTime;
        //ajust fall rate
        float gravityMultiplier = 1;
        if (!onGround && velocity.y < 0) gravityMultiplier = fallRateMultiplier;
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
		if (!onGround && velocity.y > 0 && !Input.GetButton("Jump")) gravityMultiplier = lowJumpRateMultiplier;

        // move character
        rb.velocity= velocity;

		
	}

  
        IEnumerator DoubleJump()
        {
            // wait a little after the jump to allow a double jump
            yield return new WaitForSeconds(0.01f);
            // allow a double jump while moving up
            while (velocity.y > 0)
            {
                // if "jump" pressed add jump velocity
                if (Input.GetButtonDown("Jump"))
                {
                    velocity.y += Mathf.Sqrt(doubleJumpHeight * -2 * Physics.gravity.y);
                    break;
                }
                yield return null;
            }
        }
    

    

    
}
