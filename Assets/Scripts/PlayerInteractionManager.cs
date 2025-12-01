using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteractionManager : MonoBehaviour
{
    private Tier? _selectedMonsterTier;
    private MonsterData _selectedMonsterData;

    private void Update()
    {
        // Ensure a mouse is connected
        if (Mouse.current == null)
        {
            Debug.Log("Mouse missing!");
            return;
        }

        // resetting the selected data if outside of the MainScene
        if (!SceneManager.GetActiveScene().name.Equals("MainScene") && _selectedMonsterData != null)
        {
            _selectedMonsterTier = null;
            _selectedMonsterData = null;
        }

        // only runs if in the MainScene, there is a monster selected, and the player clicked
        if (SceneManager.GetActiveScene().name.Equals("MainScene") && _selectedMonsterTier.HasValue && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (ResourceManager.Instance.SpendGold(_selectedMonsterData.cost))
            {
                Vector2 screenPosition = Mouse.current.position.ReadValue();
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                spawnPosition.z = 0;

                EntityManager.Instance.SpawnMonster(_selectedMonsterTier.Value, spawnPosition);
            }
            else
            {
                Debug.Log("Too broke");
                // Maybe we should add a sound effect to the game that says the above
            }
        }
    }

    // This method will be called by the UI toggle buttons
    public void OnMonsterToggleSelected(MonsterToggleButton button)
    {
        if (button.IsSelected())
        {
            _selectedMonsterTier = button.monsterTier;
            _selectedMonsterData = Resources.Load<MonsterData>($"ScriptableObjects/Monster{EntityManager.TierToInt(_selectedMonsterTier.Value)}");
        }
        else
        {
            _selectedMonsterTier = null;
            _selectedMonsterData = null;
        }
    }
}