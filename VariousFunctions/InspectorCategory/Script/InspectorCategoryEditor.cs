#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
  
[CustomEditor(typeof(InspectorCategory))]
public class InspectorCategoryEditor : Editor
{
    #region Fields
    private InspectorCategory targetCategory;  
    private SerializedProperty serializedCategories; 
    private Dictionary<MonoBehaviour, Editor> scriptEditors = new Dictionary<MonoBehaviour, Editor>();
    private Dictionary<string, SerializedProperty> categoryLookup;
    private HashSet<string> categoryNames = new HashSet<string>(); 
    private string inputText = ""; 
    private string previousInputText = "";
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        targetCategory = target as InspectorCategory;
        serializedCategories = serializedObject.FindProperty("categories");
        InitializeCategoryLookup();
    }
    private void OnDisable()
    {
        foreach (var editor in scriptEditors.Values)
        {
            if (editor != null) DestroyImmediate(editor);
        }
        scriptEditors.Clear();
    } 
    #endregion

    #region Category Management 
    private void InitializeCategoryLookup()
    { 
        categoryLookup = new Dictionary<string, SerializedProperty>();
        for (int i = 0; i < targetCategory.categories.Count; i++)
        {
            var categoryProperty = serializedCategories.GetArrayElementAtIndex(i);
            var categoryName = categoryProperty.FindPropertyRelative("categoryName").stringValue;
            categoryLookup[categoryName] = categoryProperty;
        }
    }
    private SerializedProperty GetCategoryByName(string categoryName) => 
        categoryLookup.TryGetValue(categoryName, out var categoryProperty) ? categoryProperty : null;

    private bool CategoryExists(string categoryName) => categoryNames.Contains(categoryName);
    private void AddCategory(string categoryName)
    {
        if (CategoryExists(categoryName)) return;

        Undo.RecordObject(targetCategory, "Add Category");
        targetCategory.categories.Add(new InspectorCategory.Category(categoryName));
        categoryNames.Add(categoryName);
        EditorUtility.SetDirty(targetCategory);
    }
    private void AddSubCategory(List<InspectorCategory.Category> categories,string parentCategoryName, string subCategoryName)
    {
        var parentCategory = categories.FirstOrDefault(c => c.categoryName == parentCategoryName);
        if (parentCategory == null) return;

        if (parentCategory.subCategories.Any(c => c.categoryName == subCategoryName)) return;  

        Undo.RecordObject(targetCategory, "Add SubCategory");
        parentCategory.subCategories.Add(new InspectorCategory.Category(subCategoryName));
        EditorUtility.SetDirty(targetCategory);
    } 
    private void RemoveCategory(List<InspectorCategory.Category> categories, string categoryName)
    {
        var categoryToRemove = categories.FirstOrDefault(c => c.categoryName == categoryName);
        if (categoryToRemove != null)
        {
            Undo.RecordObject(targetCategory, "Remove Category");

            RecursionSubCategoriesRemove(categoryToRemove);

            foreach (var script in categoryToRemove.scripts)
            {
                if (script != null)
                { 
                    script.hideFlags = HideFlags.None;
                    EditorUtility.SetDirty(script);  
                }
            }
            targetCategory.categories.Remove(categoryToRemove); 
            categoryNames.Remove(categoryName);  
            EditorUtility.SetDirty(targetCategory);
        }
    } 
    private void RecursionSubCategoriesRemove(InspectorCategory.Category category)
    {
        foreach (var subCategory in category.subCategories)
        {
            RecursionSubCategoriesRemove(subCategory); // 재귀 호출

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
    private void MoveCategory(List<InspectorCategory.Category> categories, int index, int newIndex)
    {
        if (newIndex < 0 || newIndex >= categories.Count || index == newIndex) return;

        Undo.RecordObject(targetCategory, "Move Category");
        (categories[index], categories[newIndex]) = (categories[newIndex], categories[index]);
        EditorUtility.SetDirty(targetCategory);
    }
    private void SetCategoryDisplay(InspectorCategory.Category category, bool isActive)
    {
        category.isDisplay = isActive;

        foreach (var subCategory in category.subCategories)
        {
            SetCategoryDisplay(subCategory, isActive); // 하위 카테고리까지 재귀적으로 설정
        }
    }
    private void FilteringCategory(string scriptsName)
    {
        if (previousInputText == scriptsName) return;
        else if (!string.Equals(scriptsName, CategoryNameConstants.PlaceholderText))
        {
            previousInputText = scriptsName;
        }

        bool isFiltering = !string.IsNullOrEmpty(scriptsName) && scriptsName != CategoryNameConstants.PlaceholderText;
         
        foreach (var category in targetCategory.categories)
        {
            category.isDisplay = isFiltering;
            bool isMatching = isFiltering && CheckCategoryMatchingScript(category, scriptsName);
            SetCategoryDisplay(category, isMatching || !isFiltering);
        }

        // 필터링 로직
        if(isFiltering)
        {
            foreach (var category in targetCategory.categories)
            {
                if (CheckCategoryMatchingScript(category, scriptsName))
                {
                    category.isDisplay = true;
                    SetCategoryDisplay(category, true); // 하위 카테고리까지 활성화
                }
            }
        }
        
    }
 
    private bool CheckCategoryMatchingScript(InspectorCategory.Category category, string scriptsName)
    {
        bool isMatchingScript = false;
         
        foreach (var scriptObject in category.scripts)
        {
            string scriptName = scriptObject.GetType().Name;
            if (scriptName.IndexOf(scriptsName, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                isMatchingScript = true;
                break;
            }
        }

        // 하위 카테고리 검사 (재귀 호출)
        foreach (var subCategory in category.subCategories)
        {
            if (CheckCategoryMatchingScript(subCategory, scriptsName))
            {
                isMatchingScript = true;
                break;
            }
        }

        return isMatchingScript;
    }
    #endregion

    #region Script Management
    private bool ContainsScript(SerializedProperty scripts, MonoBehaviour script) =>
        Enumerable.Range(0, scripts.arraySize).Any(i => scripts.GetArrayElementAtIndex(i).objectReferenceValue == script);

    private void AddScriptToCategory(SerializedProperty scripts, MonoBehaviour script)
    {
        Undo.RecordObject(targetCategory, "Add Script to Category");
        scripts.arraySize++;
        scripts.GetArrayElementAtIndex(scripts.arraySize - 1).objectReferenceValue = script;
        if (script.hideFlags == HideFlags.None)
        {
            script.hideFlags = HideFlags.HideInInspector;
            EditorUtility.SetDirty(script);
        }
        serializedObject.ApplyModifiedProperties();
    }
    private void RemoveScriptFromCategory(string targetCategoryName, MonoBehaviour scriptObject)
    {
        var categoryProperty = GetCategoryByName(targetCategoryName);
        if (categoryProperty == null) return;

        var scripts = categoryProperty.FindPropertyRelative("scripts");
        var scriptExpanded = categoryProperty.FindPropertyRelative("scriptExpanded");
        Undo.RecordObject(targetCategory, "Remove Script from Category");

        for (int i = 0; i < scripts.arraySize; i++)
        {
            var scriptProperty = scripts.GetArrayElementAtIndex(i);
            if (scriptProperty.objectReferenceValue == scriptObject)
            {
                scriptProperty.objectReferenceValue = null;
                scripts.DeleteArrayElementAtIndex(i);
                scriptExpanded.DeleteArrayElementAtIndex(i); 
                serializedObject.ApplyModifiedProperties();
                return;
            }
        }
    }
    private void MoveScriptToNewCategory(MonoBehaviour scriptObject, string targetCategoryName, string newCategoryName)
    {
        Undo.RecordObject(targetCategory, "Move Script to New Category");
        var currentCategoryProperty = GetCategoryByName(targetCategoryName);
        if (currentCategoryProperty != null)
        {
            var currentScripts = currentCategoryProperty.FindPropertyRelative("scripts");
            if (currentScripts != null && currentScripts.arraySize > 0)
            {
                RemoveScriptFromCategory(targetCategoryName, scriptObject);
            }
        }

        var newCategoryProperty = GetCategoryByName(newCategoryName);
        if (newCategoryProperty != null)
        {
            var newScripts = newCategoryProperty.FindPropertyRelative("scripts");
            var scriptExpanded = newCategoryProperty.FindPropertyRelative("scriptExpanded");

            if (newScripts != null)
            {
                AddScriptToCategory(newScripts, scriptObject);
                int newScriptIndex = newScripts.arraySize - 1;
                if (scriptExpanded.arraySize <= newScriptIndex)
                {
                    scriptExpanded.InsertArrayElementAtIndex(newScriptIndex);
                    serializedObject.ApplyModifiedProperties();
                }
                scriptExpanded.GetArrayElementAtIndex(newScriptIndex).boolValue = false;
                serializedObject.Update();
            }
        }
    }
      private void MoveScript(SerializedProperty serializedCategories, int index, int newIndex)
    { 
        var scripts = serializedCategories.FindPropertyRelative("scripts");

        if (newIndex < 0 || newIndex >= scripts.arraySize) return;

        Undo.RecordObject(targetCategory, "Swap Script");
        var temp = scripts.GetArrayElementAtIndex(index).objectReferenceValue;
        scripts.GetArrayElementAtIndex(index).objectReferenceValue = scripts.GetArrayElementAtIndex(newIndex).objectReferenceValue;
        scripts.GetArrayElementAtIndex(newIndex).objectReferenceValue = temp;

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region UI Methods
    private void ShowCategoryNameDialog(string windowTitle, string buttonLabel, System.Action<string> onCategoryNameSubmitted)
    {
        CategoryNameDialog window = ScriptableObject.CreateInstance<CategoryNameDialog>();
        window.titleContent = new GUIContent(windowTitle);
        window.ShowUtility();
        window.OnCategoryNameSubmitted = (categoryName) =>
        {
            if (string.IsNullOrEmpty(categoryName)) return;
            onCategoryNameSubmitted?.Invoke(categoryName);
        };
    }

    private void ShowCategoryEditDialog(SerializedProperty categoryProperty, List<InspectorCategory.Category> categories, InspectorCategory targetCategory)
    {
        CategoryEditDialog window = ScriptableObject.CreateInstance<CategoryEditDialog>();
        window.titleContent = new GUIContent("카테고리 편집");
        window.Init(categoryProperty, categories, targetCategory);
        window.ShowUtility(); 
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); 
        serializedCategories = serializedObject.FindProperty("categories");

        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUIStyle editExpandedStyle = new GUIStyle(EditorStyles.foldout);
        editExpandedStyle.alignment = TextAnchor.LowerLeft;
        string arrowIcon = targetCategory.editExpanded ? "▼" : "●";
        targetCategory.editExpanded = GUILayout.Toggle(targetCategory.editExpanded, arrowIcon+ "   카데고리 관리   " + arrowIcon, 
            EditorStyles.boldLabel, GUILayout.Width(140), GUILayout.Height(15));

        if(targetCategory.editExpanded)
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUIStyle centerStyle = new GUIStyle(EditorStyles.textField);
            centerStyle.alignment = TextAnchor.MiddleCenter;

            inputText = EditorGUILayout.TextField(
            string.IsNullOrEmpty(inputText) ? CategoryNameConstants.PlaceholderText : inputText, centerStyle);

            FilteringCategory(inputText);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(CategoryNameConstants.AddButtonLabel))
            {
                ShowCategoryNameDialog("카테고리 추가", CategoryNameConstants.AddButtonLabel, (categoryName) =>
                {
                    AddCategory(categoryName);
                });
            }
            if (GUILayout.Button(CategoryNameConstants.RemoveButtonLabel))
            {
                ShowCategoryNameDialog("카테고리 삭제", CategoryNameConstants.RemoveButtonLabel, (categoryName) =>
                {
                    RemoveCategory(targetCategory.categories,categoryName);
                });
            } 
            EditorGUILayout.EndHorizontal(); 
        }
        else
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); 
        }
        GUILayout.EndVertical();

        for (int i = 0; i < targetCategory.categories.Count; i++)
        {
            if (!targetCategory.categories[i].isDisplay) continue; 
            var categoryProperty = serializedCategories.GetArrayElementAtIndex(i);
            DrawCategoryHierarchy(categoryProperty, 0,i, targetCategory.categories);  // 최상위 카테고리부터 시작
        } 
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCategoryHierarchy(SerializedProperty categoryProperty, int indentLevel, int index, List<InspectorCategory.Category> categories)
    { 
        using (new EditorGUI.IndentLevelScope(indentLevel))
        {
            var categoryName = categoryProperty.FindPropertyRelative("categoryName").stringValue;
            var categoryColor = categoryProperty.FindPropertyRelative("categoryColor");
            var scripts = categoryProperty.FindPropertyRelative("scripts");
            var isExpanded = categoryProperty.FindPropertyRelative("isExpanded");
            var preExpanded = categoryProperty.FindPropertyRelative("preExpanded");
            var isActive = categoryProperty.FindPropertyRelative("isActive");
            var scriptExpanded = categoryProperty.FindPropertyRelative("scriptExpanded");
            var subCategories = categoryProperty.FindPropertyRelative("subCategories");

            EditorGUILayout.Space();

            // 카데고리 관리 ( Foldout, 비/활성화, 스크립트 추가, 편집, 하위카데고리 추가, 카데고리 순서 변경 )
            using (new EditorGUILayout.HorizontalScope())
            {
                // 카테고리 이름에 색상 적용
                GUIStyle categoryStyle = new GUIStyle(EditorStyles.foldout)
                {
                    normal = { textColor = categoryColor.colorValue },
                    onNormal = { textColor = categoryColor.colorValue },
                    fontStyle = FontStyle.Bold
                };
                // 펼치기/접기 버튼 (접고 필때 하위 카데고리는 접혀있을수 있도록)
                isExpanded.boolValue = EditorGUILayout.Foldout(isExpanded.boolValue, categoryName, true, categoryStyle);
                if (isExpanded.boolValue)
                {
                    if (preExpanded.boolValue != isExpanded.boolValue)
                    {
                        preExpanded.boolValue = isExpanded.boolValue;

                        if (subCategories.arraySize > 0)
                        {
                            for (int i = 0; i < subCategories.arraySize; i++)
                            {
                                var subCategory = subCategories.GetArrayElementAtIndex(i);
                                var subIsExpanded = subCategory.FindPropertyRelative("isExpanded");
                                if (subIsExpanded != null) subIsExpanded.boolValue = false;
                            }
                        }
                    }
                    Rect fieldRect = EditorGUILayout.GetControlRect(GUILayout.Width(150));
                    Rect toggleRect = new Rect(fieldRect.x - 20 - (EditorGUI.indentLevel * 15), fieldRect.y, 30, fieldRect.height);

                    // 활성화 비활성화 (카테고리 상태 변경 시, 해당 카테고리 내 모든 스크립트의 활성화 상태도 동일하게 변경)
                    bool newIsActive = EditorGUI.Toggle(toggleRect, isActive.boolValue);
                    if (newIsActive != isActive.boolValue)
                    {
                        for (int j = 0; j < scripts.arraySize; j++)
                        {
                            var scriptProperty = scripts.GetArrayElementAtIndex(j);
                            var scriptObject = scriptProperty.objectReferenceValue as MonoBehaviour;
                            if (scriptObject != null) scriptObject.enabled = newIsActive;
                        }
                        isActive.boolValue = newIsActive;
                    }

                    // 스크립트 추가 입력란   
                    var newScript = EditorGUI.ObjectField(fieldRect, "", null, typeof(MonoBehaviour), true) as MonoBehaviour;
                    if (newScript == null)
                    {
                        GUIStyle placeholderStyle = new GUIStyle(GUI.skin.textField)
                        {
                            normal = { textColor = new Color(0.6f, 0.6f, 0.6f, 0.9f) },
                            alignment = TextAnchor.MiddleCenter
                        };
                        GUI.Label(fieldRect, "스크립트 추가", placeholderStyle);
                    }
                    else if (!ContainsScript(scripts, newScript))
                    {
                        AddScriptToCategory(scripts, newScript);
                        scriptExpanded.InsertArrayElementAtIndex(scriptExpanded.arraySize);
                        scriptExpanded.GetArrayElementAtIndex(scriptExpanded.arraySize - 1).boolValue = false;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUI.indentLevel * 15);
                    if (GUILayout.Button("카데고리 편집"))
                    {
                        ShowCategoryEditDialog(categoryProperty, categories, targetCategory);
                    }
                    if (GUILayout.Button("하위 카테고리 추가"))
                    {
                        ShowCategoryNameDialog("하위 카테고리 추가", "추가", (subCategoryName) =>
                        {
                            AddSubCategory(categories, categoryName, subCategoryName);
                        });
                    }
                }
                else
                {
                    preExpanded.boolValue = isExpanded.boolValue;
                    // 카테고리 위아래 이동 버튼
                    if (GUILayout.Button("↑", GUILayout.Width(30))) MoveCategory(categories, index, index - 1);
                    if (GUILayout.Button("↓", GUILayout.Width(30))) MoveCategory(categories, index, index + 1);
                }
            }

            // 카데고리에 속한 스크립트 표시
            if (isExpanded.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("box");
                for (int j = 0; j < scripts.arraySize; j++)
                {
                    var scriptProperty = scripts.GetArrayElementAtIndex(j);
                    var scriptObject = scriptProperty.objectReferenceValue as MonoBehaviour;

                    if (scriptObject != null)
                    {
                        if (!string.IsNullOrEmpty(previousInputText) &&
                            scriptObject.GetType().Name.IndexOf(previousInputText, StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            continue;
                        }

                        if (scriptObject != null && !scriptEditors.ContainsKey(scriptObject))
                        {
                            scriptEditors[scriptObject] = Editor.CreateEditor(scriptObject);
                        }

                        scriptObject.hideFlags = HideFlags.HideInInspector;
                        EditorGUILayout.BeginVertical("box");

                        // 스크립트 관리 ( Foldout, 비/활성화, 이동, 삭제, 순서변경 )
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (scriptExpanded.GetArrayElementAtIndex(j).boolValue = EditorGUILayout.Foldout(scriptExpanded.GetArrayElementAtIndex(j).boolValue, scriptObject.GetType().Name, true))
                            {
                                // 스크립트 활성화 체크박스
                                scriptObject.enabled = EditorGUILayout.Toggle(scriptObject.enabled, GUILayout.Width(18));  // 체크박스

                                // 스크립트 이동 버튼
                                if (GUILayout.Button("이동", GUILayout.Width(40)))
                                {
                                    ShowCategoryNameDialog("스크립트 이동", CategoryNameConstants.MoveButtonLabel, (newCategoryName) =>
                                    {
                                        if (!CategoryExists(newCategoryName)) return;

                                        MoveScriptToNewCategory(scriptObject, categoryName, newCategoryName);
                                    });
                                }
                                // 스크립트 삭제 버튼
                                if (GUILayout.Button("삭제", GUILayout.Width(40)))
                                {
                                    RemoveScriptFromCategory(categoryName, scriptObject);
                                    scriptObject.hideFlags = HideFlags.None;
                                    EditorUtility.SetDirty(scriptObject);
                                    continue;
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("↑", GUILayout.Width(20))) MoveScript(categoryProperty, j, j - 1);
                                if (GUILayout.Button("↓", GUILayout.Width(20))) MoveScript(categoryProperty, j, j + 1);
                            }
                        }

                        EditorGUI.indentLevel++;
                        if (scriptExpanded.GetArrayElementAtIndex(j).boolValue)
                        {
                            SerializedObject scriptSerializedObject = scriptEditors[scriptObject].serializedObject;
                            scriptSerializedObject.Update();

                            // 인스펙터 그리기
                            using (new EditorGUILayout.VerticalScope("box"))
                            {
                                Editor customEditor = Editor.CreateEditor(scriptObject);
                                if (customEditor != null)
                                {
                                    customEditor.OnInspectorGUI();
                                }
                                else
                                {
                                    DrawDefaultInspector();
                                }
                            }
                            scriptSerializedObject.ApplyModifiedProperties();
                        }
                        EditorGUI.indentLevel--;
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndVertical();
            }

            // 하위 카테고리가 있는 경우 재귀적으로 하위 카테고리 표시
            if (isExpanded.boolValue)
            {
                if (subCategories.arraySize > 0)
                {
                    for (int i = 0; i < subCategories.arraySize; i++)
                    {
                        var subCategoryProperty = subCategories.GetArrayElementAtIndex(i);
                        if (!subCategoryProperty.FindPropertyRelative("isDisplay").boolValue) continue;

                        DrawCategoryHierarchy(subCategoryProperty, indentLevel + 1, i, categories[index].subCategories);
                    }
                }
            }
        }    
    } 
    #endregion
}
#endif