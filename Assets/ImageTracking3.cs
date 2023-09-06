using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ImageTracking3 : MonoBehaviour
{

    public string ReferenceImageName;
    private ARTrackedImageManager _TrackedImageManager;

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
            transform.SetPositionAndRotation(trackedImageTransform.position, trackedImageTransform.rotation);
            CancelInvoke();
        }

        if (trackedImage.trackingState != TrackingState.Tracking)
        // when TrackingState == TrackingState.Limited or .None
        {
            Invoke("DisableMe", 1f);
        }
    }

    private void DisableMe()
    {
        gameObject.transform.position = new Vector3(0, 5000, 0);
    }
}
