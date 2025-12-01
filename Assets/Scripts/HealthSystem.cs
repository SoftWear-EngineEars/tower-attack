// HealthSystem.cs
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject healthBarPrefab; // Slider goes here
    [SerializeField] private Vector3 offset = new(0, 1.2f, 0); // manual offset for health bar position in prefab

    private float _maxHealth;
    private float _currentHealth;
    private Slider _healthBarSlider;
    private Transform _canvasTransform;

    public event Action OnDeath;

    private void Start()
    {
        // Find the main canvas to parent the health bar to
        _canvasTransform = FindAnyObjectByType<Canvas>().transform;
        var healthBarInstance = Instantiate(healthBarPrefab, _canvasTransform);
        _healthBarSlider = healthBarInstance.GetComponent<Slider>();
        UpdateHealthBarValue();
        UpdateHealthBarPosition();
    }

    private void LateUpdate() // Tech
    {
        UpdateHealthBarPosition();
    }

    private void OnDestroy()
    {
        // Clean up the health bar when the object is destroyed
        if (_healthBarSlider != null)
        {
            Destroy(_healthBarSlider.gameObject);
        }
    }

    public void Initialize(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        UpdateHealthBarValue();

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    private void UpdateHealthBarPosition()
    {
        if (_healthBarSlider == null)
            return;
        var screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
        _healthBarSlider.transform.position = screenPos;
        _healthBarSlider.gameObject.SetActive(screenPos.z > 0);
    }

    private void UpdateHealthBarValue()
    {
        if (_healthBarSlider == null)
            return;
        _healthBarSlider.value = _currentHealth / _maxHealth;
    }
}
