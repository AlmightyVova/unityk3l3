using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    //public Transform cam;

    public float turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    private Vector3 _targetPosition;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 mousepos;
    private NavMeshAgent agent;

    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("walking");

    public TextMeshProUGUI interactableText;

    private void Start()
    {
        controller.detectCollisions = false;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 30;
        agent.angularSpeed = 120;
        agent.acceleration = 50;

        _animator = GetComponent<Animator>();

        interactableText.text = "";
        interactableText.faceColor = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        if (agent.remainingDistance == 0)
        {
            _animator.SetBool(Walking, false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            mousepos = Input.mousePosition;
            ray = Camera.main.ScreenPointToRay(mousepos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);
                /*_targetPosition = -new Vector3(transform.position.x - hit.point.x, transform.forward.y,
                        transform.position.z - hit.point.z)
                    .normalized;*/
            }

            _animator.SetBool(Walking, true);

            if (hit.collider.CompareTag("item"))
            {
                interactableText.text = $"This is an {hit.collider.tag}\n" +
                                        $"Pick up the item";
            }
            else if (hit.collider.CompareTag("enemy"))
            {
                interactableText.text = $"This is an {hit.collider.tag}\n" +
                                        $"Fight the enemy";
            }
            else if (hit.collider.CompareTag("npc"))
            {
                interactableText.text = $"This is an {hit.collider.tag}\n" +
                                        $"Open chat";
            }
            else
            {
                interactableText.text = "";
                interactableText.faceColor = new Color32(255, 255, 255, 255);
            }
        }

        /*var rotQ = Quaternion.LookRotation(transform.forward + _targetPosition);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotQ, Time.deltaTime * 15);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg /* + cam.eulerAngles.y#1#;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * (speed * Time.deltaTime));
        }*/
    }
}