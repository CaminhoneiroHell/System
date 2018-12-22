
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour {

    public float speed = 20;
    public Movement movement;
    public IUnityService unityService;
    public ISoundManager soundManager;

    [Inject(Id = "PlayerWeapon")]
    private IWeapon _weapon;

    // Use this for initialization
    void Start () {

        //_weapon.Charge();

        movement = new Movement(speed);
        if (unityService == null)
            unityService = new UnityService();

        //if (soundManager == null)
        //    soundManager = new SoundManager();
	}

    [Inject]
    public void Construct(ISoundManager _soundManager)
    {
        soundManager = _soundManager;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position += movement.Calculate(
            unityService.GetAxis("Horizontal"),
            unityService.GetDeltaTime());
        
        if (unityService.GetAxis("Horizontal") != 0 &&
            !soundManager.IsPlayingSound(GetComponent<AudioSource>()))
        {
            soundManager.Play(GetComponent<AudioSource>());
        }

	}

}
