using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject optionCanvas;

    [SerializeField] GameObject test;
    public void StartButton()
    {
        test.SetActive(true);

        Utils.CursorLock();
        SceneManager.LoadScene("Main",new Vector3(0,81.5f,0));
    }

    public void OptionsButton(bool flag)
    {
        if(flag)
        {
            InputManager.GetInstance.LoadKeyBindings();
        }
        else
        {
            InputManager.GetInstance.SaveKeyBindings();
        }
        titleCanvas.gameObject.SetActive(!flag);
        optionCanvas.gameObject.SetActive(flag);
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    private void Start()
    {
        test.SetActive(false);
        Utils.CursorUnLock();
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
