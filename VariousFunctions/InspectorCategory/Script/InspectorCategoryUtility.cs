using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// 카테고리 관련 상수 문자열을 정의하는 클래스
public static class CategoryNameConstants
{
    public const string CategoryNameField = "카테고리 이름";
    public const string AddButtonLabel = "카테고리 추가";
    public const string RemoveButtonLabel = "카테고리 삭제";
    public const string AddFailMessage = "이미 존재하는 카테고리입니다.";
    public const string MoveButtonLabel = "카데고리 이동";
    public const string CategoryOrderKey = "CategoryOrder";
    public const string PlaceholderText = " 검색할 스크립트 이름을 입력하세요. ";
}
 
// 유니티 실행시 카데고리에 등록된 스크립트 안보이게 처리
[InitializeOnLoad]
public static class InspectorCategoryHider
{
    private const string SessionKey = "InspectorCategoryHider_Executed"; // 실행 여부를 저장할 키
    static InspectorCategoryHider()
    { 
        // 유니티 실행 후 호출
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
        // 모든 InspectorCategory 오브젝트 찾기 2023이전 : FindObjectsByType -> FindObjectsOfType
        InspectorCategory[] allCategories = Object.FindObjectsByType<InspectorCategory>(FindObjectsSortMode.None);

        if (allCategories.Length == 0)
        {
            Debug.LogWarning("InspectorCategory 객체를 찾을 수 없습니다.");
            return;
        }

        foreach (var category in allCategories)
        {
            HideScriptsRecursively(category.categories);
        }
        Debug.Log("잉?");
    }

    private static void HideScriptsRecursively(List<InspectorCategory.Category> categories)
    {
        if (categories == null) return;

        foreach (var category in categories)
        {
            // 해당 카테고리의 스크립트 숨기기
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

            // 하위 카테고리도 재귀적으로 처리
            HideScriptsRecursively(category.subCategories);
        }
    }
}

// 이름을 입력받는 대화창을 제공하는 클래스
public class CategoryNameDialog : EditorWindow
{
    public System.Action<string> OnCategoryNameSubmitted;
    private string categoryName = "";

    private void OnGUI()
    {
        EditorGUILayout.LabelField("카테고리 이름 입력", EditorStyles.boldLabel);
        categoryName = EditorGUILayout.TextField("이름", categoryName);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("확인"))
        {
            OnCategoryNameSubmitted?.Invoke(categoryName);
            Close();
        }
        if (GUILayout.Button("취소"))
        {
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}

// 카테고리를 수정하거나 삭제할 수 있는 대화창을 제공하는 클래스
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
        EditorGUILayout.LabelField("카테고리 수정", EditorStyles.boldLabel);
        categoryName = EditorGUILayout.TextField("이름", categoryName);
        categoryColor = EditorGUILayout.ColorField("색상", categoryColor);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("저장"))
        {
            categoryProperty.FindPropertyRelative("categoryName").stringValue = categoryName;
            categoryProperty.FindPropertyRelative("categoryColor").colorValue = categoryColor;
            categoryProperty.serializedObject.ApplyModifiedProperties();
            Close();
        }
        if (GUILayout.Button("취소"))
        {
            Close();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("삭제"))
        {
            if (EditorUtility.DisplayDialog("카테고리 삭제", $"정말 '{categoryName}' 카테고리를 삭제하시겠습니까?", "삭제", "취소"))
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