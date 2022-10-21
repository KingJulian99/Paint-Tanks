using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void TankDestroyedNotify(GameObject go);

public class TankController : MonoBehaviour
{

    public float driveSpeed;
    public float rotateSpeed;
    public int health;
    public Color teamColor;
    public int bounceNumber;
    public bool alive;
    public Material paintExplosionMaterial;

    private CharacterController characterController;
    private float randomRotationSpeed; 
    private float gravity;
    private bool rotatingUncontrollably;
    private ParticleSystem alertEffect;
    private ParticleSystem alertEffectRemove;
    private ParticleSystem explosion;

    // UI fields
    //public GameObject healthBar;
    //public int hBarNumber;

    public event TankDestroyedNotify TankDestroyed;
    
    void Start()
    {
        this.driveSpeed = 5.0f;
        this.rotateSpeed = 55.0f;
        this.health = 100;
        this.characterController = this.GetComponent<CharacterController>();
        this.bounceNumber = 1;
        this.randomRotationSpeed = Random.Range(10.0f, 20.0f);
        this.gravity = -9.0f;
        this.rotatingUncontrollably = false;
        this.alertEffect = this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        this.alertEffectRemove = this.gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        this.explosion = this.gameObject.transform.GetChild(5).gameObject.GetComponent<ParticleSystem>();
        this.alive = true;

        Material newMaterial = Instantiate(paintExplosionMaterial);
        newMaterial.SetColor("_BaseColor", this.teamColor);

        var main = this.explosion.main;
        main.simulationSpeed = 0.6f;

        // Setting the explosion color
        this.gameObject.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = newMaterial;
        this.gameObject.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = newMaterial;
        this.gameObject.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystemRenderer>().trailMaterial = newMaterial;
        this.gameObject.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = newMaterial;
        this.gameObject.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystemRenderer>().trailMaterial = newMaterial;

        ChangeTankColor();

        //healthBar.transform.Find("Color").GetComponent<Image>().color = teamColor;
    }

    void Update()
    {
        UpdateHealthVisual();

        GroundPaintCheck();

        //UpdateHealthBar();

        if ( !this.characterController.isGrounded ) {
            this.characterController.Move( new Vector3(0.0f, this.gravity * Time.deltaTime, 0.0f) ); // applies "gravity"

            if(this.rotatingUncontrollably) {
                this.transform.Rotate(this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime);
            }

        } else {
            // Tank movements
            if(Input.GetKey("w")) {
                this.characterController.SimpleMove(this.transform.forward * driveSpeed);
            }

            if(Input.GetKey("s")) {
                this.characterController.SimpleMove(-this.transform.forward * driveSpeed);
            }

            if(Input.GetKey("d")) {
                this.transform.Rotate(0.0f, this.rotateSpeed * Time.deltaTime, 0.0f);
            }

            if(Input.GetKey("a")) {
                this.transform.Rotate(0.0f, -this.rotateSpeed * Time.deltaTime, 0.0f);
            }
        }
        
        if(this.health <= 0 && this.alive) {
            this.alive = false;
            
            OnTankDestroyed();
            Explode();
        }

        if(this.health <= 0) {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
            {
                Paintable p = hit.collider.GetComponent<Paintable>();
                if(p != null){
                    PaintManager.instance.paint(p, hit.point, 4, 0.3f, 0.1f, this.teamColor);
                }
            }
        }

    }

    public void GroundPaintCheck() {
        /*
            This function casts a ray below the center of the tank, getting the color directly below.
            If the color is deemed to be that of another team, the tank is slowed.
        */

        RaycastHit hit;

        if (!Physics.Raycast(transform.position, -Vector3.up, out hit)) {return;}

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.GetTexture("_MaskTexture") == null || meshCollider == null)
            return;

        RenderTexture rendTex = rend.material.GetTexture("_MaskTexture") as RenderTexture;
        RenderTexture.active = rendTex;

        Texture2D tex = new Texture2D(rendTex.width, rendTex.height, TextureFormat.RGB24, false); 

        tex.ReadPixels(new Rect(0, 0, rendTex.width, rendTex.height), 0, 0);
        tex.Apply();
        
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        // Do color difference.
        string colorBelow = GetTeamColor(tex.GetPixel((int)pixelUV.x, (int)pixelUV.y));
        if(colorBelow != "none" && colorBelow != GetTeamColor(this.teamColor)) {
            this.driveSpeed = 3.0f;
            if(!this.alertEffect.isPlaying) {
                this.alertEffect.Play();
            }
            if(this.alertEffectRemove.isPlaying) {
                this.alertEffectRemove.Pause();
                this.alertEffectRemove.Clear();
            }
        } else {
            this.driveSpeed = 5.0f; 
            if(this.alertEffect.isPlaying) {
                this.alertEffect.Pause();
                this.alertEffect.Clear();
            }
            if(!this.alertEffectRemove.isPlaying) {
                this.alertEffectRemove.Play();
            }
        }

        // Clean up
        RenderTexture.active = null;
        DestroyImmediate(tex);

    }

    public string GetTeamColor(Color color) {
        
        if(color.r == 0.0f && color.g == 0.0f && color.b == 0.0f) {
            return "none";

        } else if(color.r >= color.g) {
            if(color.r >= color.b) {
                return "red";
            } else {
                return "blue";
            }
        } else {
            if(color.g >= color.b) {
                return "green";
            } else {
                return "blue";
            }
        }
    }

    public void SetTeamColor(Color color)
    {
        this.teamColor = color;
    }

    public void Explode() {

        // Play particlesystem
        this.explosion.Play();

        // Destroy it all (can change in future) 
        Destroy(this.gameObject, 0.45f);
        
    }

    public void RotateUncontrollably() {
        this.rotatingUncontrollably = true;
        Invoke("SetHealthToZero", 2f);
    }

    public void SetHealthToZero() {
        this.health = 0;
    }

    public void SetGravity(float gravity) {
        this.gravity = gravity;
    }

    void ChangeTankColor()
    {
        ChangeTankColor(this.teamColor);
    }
    void ChangeTankColor(Color col) {
        /*
            This method sets both the tank and turret prefab colors to the current teamColor.
        */
        
        Material tankMaterial = GetComponent<Renderer>().material;
        tankMaterial.SetColor("_BaseColor", col);

        // Change turrt color if it's a not power up
        if (this.gameObject.transform.Find("Turret"))
        {
            Material turretMaterial = this.gameObject.transform.Find("Turret").GetComponent<Renderer>().material;
            turretMaterial.SetColor("_BaseColor", col);
        }
    }

    private void UpdateHealthVisual()
    {
        float healthRatio = health / 100f;

        if (healthRatio > 0.4f)
        {
            ChangeTankColor(this.teamColor * healthRatio);
        }
        else
        {
            ChangeTankColor(this.teamColor * 0.4f);
        }
    }

    //public void SetHealthBar(GameObject healthBar, int barNum)
    //{
    //    this.healthBar = healthBar;
    //    this.hBarNumber = barNum;
    //}

    //private void UpdateHealthBar()
    //{
    //    Transform barTransform = healthBar.transform.Find("HealthBar").GetComponent<Image>().transform;

    //    if (health <= 0)
    //    {
    //        barTransform.localScale = Vector3.zero;
    //    }
    //    else
    //    {
    //        float healthRatio = health / 100f;

    //        barTransform.localScale = new Vector3(healthRatio * 0.5f, barTransform.localScale.y, barTransform.localScale.z);
    //    }

    //    if (health < 40)
    //    {
    //        healthBar.transform.Find("HealthBar").GetComponent<Image>().color = Color.red;
    //    }

    //}

    protected virtual void OnTankDestroyed()
    {
        TankDestroyed?.Invoke(this.gameObject);
    }
}