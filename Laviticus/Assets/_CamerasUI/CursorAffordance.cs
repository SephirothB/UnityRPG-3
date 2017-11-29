using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {

        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        [SerializeField] const int stiffLayerNumber = 10;




        CameraRaycaster cameraRaycaster;
        public Vector3 mousePos;

        // Use this for initialization
        void Start()
        {

            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.NotifyLayerChangeObservers += OnLayerChanged;



        }




        void OnLayerChanged(int newLayer)
        {
            switch (newLayer)
            {
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); break;
                case enemyLayerNumber:
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto); break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }
        }
    }
}