using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    public void ChangeToScene(string sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
        Application.LoadLevel(sceneToChangeTo);
}
