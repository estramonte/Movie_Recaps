using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform[] targetCameraPositions;
    private float delayBeforeFirstTransition = 2.0f;
    private float firstTransitionDuration = 2.0f;
    private float secondTransitionDuration = 4.0f; // Time to transition between positions
    [SerializeField] private Camera camera;


    public IEnumerator StartCameraTransition()
    {
        // Wait for the specified delay before starting the transition for watching the guy sit
        yield return new WaitForSeconds(delayBeforeFirstTransition);

        // Start the camera transition Coroutine
        StartCoroutine(BeingTransitions());
    }

     private IEnumerator BeingTransitions()
    {
        // Move to the first camera position
        yield return StartCoroutine(TransitionCamera(targetCameraPositions[0], firstTransitionDuration));

        // Wait for a second at the first position
        yield return new WaitForSeconds(0.5f);

        // Move to the next camera positions
        for (int i = 1; i < targetCameraPositions.Length; i++)
        {
            yield return StartCoroutine(TransitionCamera(targetCameraPositions[i], secondTransitionDuration));
        }
    }

    private IEnumerator TransitionCamera(Transform targetPosition, float duration)
    {
        Transform originalTransform = camera.transform;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate the position between the original and target positions
            camera.transform.position = Vector3.Lerp(originalTransform.position, targetPosition.position, elapsedTime / duration);

            // Interpolate the rotation between the original and target rotations (optional)
            camera.transform.rotation = Quaternion.Slerp(originalTransform.rotation, targetPosition.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the exact target position
        camera.transform.position = targetPosition.position;
        camera.transform.rotation = targetPosition.rotation;
    }
}