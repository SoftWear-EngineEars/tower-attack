// HealthSystem.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject healthBarPrefab; // Slider goes here
    [SerializeField] private Vector3 offset = new(0, 1.2f, 0); // manual offset for health bar position in prefab

    [Header("Damage Animation Settings")]
    [SerializeField] private float flashDuration = 0.5f; // Duration of the red flash

    private float _maxHealth;
    private float _currentHealth;
    private Slider _healthBarSlider;
    private Transform _canvasTransform;
    private SpriteRenderer _spriteRenderer; // Reference to the SpriteRenderer

    public event Action OnDeath;

    private void Start()
    {
        // Find the main canvas to parent the health bar to
        _canvasTransform = FindAnyObjectByType<Canvas>().transform;
        var healthBarInstance = Instantiate(healthBarPrefab, _canvasTransform);
        _healthBarSlider = healthBarInstance.GetComponent<Slider>();
        UpdateHealthBarValue();
        UpdateHealthBarPosition();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            PlayDeathAnimation(); // Create a new object for the animation
            OnDeath?.Invoke(); // Trigger the death event
        }
        else if (_spriteRenderer != null)
        {
            StartCoroutine(FlashRed()); // Trigger the fade effect
        }
    }

    private void PlayDeathAnimation()
    {
        if (_spriteRenderer == null)
            return;

        // Create a new GameObject for the death animation
        GameObject deathAnimationObject = new GameObject("DeathAnimation");
        var deathAnimation = deathAnimationObject.AddComponent<DeathAnimation>();
        deathAnimation.Initialize(
            _spriteRenderer.sprite,
            transform.position,
            transform.localScale,
            _spriteRenderer.sortingLayerID,
            _spriteRenderer.sortingOrder,
            flashDuration
        );
    }

    private IEnumerator FlashRed()
    {
        if (_spriteRenderer == null)
            yield break;

        var originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red; // Turn red instantly

        float fadeDuration = flashDuration; // Duration of the fade
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            _spriteRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = originalColor; // Ensure it ends at the original color
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
