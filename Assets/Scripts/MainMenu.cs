
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "GameScene";

    private string saveLocation;

    private void Awake()
    {
        saveLocation = Path.Combine(
            Application.persistentDataPath,
            "saveData.json"
        );
    }

    // Connect this to the Play Game button
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Connect this to the Delete Save button
    public void DeleteSave()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
            Debug.Log("Save file deleted!");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }
}
