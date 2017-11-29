using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        // INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
        [SerializeField] int[] layerPriorities;

        float maxRaycastDepth = 100f; // Hard coded value
        int topPriorityLayerLastFrame = -1; // So get ? from start with Default layer terrain

        // Setup delegates for broadcasting layer changes to other classes
        public delegate void OnCursorLayerChange(int newLayer); // declare new delegate type
        public event OnCursorLayerChange NotifyLayerChangeObservers; // instantiate an observer set

        public delegate void OnClickPriorityLayerLeft(RaycastHit raycastHit, int layerHit); // declare new delegate type for left click
        public event OnClickPriorityLayerLeft NotifyLeftMouseClickObservers; // instantiate an observer set for left click

        public delegate void OnClickPriorityLayerRight(RaycastHit raycastHit, int layerHit); // declare new delegate type for right click
        public event OnClickPriorityLayerRight NotifyRightMouseClickObservers; // instantiate an observer set for right click


        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                NotifyObserersIfLayerChanged(5);
                return; // Stop looking for other objects
            }

            // Raycast to max depth, every frame as things can move under mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

            RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);
            if (!priorityHit.HasValue) // if hit no priority object
            {
                NotifyObserersIfLayerChanged(0); // broadcast default layer
                return;
            }

            // Notify delegates of layer change
            var layerHit = priorityHit.Value.collider.gameObject.layer;
            NotifyObserersIfLayerChanged(layerHit);

            // Notify delegates of highest priority game object under mouse when left button clicked
            if (Input.GetMouseButton(0))
            {
                NotifyLeftMouseClickObservers(priorityHit.Value, layerHit);
            }
            
            // Notify delegates of highest priority game object under mouse when right button clicked
            if (Input.GetMouseButtonDown(1))
            {
                NotifyRightMouseClickObservers(priorityHit.Value, layerHit);
            }


        }

        void NotifyObserersIfLayerChanged(int newLayer)
        {
            if (newLayer != topPriorityLayerLastFrame)
            {
                topPriorityLayerLastFrame = newLayer;
                NotifyLayerChangeObservers(newLayer);
            }
        }

        RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHits)
        {
            // Form list of layer numbers hit
            List<int> layersOfHitColliders = new List<int>();
            foreach (RaycastHit hit in raycastHits)
            {
                layersOfHitColliders.Add(hit.collider.gameObject.layer);
            }

            // Step through layers in order of priority looking for a gameobject with that layer
            foreach (int layer in layerPriorities)
            {
                foreach (RaycastHit hit in raycastHits)
                {
                    if (hit.collider.gameObject.layer == layer)
                    {
                        return hit; // stop looking
                    }
                }
            }
            return null; // because cannot use GameObject? nullable
        }
    }
}