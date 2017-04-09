using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PublicVariables
{
    [Range(0.0f, 1.0f)]
    public static float req_Amplitude = 0.15f;
}

public class Trigger : MonoBehaviour {
    int initTimes = 0;

    public enum Affectors
    {
        Both, NonPlayer, Player,
    }

    public Affectors affectors = Affectors.Both;

    [System.NonSerialized]
    public bool isTriggered;
    public bool continous = false; //om true så kallar den kommandot hela tiden, bör vara iklickad om man kör med ljud
    public float collisionExtent = 5;

    //[Range(0.0f, 1.0f)]
    //public float PublicVariables.req_Amplitude = 0.0f;

    //switch
    public bool switchTriggered = false; //ändrar state till motsatt istället för satta, som en spak
    public float switchCD = 1.0f;
    public float switchTimer = 0.0f;
    //switch
    public LayerMask collisionMask;

    public ParticleSystem psActivated;

    public UnityEvent fEventStart;
    public UnityEvent fEventExit;

    private float startVolume;
    private AudioSource audioSource;
    public AudioClip audioActive;
    public AudioClip audioDeactive;
    // Use this for initialization
    void Start()
    {
        isTriggered = true; //är viktig så att ToggleTrigger inte fuckar med sina if-satser
        ToggleTrigger(false);

        initTimes++;
        audioSource = transform.GetComponent<AudioSource>();
        if (audioSource != null)
            startVolume = audioSource.volume;

        Reset();
    }

    public void Reset()
    {
        ExitTrigger();
    }

    void FixedUpdate()
    {
        if (initTimes == 0) return;

        bool b;

        if (switchTriggered)
        {
            b = GetTriggeredSwitch();
        }
        else
        {
            b = GetTriggered();
        }

        ToggleTrigger(b);
    }

    public bool GetTriggeredSwitch()
    {
        if (switchTimer > Time.time)
        {
            return isTriggered; //ingen ändring
        }

        switchTimer = Time.time + switchCD;


        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(collisionExtent, collisionExtent, collisionExtent), Quaternion.identity, collisionMask);
        if (col.Length > 0)
        {
            float bestAmplitude = -Mathf.Infinity;
            if (PublicVariables.req_Amplitude > 0) //använd amplitud, annars räcker det att du står i collidern
            {
                for (int i = 0; i < col.Length; i++)
                {
                    FrequencyAnalysis fA = col[i].GetComponent<FrequencyAnalysis>();
                    SoundAnalysisNotPlayer fB = col[i].GetComponent<SoundAnalysisNotPlayer>();
                    if (fA != null && (affectors == Affectors.Both || affectors == Affectors.Player))
                    {
                        bestAmplitude = Mathf.Max(bestAmplitude, fA.m_momentaryAmplitude);
                    }
                    if (fB != null && (affectors == Affectors.Both || affectors == Affectors.NonPlayer))
                    {
                        print(Time.time);
                        bestAmplitude = Mathf.Max(bestAmplitude, fB.m_currentAmplitude);
                    }
                    else if (col[i].transform.parent != null)
                    {
                        fB = col[i].transform.parent.GetComponent<SoundAnalysisNotPlayer>();
                        if (fB != null && (affectors == Affectors.Both || affectors == Affectors.NonPlayer))
                        {
                            bestAmplitude = Mathf.Max(bestAmplitude, fB.m_currentAmplitude);
                            print(Time.time);
                        }
                    }
                }

                if (bestAmplitude < PublicVariables.req_Amplitude) //inte tillräkligt med amplitud
                {
                    return isTriggered;
                }
                return !isTriggered;
            }
        }
        return isTriggered;
    }

    public bool GetTriggered()
    {
        Collider[] col = Physics.OverlapBox(transform.position, new Vector3(collisionExtent, collisionExtent, collisionExtent), Quaternion.identity, collisionMask);
        if (col.Length > 0)
        {
            float bestAmplitude = -Mathf.Infinity;
            if(PublicVariables.req_Amplitude > 0) //använd amplitud, annars räcker det att du står i collidern
            {
                for(int i = 0; i < col.Length; i++)
                {
                    FrequencyAnalysis fA = col[i].GetComponent<FrequencyAnalysis>();
                    SoundAnalysisNotPlayer fB = col[i].GetComponent<SoundAnalysisNotPlayer>();
                    if(fA != null && (affectors == Affectors.Both || affectors == Affectors.Player))
                    {
                        bestAmplitude = Mathf.Max(bestAmplitude, fA.m_momentaryAmplitude);
                    }
                    if(fB != null && (affectors == Affectors.Both || affectors == Affectors.NonPlayer))
                    {
                        bestAmplitude = Mathf.Max(bestAmplitude, fB.m_currentAmplitude);
                    }
                    else if(col[i].transform.parent != null)
                    {
                        fB = col[i].transform.parent.GetComponent<SoundAnalysisNotPlayer>();
                        if (fB != null && (affectors == Affectors.Both || affectors == Affectors.NonPlayer))
                        {
                            bestAmplitude = Mathf.Max(bestAmplitude, fB.m_currentAmplitude);
                        }
                    }
                }

                if(bestAmplitude < PublicVariables.req_Amplitude) //inte tillräkligt med amplitud
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void ToggleTrigger(bool b)
    {
        if (b)
        {
            if (isTriggered != b)
            {
                if (psActivated != null)
                {
                    //psActivated.Simulate(0.0f, true, true);
                    ParticleSystem.EmissionModule psemit = psActivated.emission;
                    //psemit.enabled = true;
                    psActivated.Play();
                }
                StartTrigger();

            }
            else if (continous)
            {
                StartTrigger();
            }
            isTriggered = true;
        }
        else
        {
            if (isTriggered != b)
            {
                ExitTrigger();
            }
            else if(continous) //maybe?
            {
                ExitTrigger();
            }

            if (psActivated != null)
            {
                psActivated.Stop();
            }
            isTriggered = false;

        }
    }

    public virtual void StartTrigger()
    {
        if (audioSource != null)
        {
            if (audioActive != null)
            {
                //audioSource.clip = audioActive;
                //audioSource.Play();
                StopAllCoroutines();
                StartCoroutine(FadeInClip(audioActive));
            }
        }
        fEventStart.Invoke();
    }

    public virtual void ExitTrigger()
    {
        if (audioSource != null)
        {
            if (audioDeactive != null)
            {
                //audioSource.clip = audioDeactive;
                //audioSource.Play();
                StopAllCoroutines();
                StartCoroutine(FadeInClip(audioDeactive));
            }
        }
        fEventExit.Invoke();
    }

    IEnumerator FadeInClip(AudioClip ac)
    {
        while (!AudioFadeOut())
            yield return new WaitForSeconds(0.01f);
        audioSource.clip = ac;
        audioSource.Play();
        while (!AudioFadeIn())
            yield return new WaitForSeconds(0.01f);
    }

    bool AudioFadeIn()
    {
        if (audioSource.volume < startVolume)
        {
            audioSource.volume += 0.4f * Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    bool AudioFadeOut()
    {
        if (audioSource.volume > 0.002)
        {
            audioSource.volume -= 0.4f * Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
