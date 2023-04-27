using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LvlManager;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    #region zmienne
    [SerializeField] float speed,Strength;
    [SerializeField] Rigidbody rgbody;
    [SerializeField] GameObject groundcheck,ParticleEffect,SlamParticles,trail,PointCounter;
    [SerializeField] LayerMask ground;
    [SerializeField] Text speedUI;
    [SerializeField] TextMeshProUGUI ComboText,WinText,GamePointsText,nearmissText;
    [SerializeField] CameraController cam;
    [SerializeField] LevelManager LVLmanager;
    [SerializeField] Transform SpawnPos;
    [SerializeField] GameObject[] ParticleEffects;
    [SerializeField] AudioSource sound,AddSound;
    [SerializeField] AudioClip swoosh, swoop, slam;
    public delegate void PlayerEvents();
    public static event PlayerEvents OnDeath;
    int CurrentParticleEffect;
    int ComboPoints;
    public int GamePoints;
    Transform ComboIdentity;
    IEnumerator jumpCor,WaitForSlam;
    IEnumerator CurrComCor;
    float DefaultSpeed;
    bool SpeedCheck, checkground, debounceCombo,debounceNearMiss;
    public bool Stop,IsDead;
    Vector3 doubleScale;
    Vector3 onehalfscale;
    public bool InWinZone;
    #endregion
    void Start()
    {
        ComboIdentity = ComboText.transform;
        transform.position = SpawnPos.position;
        DefaultSpeed = speed;
        StartPlayer();
        doubleScale = new Vector3(2, 2, 2);
        onehalfscale = new Vector3(2, 2, 2);
    }
    bool CheckGround()
    {
        if (Physics.CheckSphere(groundcheck.transform.position, 1f, ground)) return true;
        
        else return false;
    }
    private void OnEnable()
    {
        NearMissScript.OnMiss += NearMiss;
        NearMissScript.OnWallHit += AddCombo;
        
    }
    private void OnDisable()
    {
        NearMissScript.OnMiss -= NearMiss;
        NearMissScript.OnWallHit -= AddCombo;
    }

    IEnumerator VelFade(float dir,float multiplier) //smooths out and balances jumping and slamming movement.
    {
        bool done = false;
        float vel = 1f;
        while (!done)
        {
            rgbody.velocity = new Vector3(rgbody.velocity.x,(dir * Strength * multiplier) * Mathf.Abs(vel));
            vel -= Time.deltaTime;
            if (vel <= 0) done = true;
            yield return 0;
        }
        yield return null;
    }
    public void PlaySound(AudioClip clip)
    {
        sound.PlayOneShot(clip);
    }
    public void Controls()
    {

        if ((Input.touchCount >0&& Input.GetTouch(0).phase== TouchPhase.Began)||Input.GetKeyDown(KeyCode.Mouse0))
        {
        if(jumpCor!=null) StopCoroutine(jumpCor);
            if (CheckGround())
            {
                jumpCor = VelFade(1, 1);
                sound.PlayOneShot(swoop);
               
                if (WaitForSlam != null) StopCoroutine(WaitForSlam);
            }
            else
            {
                jumpCor = VelFade(-1, 2);
                sound.PlayOneShot(swoosh);
                
                WaitForSlam = WaitForGround();
                StartCoroutine(WaitForSlam);

            }
        StartCoroutine(jumpCor);

        }

    }
    IEnumerator WaitForGround()
    {
        yield return new WaitForSeconds(0.1f);//delay for slamforce to register correctly;
        float SlamForce = SlamStrength();
        ParticleEffects[4].SetActive(true);
        yield return new WaitUntil(() => checkground);
        ParticleEffects[4].SetActive(false);
        if (SlamForce > 10f)
        {
            Instantiate(ParticleEffect, transform.position, transform.rotation);
            sound.PlayOneShot(slam);
        }

    }
    public float SlamStrength() //returns the strength of a ground slam, 
    {
        return -rgbody.velocity.y;
    }
    void ManageParticles(int index) //manages particles on player 
    {
        for(int o = 0; o < ParticleEffects.Length; o++)
        {
            if (o == index - 1) ParticleEffects[o].SetActive(true);
            else ParticleEffects[o].SetActive(false);
        }
    }
    public void Combo(int _ComboPoints)
    {
        ComboPoints += _ComboPoints;
       
        switch (ComboPoints)
        {
            case 1:
                if(CurrComCor!=null)StopCoroutine(CurrComCor);
                ComboText.gameObject.SetActive(true);
                ComboText.gameObject.transform.localScale = Vector3.zero;
                ComboText.transform.position = ComboIdentity.position;
                ComboText.transform.rotation = ComboIdentity.rotation;
                ComboText.text = "Good!";
                ComboText.color = Color.green;
                ManageParticles(1);
                LeanTween.cancelAll();
                LeanTween.scale(ComboText.gameObject, doubleScale, 0.25f).setEase(LeanTweenType.easeOutBounce);
                LeanTween.rotateZ(ComboText.gameObject, ComboIdentity.rotation.z + 15, 0.35f).setLoopPingPong();
                CurrComCor = Comboreset();
                StartCoroutine(CurrComCor);
                break;
            case 2:
                StopCoroutine(CurrComCor);
                ComboText.gameObject.SetActive(true);
                ComboText.gameObject.transform.localScale = Vector3.zero;
                ComboText.transform.position = ComboIdentity.position;
                ComboText.transform.rotation = ComboIdentity.rotation;
                ComboText.text = "Great!";
                ComboText.color = Color.blue;
                ManageParticles(2);
                LeanTween.cancelAll();
                LeanTween.scale(ComboText.gameObject, doubleScale, 0.25f).setEase(LeanTweenType.easeOutBounce);
                LeanTween.rotateZ(ComboText.gameObject, ComboIdentity.rotation.z + 15, 0.35f).setLoopPingPong();
                CurrComCor = Comboreset();
                StartCoroutine(CurrComCor);
                break;
            case 3:
                StopCoroutine(CurrComCor);
                ComboText.gameObject.SetActive(true);
                ComboText.gameObject.transform.localScale = Vector3.zero;
                ComboText.transform.position = ComboIdentity.position;
                ComboText.transform.rotation = ComboIdentity.rotation;
                ComboText.text = "Incredible!";
                ComboText.color = Color.cyan;
                ManageParticles(3);
                LeanTween.cancelAll();
                LeanTween.scale(ComboText.gameObject, doubleScale, 0.25f).setEase(LeanTweenType.easeOutBounce);
                LeanTween.rotateZ(ComboText.gameObject, ComboIdentity.rotation.z + 15, 0.35f).setLoopPingPong();
                CurrComCor = Comboreset();
                StartCoroutine(CurrComCor);
                break;
            case 4:
                StopCoroutine(CurrComCor);
                ComboText.gameObject.SetActive(true);
                ComboText.gameObject.transform.localScale = Vector3.zero;
                ComboText.transform.position = ComboIdentity.position;
                ComboText.transform.rotation = ComboIdentity.rotation;
                ComboText.text = "Perfect!";
                ComboText.color = Color.red;
                ManageParticles(4);
                LeanTween.cancelAll();
                LeanTween.scale(ComboText.gameObject, doubleScale, 0.25f).setEase(LeanTweenType.easeOutBounce);
                LeanTween.rotateZ(ComboText.gameObject, ComboIdentity.rotation.z + 15, 0.35f).setLoopPingPong();
                CurrComCor = Comboreset();
                StartCoroutine(CurrComCor);
                break;
        }
        
    } //displays combo text on screen
    async void NearMiss()
    {
        nearmissText.transform.localScale = Vector3.zero;
        nearmissText.gameObject.SetActive(true);
        LeanTween.scale(nearmissText.gameObject, onehalfscale, 0.25f).setEase(LeanTweenType.easeOutBounce);
        await Task.Delay(1000);
        LeanTween.scale(nearmissText.gameObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeOutBounce);
        await Task.Delay(250);
        nearmissText.gameObject.SetActive(false);

        if (!debounceNearMiss)
        {
            ModifyPoints(1);
            debounceNearMiss = true;
            await Task.Delay(250);
            debounceNearMiss = false;
        }

    }
    public async void AddCombo()  //special function when hitting walls, add point and plays a sound
    {
        if (debounceCombo) return;
        debounceCombo = true;
            Combo(1);
        AddSound.Play();
        await Task.Delay(1200);
        debounceCombo = false;
        
    }
    IEnumerator Comboreset()//resets combo points
    {
        yield return new WaitForSeconds(4f);
        LeanTween.scale(ComboText.gameObject, Vector3.zero, 1f).setEaseInBack();
        yield return new WaitForSeconds(1f);
        ComboPoints = 0;
        
        ManageParticles(99);

    }
    public void WinCelebration()
    {
        rgbody.velocity = Vector3.zero;
        trail.SetActive(false);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        LeanTween.moveY(gameObject, 3.2f, 0.25f).setLoopPingPong();
        cam.ChangeCameraMode(2);
        cam.ChangeCameraMode(0);

    }
    public void PlayerDeath() 
    {
        OnDeath();
        IsDead = true;
        Instantiate(ParticleEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
        nearmissText.gameObject.SetActive(false);
        ComboText.gameObject.SetActive(false);
        ComboPoints = 0;
        trail.SetActive(false);
        if (!InWinZone) Invoke("Respawn", 2f);
        else Debug.Log("execute Win screen");

    }
    public void Respawn()
    {
        IsDead = false;
        LeanTween.cancelAll(gameObject);
        cam.ChangeCameraMode(1);
        transform.position = SpawnPos.position;
        LVLmanager.RestartCurrent();
        Time.timeScale = 1;
        GamePoints = 0;
       
        InWinZone = false;
        GamePointsText.text = GamePoints.ToString();
        cam.ChangeCameraMode(1);
        gameObject.SetActive(true);
        SpeedCheck = false;
        speed = DefaultSpeed;
        StartPlayer();
        trail.SetActive(true);
        //rgbody.velocity = Vector3.right * speed;

    }
    public void ModifySpeed(float _Speed)
    {
        speed += _Speed;
    }
    public void ModifyPoints(int _Points)
    {
        GamePoints += _Points;
        GamePointsText.text = GamePoints.ToString();
        GameObject curr=Instantiate(PointCounter, ComboText.transform.parent);
        curr.GetComponent<TextMeshProUGUI>().text = "+" + _Points;
    }
    void StartPlayer()
    {
        Stop = false;
        StartCoroutine(ControlLoop());
        StartCoroutine(SpeedAdd());
    }
    public void StopPlayer()
    {
        Stop = true;
        
    }
    IEnumerator ControlLoop()
    {
        while (!Stop)
        {
            checkground = CheckGround();
            Controls();
            if (checkground && jumpCor != null) StopCoroutine(jumpCor);
            yield return 0;
        }
    }

    IEnumerator SpeedAdd()
    {
        while (!Stop)
        {
            speedUI.text = rgbody.velocity.x.ToString();

            if (rgbody.velocity.x > 5) SpeedCheck = true;
            if (rgbody.velocity.x < 1.5f && SpeedCheck) PlayerDeath();
            if (speed <= 0) PlayerDeath();
            rgbody.velocity = (Vector3.right * speed) + new Vector3(0, rgbody.velocity.y, rgbody.velocity.z);
            yield return new WaitForFixedUpdate();
        }
    }
    
}
