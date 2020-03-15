using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFXSoundPlay : MonoBehaviour {

    private AudioSource m_audio;
    public bool playSound, releaseCageOnTrack;

    [SerializeField]
    float m_TimesHitted = 0f;

    public float TimesHitted
    {
        get { return m_TimesHitted; }
        set
        {
            float v = Mathf.Clamp(value, 0f, 4f);
            if (m_TimesHitted != v)
                m_TimesHitted = v;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        TimesHitted = m_TimesHitted;
    }
#endif
    // Use this for initialization
    void Start () {
        m_audio = GetComponent<AudioSource>();

        if (m_audio == null)
            Debug.LogWarning("Obj sound null check it!");
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            m_audio.Play();

        if (playSound)
        {
            m_audio.Play();
            playSound = false;
        }

        if(m_TimesHitted >= 3)
        {
            releaseCageOnTrack = true;
        }
	}



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "cageparts")
        {
            playSound = true;
            m_TimesHitted += 1f;
        }
    }
}
