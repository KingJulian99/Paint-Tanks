using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadTankController : MonoBehaviour
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
    private TankMovement tankMovement;

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
        this.tankMovement = this.GetComponent<TankMovement>();
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
    }

    void Update()
    {

        ChangeTankColor();

        GroundPaintCheck();

        if ( !this.characterController.isGrounded && this.alive ) {
            this.characterController.Move( new Vector3(0.0f, this.gravity * Time.deltaTime, 0.0f) ); // applies "gravity"

            if(this.rotatingUncontrollably) {
                this.transform.Rotate(this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime);
            }
        } 
        
        if(this.health <= 0 && this.alive) {
            this.alive = false;
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
        print("groundpaint check for gamepad tank");
        /*
            This function casts a ray below the center of the tank, getting the color directly below.
            If the color is deemed to be that of another team, the tank is slowed.
        */

        RaycastHit hit;

        if (!Physics.Raycast(transform.position, -Vector3.up, out hit)) {return;}

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        // Do check here
        if(hit.collider.tag == "Ground") {

            print("hit ground");

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.GetTexture("_MaskTexture") == null || meshCollider == null) {return;}

            print("passed checks");

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
            print(colorBelow);
            if(colorBelow != "none" && colorBelow != GetTeamColor(this.teamColor)) {
                this.tankMovement.slowDown = 2.0f;
                if(!this.alertEffect.isPlaying) {
                    this.alertEffect.Play();
                }
                if(this.alertEffectRemove.isPlaying) {
                    this.alertEffectRemove.Pause();
                    this.alertEffectRemove.Clear();
                }
            } else { 
                this.tankMovement.slowDown = 0.0f;
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

    public void Explode() {
        // Play particlesystem
        this.explosion.Play();

        // Deactivate scripts that can get in the way.
        Invoke("Deactivate", 0.4f);

        // Send to the graveyard
        Invoke("MoveToGraveyard", 0.45f);
        Invoke("Revive", 0.45f);
    }

    private void Deactivate() {
        this.transform.GetComponent<CharacterController>().enabled = false;
        this.transform.GetComponent<TankMovement>().enabled = false;
        this.transform.GetComponent<GamepadTankController>().enabled = false;
    }

    public void MoveToGraveyard() {
        this.transform.position = new Vector3(0, -10, 0);
    }

    public void Revive() {
        this.rotatingUncontrollably = false;
        this.alive = true;
        this.health = 100;
        OnTankDestroyed();
    }

    protected virtual void OnTankDestroyed()
    {
        TankDestroyed?.Invoke(this.gameObject);
    }

    public void RotateUncontrollably() {
        this.rotatingUncontrollably = true;
        Invoke("Explode", 2f);
    }

    public void SetGravity(float gravity) {
        this.gravity = gravity;
    }

    private void ChangeTankColor() {
        /*
            This method sets both the tank and turret prefab colors to the current teamColor.
        */
        
        Material tankMaterial = GetComponent<Renderer>().material;
        tankMaterial.SetColor("_BaseColor", this.teamColor);
        Material turretMaterial = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        turretMaterial.SetColor("_BaseColor", this.teamColor);
    }
}