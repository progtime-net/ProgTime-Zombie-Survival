using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class TurretPlacementManager : NetworkBehaviour
{
    [Header("Настройка турели")]
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private float placementDistance = 10f;

    [Header("Настройка размещения")]
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float placementCheckRadius = 1.0f;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private Material canPlaceMaterial;
    [SerializeField] private Material cantPlaceMaterial;

    private GameObject ghostTurret;
    private bool isPlacing = false;
    private Renderer[] ghostRenderers;
    private InputSystem playerInputActions;

    void OnEnable()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new InputSystem();
        }
        playerInputActions.Enable();
        playerInputActions.Player.PlaceTurret.performed += OnPlaceTurretPressed;
    }

    void OnDisable()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Player.PlaceTurret.performed -= OnPlaceTurretPressed;
            playerInputActions.Disable();
        }
    }

    private void OnPlaceTurretPressed(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        if (isPlacing)
        {
            if (CanPlaceTurret())
            {
                PlaceTurret();
            }
        }
        else
        {
            StartPlacingTurret();
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (isPlacing && ghostTurret != null)
        {
            MoveGhostTurret();
        }
    }

    private void StartPlacingTurret()
    {
        isPlacing = true;
        ghostTurret = Instantiate(turretPrefab);

        NetworkIdentity networkIdentity = ghostTurret.GetComponent<NetworkIdentity>();
        if (networkIdentity != null)
        {
            networkIdentity.enabled = false;
        }

        TurretController turretController = ghostTurret.GetComponent<TurretController>();
        if (turretController != null)
        {
            turretController.enabled = false;
        }
        
        ghostRenderers = ghostTurret.GetComponentsInChildren<Renderer>();

        Collider[] colliders = ghostTurret.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void MoveGhostTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placementDistance, placementLayer))
        {
            ghostTurret.transform.position = hit.point;
            
            if (CanPlaceTurret())
            {
                SetGhostMaterial(canPlaceMaterial);
            }
            else
            {
                SetGhostMaterial(cantPlaceMaterial);
            }
        }
    }
    
    private bool CanPlaceTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, placementDistance, placementLayer))
        {
            bool isColliding = Physics.CheckSphere(hit.point, placementCheckRadius, collisionLayers);
            
            return !isColliding;
        }
        return false;
    }
    
    private void SetGhostMaterial(Material material)
    {
        foreach (Renderer renderer in ghostRenderers)
        {
            renderer.material = material;
        }
    }

    private void PlaceTurret()
    {
        if (!isPlacing || ghostTurret == null || !CanPlaceTurret()) return;
        
        CmdPlaceTurret(ghostTurret.transform.position, ghostTurret.transform.rotation);
        
        Destroy(ghostTurret);
        
        isPlacing = false;
    }

    [Command]
    private void CmdPlaceTurret(Vector3 position, Quaternion rotation)
    {
        GameObject newTurret = Instantiate(turretPrefab, position, rotation);
        NetworkServer.Spawn(newTurret);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (ghostTurret != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(ghostTurret.transform.position, placementCheckRadius);
        }
    }
}