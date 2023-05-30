using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class XRInstantiateGrabbableObject : XRBaseInteractable
{
    [SerializeField]
    private GameObject grabbableObject;

    //[SerializeField]
    //private Transform transformToInstantiate;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (this.GetComponent<PriceComponent>().Buy())
        {
            // Instantiate object
            GameObject newObject = Instantiate(grabbableObject, ControllersSingleton.Instance.RightController.transform.position, Quaternion.identity);
            // Get grab interactable from prefab
            XRGrabInteractable objectInteractable = newObject.GetComponent<XRGrabInteractable>();

            // Select object into same interactor
            interactionManager.SelectEnter(args.interactorObject, objectInteractable);

            base.OnSelectEntered(args);
        }
    }
}