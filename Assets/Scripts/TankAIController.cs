using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Event to detect Victory
public delegate void VictoryNotify();

public class TankAIController : MonoBehaviour
{

    public float driveSpeed;
    public float rotateSpeed;
    public int health;
    public Color teamColor;
    public int bounceNumber;

    private CharacterController characterController;
    private float randomRotationSpeed; 
    private float gravity;
    private bool rotatingUncontrollably;
    private ParticleSystem alertEffect;
    private ParticleSystem alertEffectRemove;

    // AI fields
    private NavMeshAgent navAgent;
    private List<GameObject> targets;
    private GameObject playerContainer;
    public GameObject target;
    public bool lineOfSight;
    public bool gotTargets = false;

    public event VictoryNotify Victory;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerContainer = GameObject.Find("PlayerContainer");
        lineOfSight = false;

        SpawnScript s = GameObject.Find("SpawnManager").GetComponent<SpawnScript>();
        s.SpawnDone += SetUpAI;
    }

    void Start()
    {
        this.driveSpeed = 5.0f;
        this.rotateSpeed = 55.0f;
        this.health = 100;
        this.characterController = this.GetComponent<CharacterController>();
        this.bounceNumber = 0;
        this.randomRotationSpeed = Random.Range(10.0f, 20.0f);
        this.gravity = -9.0f;
        this.rotatingUncontrollably = false;
        this.alertEffect = this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        this.alertEffectRemove = this.gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();

        ChangeTankColor();
    }

    void Update()
    {
        // if there are no targets do nothing
        if (!gotTargets) { return; }

        ChangeTankColor();

        GroundPaintCheck();

        UpdateTargets();

        target = GetTarget();

        if ( !this.characterController.isGrounded ) {
            this.characterController.Move( new Vector3(0.0f, this.gravity * Time.deltaTime, 0.0f) ); // applies "gravity"

            if(this.rotatingUncontrollably) {
                this.transform.Rotate(this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime);
            }

        } else {
            

            if (target != null && !LineOfSight())
            {
                // Move toward target
                navAgent.SetDestination(target.transform.position);
            }
            else
            {
                // Stop at current destination
                navAgent.SetDestination(gameObject.transform.position);
            }
        }
        
        if(this.health <= 0) {
            Destroy(this.gameObject);
        }

    }

    // Executes when all players and ai are spawned
    // Gets the list of targets
    private void SetUpAI()
    {
        targets = new List<GameObject>();

        foreach (Transform child in playerContainer.transform)
        {
            if (child.gameObject != gameObject)
            {
                targets.Add(child.gameObject);
            }
        }

        gotTargets = true;
    }

    // Checks whether the AI has line of sight of its target
    private bool LineOfSight()
    {
        NavMeshHit hit;

        if (!navAgent.Raycast(target.transform.position, out hit))
        {
            lineOfSight = true;
            return true;
        }

        lineOfSight = false;
        return false;
    }

    // Randomly get target from list of spawned players and AI
    private GameObject GetTarget()
    {
        // Check if target is not dead
        if (target != null) { return target; }

        // Assign new target
        if (targets.Count > 0)
        {
            int spwn = Random.Range(0, targets.Count);
            GameObject t = targets[spwn];

            return t;
        }

        OnVictory();
        return null;
    }

    // Updates the list of targets by removing players that aren't
    // in the scene anymore
    private void UpdateTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }
    }

    protected virtual void OnVictory()
    {
        Debug.Log("Victory");
        Victory?.Invoke();
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

    public void RotateUncontrollably() {
        this.rotatingUncontrollably = true;
    }

    public void SetGravity(float gravity) {
        this.gravity = gravity;
    }

    void ChangeTankColor() {
        /*
            This method sets both the tank and turret prefab colors to the current teamColor.
        */
        
        Material tankMaterial = GetComponent<Renderer>().material;
        tankMaterial.SetColor("_BaseColor", this.teamColor);
        Material turretMaterial = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        turretMaterial.SetColor("_BaseColor", this.teamColor);
    }
}