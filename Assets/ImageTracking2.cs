using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ImageTracking2: MonoBehaviour
{

    public string ReferenceImageName;
    private ARTrackedImageManager _TrackedImageManager;
    public GameObject garden;
    public Button Button1;
    private TMP_Text buttonText;

    public bool showGarden;

    void Start()
    {
        // Encontra o componente TextMeshPro do filho e altera o texto
        buttonText = Button1.transform.GetChild(0).GetComponent<TMP_Text>();
    }

/*public Behaviour Canvas;
public Button Button1;
public Button Button2;
public GameObject Anchored;*/

/*void Start()
{
    Button1 = GetComponent<Button>();
    Button2 = GetComponent<Button>();
}*/

Vector3 desiredRotation = new Vector3(-90f, 0f, 0f);

    private void Awake()
    {
        _TrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }
    private void OnEnable()
    {
        if (_TrackedImageManager != null)
        {
            _TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }
    private void OnDisable()
    {
        if (_TrackedImageManager != null)
        {
            _TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs e)
    {
        foreach (var trackedImage in e.added)
        {
            Debug.Log($"Tracked image detected: {trackedImage.referenceImage.name} with size: {trackedImage.size}");
        }
        UpdateTrackedImages(e.added);
        UpdateTrackedImages(e.updated);
    }

    private void UpdateTrackedImages(IEnumerable<ARTrackedImage> trackedImages)
    {
        // If the same image (ReferenceImageName)
        var trackedImage = trackedImages.FirstOrDefault(x => x.referenceImage.name == ReferenceImageName);
        if (trackedImage == null)
        {
            return;
        }
        
        if (trackedImage.trackingState == TrackingState.Tracking)
            {
                var trackedImageTransform = trackedImage.transform;
                transform.SetPositionAndRotation(trackedImageTransform.position, Quaternion.Euler(desiredRotation));
                CancelInvoke();
                
            }
        
        /*if (trackedImage.trackingState != TrackingState.Tracking)
            // when TrackingState == TrackingState.Limited or .None
            {
                Invoke("DisableMe", 1f);
            }*/
        }
 
    /*private void DisableMe()
    {
        gameObject.transform.position = new Vector3(0,5000,0);
    }*/

    public void gardenOn()
    {
        showGarden = !showGarden;
        if (showGarden )
        {
            garden.SetActive(true);
            buttonText.text = "House Only";
        }
        else
        {
            garden.SetActive(false);
            buttonText.text = "See Complete";
        }
    }
}

