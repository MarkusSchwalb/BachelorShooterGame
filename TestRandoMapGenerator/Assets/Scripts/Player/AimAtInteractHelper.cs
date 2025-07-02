using System;
using UnityEngine;

public class AimAtInteractHelper : MonoBehaviour
{
    private Interactable lastInterActable;
    [field: SerializeField] private LayerMask mask;
    private Player player;
    [field: SerializeField] public InteractableDisplay IDisplay {  get; private set; }
    private bool interactable = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
        player.InputReader.InteractEvent += HandleInteract;
    }

    private void HandleInteract()
    {
        if (!interactable) return;
        lastInterActable.Interact();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAimAt();
    }

    private void UpdateAimAt()
    {
        if (player.AimAt == null) return;

        Vector3 aimAtLoc;
        //screencenter
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        //ray 
        Ray ray = new Ray(player.Camera.transform.position, player.Camera.transform.forward);
        RaycastHit hit;

        //raycast
        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            aimAtLoc = hit.point;


            if (hit.transform.gameObject.CompareTag("Interactable") && hit.distance < 6)
            {
                Debug.Log("Sees Interactable");
                if (hit.transform.TryGetComponent<Interactable>(out Interactable component))
                {
                       
                    IDisplay.gameObject.SetActive(true);
                    if (component != lastInterActable)
                    {
                        lastInterActable = component;
                        IDisplay.ChangeText(component);
                    }
                    interactable = true;
                }
            }
            else
            {
                IDisplay.gameObject.SetActive(false);
                interactable = false;
            }
        }
        else
        {
            aimAtLoc = (player.Camera.transform.position 
                + (player.Camera.transform.forward * 100f)) 
                - player.Camera.transform.position; //otherwise aim just 100 m to the front
            IDisplay.gameObject.SetActive(false);
            interactable = false;
        }


        player.AimAt.transform.position = aimAtLoc;
    }
}
