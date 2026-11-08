using UnityEditor;
using UnityEngine;

public class MenuProxy : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;

		PointTracker.ResetMult();
		PointTracker.ResetPoints();
		ReputationTracker.ResetReputation();
	}

    public void LoadScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #endif
    }
}
