﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocketScript : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float index = 250F;
    [SerializeField] float indexMain = 50F;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] AudioClip Success;
    [SerializeField] ParticleSystem Win = default;
    [SerializeField] ParticleSystem Run = default;
    [SerializeField] ParticleSystem Lose = default;
    [SerializeField] float LevelLoadDelay = 3f;
    // Start is called before the first frame update

    enum State { Alive, Dead, Transcending };
    State state = State.Alive;
    bool CollisionEnabled = true;

    void Start()
    {
        print("hf");
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Dead)
        {
            Thrust();
            Rotate();
        }

        if (Debug.isDebugBuild)
        {
            SpecialDebuggingDetection();
        }
    }

    private void SpecialDebuggingDetection()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Load();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            CollisionEnabled = !CollisionEnabled;
            print(CollisionEnabled);
        }
        
    }

    private void Thrust()
    {
        float frameThrust = indexMain * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * frameThrust);
            Run.Play(); 
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(mainEngine);
        }
        else
        {
            audioSource.Stop();
            Run.Stop();
        }
    }

    private void Rotate()
    {
        float frameRotation = index * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * frameRotation);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * frameRotation);
        }
        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || !CollisionEnabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    //Debug.Log(collision.gameObject.tag);
                    //audioSource.PlayOneShot(Success);
                    break;
                }
            case "Finish":
                {
                    GoToNextLevel();
                    break;
                }
            default:
                {
                    GoDie();
                    break;
                }
        }
    }

    private void GoDie()
    {
        if (state == State.Alive)
        {
            audioSource.Stop();
            Run.Stop();
            audioSource.PlayOneShot(DeathSound);
            Lose.Play();
            state = State.Dead;
        }
        Invoke("Death", LevelLoadDelay);
    }

    private void GoToNextLevel()
    {
        if (state == State.Alive)
        {
            //audioSource.Stop();
            audioSource.PlayOneShot(Success);
            Win.Play();
            //audioSource.Play();
            state = State.Transcending;
        }
        Invoke("Load", LevelLoadDelay);
    }

    private void Death()
    {
        state = State.Alive;
        SceneManager.LoadScene(0);
    }

    private void Load()
    {
        state = State.Alive;
        SceneManager.LoadScene(1);
    }
}
