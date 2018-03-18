using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL.Manipulation.Objects.Android;
using UnityEngine.UI;

//
namespace ASL.Manipulation.Controllers.Android
{
    public class ConstructionInteraction : MonoBehaviour
    {
#if UNITY_ANDROID || UNITY_EDITOR
        private ASL.Manipulation.Objects.ObjectInteractionManager objManager;
        private SelectObject selectBehavior;
        private MoveBehavior moveBehavior;
        private TangoPointCloud m_pointCloud;
        public GameObject placement;
        public Button prev, next;

        //private Vector3 touchBeginPosition;
        //private Vector3 touchEndPosition;

        public void Awake()
        {
            objManager = gameObject.GetComponent<ASL.Manipulation.Objects.ObjectInteractionManager>();
            selectBehavior = gameObject.GetComponent<SelectObject>();
            m_pointCloud = FindObjectOfType<TangoPointCloud>();
            Debug.Log("objmanager"+objManager);
        }
        void Update()
        {
            if(PhotonNetwork.connectionState == ConnectionState.Disconnected)
            {
                placement.SetActive(false);
            }
            if(Input.anyKey)
            {
                Debug.Log("keypress");
                objManager.Instantiate("Simple");
            }
            if (Input.touchCount == 1 && placement.GetActive())
            {
               
                // Trigger place function when single touch ended.
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Ended)
                {
                    PlaceThing(t.position);
                }
            }
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
                
                GameObject g = objManager.Instantiate("Simple", planeCenter, Quaternion.LookRotation(forward, up),Vector3.one);
                objManager.Instantiate("Simple");
                placement.SetActive(false);
                prev.gameObject.SetActive(true);
                next.gameObject.SetActive(true);

                //objManager.Instantiate(g);

                //next.GetComponent<Button>().onClick.AddListener(g.GetComponent<ConstructScript>().next);
                //prev.GetComponent<Button>().onClick.AddListener(g.GetComponent<ConstructScript>().prev);

                //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //obj.transform.localPosition = planeCenter;
                //obj.transform.localRotation = Quaternion.LookRotation(forward, up);
                //obj.transform.localScale = new Vector3(.25f, .25f, .25f);
                //Instantiate(obj, planeCenter, Quaternion.LookRotation(forward, up));
            }
            else
            {
                Debug.Log("surface is too steep ");
            }
        }

#endif
    }
}
