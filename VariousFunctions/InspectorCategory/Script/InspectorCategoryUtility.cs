using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// ī�װ� ���� ��� ���ڿ��� �����ϴ� Ŭ����
public static class CategoryNameConstants
{
    public const string CategoryNameField = "ī�װ� �̸�";
    public const string AddButtonLabel = "ī�װ� �߰�";
    public const string RemoveButtonLabel = "ī�װ� ����";
    public const string AddFailMessage = "�̹� �����ϴ� ī�װ��Դϴ�.";
    public const string MoveButtonLabel = "ī���� �̵�";
    public const string CategoryOrderKey = "CategoryOrder";
    public const string PlaceholderText = " �˻��� ��ũ��Ʈ �̸��� �Է��ϼ���. ";
}
 
// ����Ƽ ����� ī������ ��ϵ� ��ũ��Ʈ �Ⱥ��̰� ó��
[InitializeOnLoad]
public static class InspectorCategoryHider
{
    private const string SessionKey = "InspectorCategoryHider_Executed"; // ���� ���θ� ������ Ű
    static InspectorCategoryHider()
    { 
        // ����Ƽ ���� �� ȣ��
        EditorApplication.delayCall += TryHideScripts;
    }

    [InitializeOnLoadMethod]
    private static void TryHideScripts()
    {
        if (!SessionState.GetBool(SessionKey, false))
        {
            EditorApplication.delayCall += () =>
            {
                HideScriptsInCategories();
                SessionState.SetBool(SessionKey, true);
            };
        }
    }

    private static void HideScriptsInCategories()
    {
        // ��� InspectorCategory ������Ʈ ã�� 2023���� : FindObjectsByType -> FindObjectsOfType
        InspectorCategory[] allCategories = Object.FindObjectsByType<InspectorCategory>(FindObjectsSortMode.None);

        if (allCategories.Length == 0)
        {
            Debug.LogWarning("InspectorCategory ��ü�� ã�� �� �����ϴ�.");
            return;
        }

        foreach (var category in allCategories)
        {
            HideScriptsRecursively(category.categories);
        }
        Debug.Log("��?");
    }

    private static void HideScriptsRecursively(List<InspectorCategory.Category> categories)
    {
        if (categories == null) return;

        foreach (var category in categories)
        {
            // �ش� ī�װ��� ��ũ��Ʈ �����
            foreach (var script in category.scripts)
            {
                if (script != null)
                {
                    script.hideFlags = HideFlags.HideInInspector;
                    SerializedObject so = new SerializedObject(script);
                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(script);
                }
            }

            // ���� ī�װ��� ��������� ó��
            HideScriptsRecursively(category.subCategories);
        }
    }
}

// �̸��� �Է¹޴� ��ȭâ�� �����ϴ� Ŭ����
public class CategoryNameDialog : EditorWindow
{
    public System.Action<string> OnCategoryNameSubmitted;
    private string categoryName = "";

    private void OnGUI()
    {
        EditorGUILayout.LabelField("ī�װ� �̸� �Է�", EditorStyles.boldLabel);
        categoryName = EditorGUILayout.TextField("�̸�", categoryName);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Ȯ��"))
        {
            OnCategoryNameSubmitted?.Invoke(categoryName);
            Close();
        }
        if (GUILayout.Button("���"))
        {
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}

// ī�װ��� �����ϰų� ������ �� �ִ� ��ȭâ�� �����ϴ� Ŭ����
public class CategoryEditDialog : EditorWindow
{
    private SerializedProperty categoryProperty;
    private InspectorCategory targetCategory;
    List<InspectorCategory.Category> categories;
    private string categoryName;
    private Color categoryColor;

    public void Init(SerializedProperty categoryProperty, List<InspectorCategory.Category> categories, InspectorCategory targetCategory)
    {
        this.categoryProperty = categoryProperty;
        this.categories = categories;
        this.targetCategory = targetCategory;
        categoryName = categoryProperty.FindPropertyRelative("categoryName").stringValue;
        categoryColor = categoryProperty.FindPropertyRelative("categoryColor").colorValue;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("ī�װ� ����", EditorStyles.boldLabel);
        categoryName = EditorGUILayout.TextField("�̸�", categoryName);
        categoryColor = EditorGUILayout.ColorField("����", categoryColor);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("����"))
        {
            categoryProperty.FindPropertyRelative("categoryName").stringValue = categoryName;
            categoryProperty.FindPropertyRelative("categoryColor").colorValue = categoryColor;
            categoryProperty.serializedObject.ApplyModifiedProperties();
            Close();
        }
        if (GUILayout.Button("���"))
        {
            Close();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("����"))
        {
            if (EditorUtility.DisplayDialog("ī�װ� ����", $"���� '{categoryName}' ī�װ��� �����Ͻðڽ��ϱ�?", "����", "���"))
            {
                RemoveCategory(categories, categoryName);
                Close();
            }
        } 
        EditorGUILayout.EndHorizontal();
    }

    private void RemoveCategory(List<InspectorCategory.Category> categories, string categoryName)
    {
        var categoryToRemove = categories.FirstOrDefault(c => c.categoryName == categoryName);
        if (categoryToRemove != null)
        {  
            RecursionSubCategoriesRemove(categoryToRemove);

            foreach (var script in categoryToRemove.scripts)
            {
                if (script != null)
                {
                    script.hideFlags = HideFlags.None;
                    EditorUtility.SetDirty(script);
                }
            }

            categories.Remove(categoryToRemove); 
            EditorUtility.SetDirty(targetCategory);
        }
    }
    private void RecursionSubCategoriesRemove(InspectorCategory.Category category)
    {
        foreach (var subCategory in category.subCategories)
        {
            RecursionSubCategoriesRemove(subCategory);  

            foreach (var script in subCategory.scripts)
            {
                if (script != null)
                {
                    script.hideFlags = HideFlags.None;
                    EditorUtility.SetDirty(script);
                }
            }
        }
    }
} 