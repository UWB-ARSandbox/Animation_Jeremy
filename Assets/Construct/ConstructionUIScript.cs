using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ConstructionUIScript : MonoBehaviour {

    public UWBNetworkingPackage.NetworkManager network;
    public Button start, prev, next, pause;
    public Dropdown instruction;
    public Text place;
    private GameObject objects;

    public GameObject m_kitten;
    private TangoPointCloud m_pointCloud;

    void Start()
    {
        m_pointCloud = FindObjectOfType<TangoPointCloud>();

    }

    // Update is called once per frame
    void Update()
    {
        if (start.enabled)
        {
            if (PhotonNetwork.connectedAndReady)
                start.interactable = true;
            else
                start.interactable = false;
        }
        if (Input.GetKeyDown("space") && place.IsActive())
        {
            dumbplace();
        }
        if (Input.touchCount == 1)
        {
            //Trigger placepictureframe function when single touch ended.
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //Make sure that pointer is not over UI before calling  PlacePictureFrame
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (place.IsActive())
                        PlaceThing(t.position);
                }
            }
        }

    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    void dumbplace()
    {
        objects = network.Instantiate(instruction.options[instruction.value].text, Vector3.zero, Quaternion.identity, Vector3.one);

        instruction.gameObject.SetActive(false);
        prev.gameObject.SetActive(true);
        next.gameObject.SetActive(true);
        pause.gameObject.SetActive(true);

        next.onClick.AddListener(objects.GetComponent<InstructScript>().Next);
        prev.onClick.AddListener(objects.GetComponent<InstructScript>().Prev);
        pause.onClick.AddListener(objects.GetComponent<InstructScript>().Pause);
    }
    void PlaceThing(Vector2 touchPosition)
    {
        // Find the plane.
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;
        if (!m_pointCloud.FindPlane(cam, touchPosition, out planeCenter, out plane))
        {
            Debug.Log("cannot find plane.");
            return;
        }

        // Place obj on the surface, and make it always face the camera.
        if (Vector3.Angle(plane.normal, Vector3.up) < 30.0f)
        {
            Vector3 up = plane.normal;
            Vector3 right = Vector3.Cross(plane.normal, cam.transform.forward).normalized;
            Vector3 forward = Vector3.Cross(right, plane.normal).normalized;


            //network.Instantiate("smallcube", planeCenter, Quaternion.LookRotation(forward, up), Vector3.one);
            //Instantiate(m_kitten, planeCenter, Quaternion.LookRotation(forward, up));
            objects = network.Instantiate(instruction.options[instruction.value].text, planeCenter, Quaternion.LookRotation(forward, up), Vector3.one);
            

            place.gameObject.SetActive(false);
            instruction.gameObject.SetActive(false);
            prev.gameObject.SetActive(true);
            next.gameObject.SetActive(true);
            pause.gameObject.SetActive(true);

            next.onClick.AddListener(objects.GetComponent<InstructScript>().Next);          
            prev.onClick.AddListener(objects.GetComponent<InstructScript>().Prev);
            pause.onClick.AddListener(objects.GetComponent<InstructScript>().Pause);

        }
        else
        {
            Debug.Log("surface is too steep ");
        }
    }
    //Debug.Log(instruction.options[instruction.value].text);

    private void OnApplicationQuit()
    {
        Destroy(objects);
    }
}
