using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Character;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int walkableLayerNumber = 8;

        float maxRaycastDepth = 100f; // Hard coded value

        Rect screenRectOnContruction = new Rect(0, 0, Screen.width, Screen.height);

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverTerrain;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Implement UI interaction

            }
            else
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            if (screenRectOnContruction.Contains(Input.mousePosition))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForWalkable(ray)) { return; }
            }
        }
        bool RaycastForEnemy(Ray ray)
        {
            RaycastHit enemyHitInfo;
            Physics.Raycast(ray, out enemyHitInfo, maxRaycastDepth);
            var gameObjectHit = enemyHitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();

            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        bool RaycastForWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask walkableLayer = 1 << walkableLayerNumber;
            bool walkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, walkableLayer);

            if (walkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverTerrain(hitInfo.point);
                return true;
            }
            return false;
        }




    }
}