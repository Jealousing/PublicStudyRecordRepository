using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[DisallowMultipleComponent]
public class InspectorCategory : MonoBehaviour
{
    [System.Serializable]
    public class Category
    {
        public string categoryName;
        public List<MonoBehaviour> scripts = new List<MonoBehaviour>();
        public List<Category> subCategories = new List<Category>();

        public bool isExpanded = false;
        public bool preExpanded = false;
        public List<bool> scriptExpanded = new List<bool>();
        public bool isActive = true;
        public bool isDisplay = true;
        public Color categoryColor = Color.white;  

        public Category(string categoryName)
        {
            this.categoryName = categoryName;
            isExpanded = false;
            scriptExpanded = new List<bool>();
        }
    }

    public List<Category> categories = new List<Category>();
    public bool editExpanded = false; 

    #region ContextMenu
    [ContextMenu("Safe Remove Component")]
    private void SafeRemoveComponent()
    {
        foreach (var category in categories)
        {
            foreach (var script in category.scripts)
            {
                if (script != null && script.hideFlags != HideFlags.None)
                    script.hideFlags = HideFlags.None;
            }
        }

        DestroyImmediate(this);
    }

    [ContextMenu("Show All Hidden Scripts")]
    private void ShowAllHiddenScripts()
    {
        MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();

        foreach (var script in allScripts)
        {
            if (script != null && script.hideFlags != HideFlags.None)
                script.hideFlags = HideFlags.None;
        }
    }
    #endregion 
}
