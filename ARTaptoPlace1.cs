using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTaptoPlace1 : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager aRPlaneManager;

    public GameObject gameObjToInstantiate;

    public GameObject spawnedObj;

    private Vector2 touchPos;
    private ARRaycastManager aRRaycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public ARSession session;

    private void OnEnable() 
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            Input.location.Stop();
            Debug.Log("stopped");
        }
    }

    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();

        aRPlaneManager = GetComponent<ARPlaneManager>();

        // session = GetComponent<ARSession>();
    }

    /// <summary>
    /// Getting the touch position from the user
    /// </summary>
    /// <param name="touchPos"></param>
    /// <returns></returns>

    bool TryGetTouchPosition(out Vector2 touchPos)
    {
        if (Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }

        touchPos = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!TryGetTouchPosition(out Vector2 touchPos))
            return;

        if (!IsPointOverUIObject(touchPos) && aRRaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = hits[0].pose;

            if (spawnedObj == null)
            {
                Debug.Log(gameObjToInstantiate.name);
                spawnedObj = Instantiate(gameObjToInstantiate, hitPos.position, gameObjToInstantiate.transform.rotation);

                 ToggleFunction();
            }

        }

        if(!IsPointOverUIObject(touchPos) && spawnedObj != null)
        {
             spawnedObj.transform.Rotate(0, 20 * Time.deltaTime, 0);
            
        }
    }
    public void ToggleFunction()
    {
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
    }

    public void ToggleFunction1()
    {
        Destroy(spawnedObj);
        session.enabled = false;
        session.Reset();

        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

    }

    public void PrefabPlace(GameObject prefab)
    {
        gameObjToInstantiate = prefab;
    }

    public void SessionTrue()
    {
        session.enabled = true;
        aRPlaneManager.enabled = true;
    }

    public void RotateObject()
    {
        spawnedObj.transform.Rotate(0,45* Time.deltaTime, 0);
    }

    private void OnDisable()
    {
        if(Input.location.status == LocationServiceStatus.Stopped)
        {
            Input.location.Start();

        }

    }

    bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

}
