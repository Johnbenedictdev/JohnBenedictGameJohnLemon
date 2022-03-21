//this is for importing directives that allow you to use code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   //this is a trial
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;//add comments later
    Vector3 m_Movement;//vector 3 is a special data type, representing movement // m_Movement is the name we want the data container to be
    Quaternion m_Rotation = Quaternion.identity;

    void Start ()//this is the mechine in the factory with its own set of purpose (a specific purpose)
    {
        m_Animator = GetComponent<Animator> ();// i dont fully know how to explain why this line of code is the way it is but essentially what it does is it makes a vraibale with no declared data type, setting the value as the Animator. but maybe this type of code is not always set to end up with a value siicne the method is asked to end up with a component rather than a value. and this componenet in the animator component. okay i just found out that variables can sometimes not store data but reference components 
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();//add comments later 
    }

    void FixedUpdate ()//this method is called every frame. everything in the method is called everytime the frame changes
    {
        float horizontal = Input.GetAxis ("Horizontal");//this axis represents the left&right arrow or A&D input from keyboard
        float vertical = Input.GetAxis ("Vertical");//this axis represents the up&down arrow or W&D input from keyboard
        //the float tells us that the following variable (horizontal or vertical) are of the data type, float
        // the horizontal/vertical are the names of the variables (we chose this)
        //the input.getaxis is a code that is predefined by unity to group characters from keyboard in a way that makes sense. for example it makes sense to have WASD and up&down&left&right arrows to represent movement. it made sense to them that W&S and up&down arrows are vertical axes while A&D and left&right arrow are horizontal. in a way they have been "modulized" in modules/groupings that make sense. in this game tho there is no up and down movement so vertical is just forward/backward as opposed to the typical up and down
        //if W/S/up/down keys are clicked, the value or parameter of input.getaxis is horizontal, therefore it is stored under horizontal variable (storage)
        m_Movement.Set(horizontal, 0f, vertical);//here we are *set*-ting the value of the setting to be: for x, whatever we got from the "horizontal" variable we have set (left or right): for y, 0 since we dont want it to move up and down: for z, whatever we got from the "vertical" variable we have created earlier (forwards or backwards), this is not to confuse with the typical "vertical=up and down". if we want up and down like jumping or scrouthing then perhaps "vertical"  can be put inside the middle value and the last value to the right can be 0f
        m_Movement.Normalize ();//in pythagoras we learn that the magnitud of a 1:1 triangle is squareroot of 2 which is almost 1.5. this means that for every directional change there is 1.5x movement change. normalizing it removes the mathematical caveat to the program. this means that for every change in direction there is a proportionate change in movement 

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);//this says: create a variable named "hasHorizontalInput" to store bool type data. then if the variable we previously created has no input, or 0f, then store it as a yes in the hasvertical imput. but the "!" suggests that it is opposite day, so really what is happening here is that if there is no-no input in horizontal. non-no cancels out to yes then really it says if theres input in vertical then store in hasvertical inout "yes". i think the reason behind this is because theres no direct code that ask "does this have something?" because then it has to know exactly what is that something. to go around this is to make a statement that asks "is this empty" then add "!" then combine to make a statement that sound sliek this "is this not empty? (does it have something?)"
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);//same explanation as above
        //mathf is a predefined class that returns a bool: so here when the horizontal value is 0f, or no movement at all, it returns a yes value. then this statement really says if the value on the right of the said variable on the left is the actual velue being generated then return a yes. if the mathematical stament within the bracket is true then it returns a ture. if the stement is not satisfied then it returns a false
        
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
         if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }
        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}