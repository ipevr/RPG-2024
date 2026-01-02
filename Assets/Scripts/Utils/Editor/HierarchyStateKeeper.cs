using UnityEditor;

namespace Utils.Editor
{
    [InitializeOnLoad]
    public static class HierarchyStateKeeper
    {
        static HierarchyStateKeeper()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // This is a hook where you could implement state saving logic.
            // However, modern Unity versions (2022+) actually try to preserve 
            // this better than older ones.
        
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                // This forces the window to repaint and often helps 
                // maintain the focus on the previously selected object.
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}