using UnityEngine;

namespace GGTools.BuildTools
{

    /// <summary>
    /// Displays the current application version on screen during runtime.
    /// 
    /// This component renders a simple overlay using Unity's OnGUI system,
    /// allowing developers or testers to easily identify the current build version.
    /// 
    /// The overlay position, style, margins, and visibility behavior can be configured
    /// through the inspector.
    /// </summary>
    public class VersionOverlay : MonoBehaviour
    {

        [SerializeField] bool onlyInDevelopmentBuild = false;
        [SerializeField] Vector2 margin = new Vector2(12, 12);
        [SerializeField] VersionOverlayPosition position = VersionOverlayPosition.TopLeft;
        [SerializeField] int fontSize = 16;
        [SerializeField] Color color = Color.white;
        [SerializeField] private bool dontDestroyOnLoad;
        [SerializeField] private Vector2 textSize = new Vector2(600, 40);

        /*   [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
           static void EnsureExists()
           {
               if(FindAnyObjectByType<VersionOverlay>() != null)
               {
                   return;
               }
               GameObject go = new GameObject("VersionOverlay (Auto)");
               go.AddComponent<VersionOverlay>();
               DontDestroyOnLoad(go);
           }*/

        void Awake()
        {
            var other = FindAnyObjectByType<VersionOverlay>();

            if (other != null && other != this)
            {
                Destroy(gameObject);
                return;
            }
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Draws the version label on the screen using Unity's immediate mode GUI system.
        /// The label displays the current <see cref="Application.version"/> and respects
        /// the configured position, margins, font size, and color.
        /// </summary>

        void OnGUI()
        {
            if (onlyInDevelopmentBuild && !Debug.isDebugBuild) return;

            string txt = $"v{Application.version}";

            var style = new GUIStyle(GUI.skin.label);
            style.fontSize = fontSize;
            style.normal.textColor = color;

            switch (position)
            {
                default:
                case VersionOverlayPosition.TopLeft:
                    style.alignment = TextAnchor.UpperLeft;
                    break;

                case VersionOverlayPosition.TopRight:
                    style.alignment = TextAnchor.UpperRight;
                    break;

                case VersionOverlayPosition.BottomLeft:
                    style.alignment = TextAnchor.LowerLeft;
                    break;

                case VersionOverlayPosition.BottomRight:
                    style.alignment = TextAnchor.LowerRight;
                    break;
            }
            Rect rect = GetRect(position, margin, textSize.x, textSize.y);
            GUI.Label(new Rect(rect), txt, style);
        }

        Rect GetRect(VersionOverlayPosition pos, Vector2 margin, float width, float height)
        {
            switch (pos)
            {
                case VersionOverlayPosition.TopRight:
                    return new Rect(
                        Screen.width - width - margin.x,
                        margin.y,
                        width,
                        height
                    );

                case VersionOverlayPosition.BottomLeft:
                    return new Rect(
                        margin.x,
                        Screen.height - height - margin.y,
                        width,
                        height
                    );

                case VersionOverlayPosition.BottomRight:
                    return new Rect(
                        Screen.width - width - margin.x,
                        Screen.height - height - margin.y,
                        width,
                        height
                    );

                default: // TopLeft
                    return new Rect(
                        margin.x,
                        margin.y,
                        width,
                        height
                    );
            }
        }
    }

    public enum VersionOverlayPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}