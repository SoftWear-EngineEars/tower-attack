using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float fadeDuration = 0.5f;

    public void Initialize(Sprite sprite, Vector3 position, Vector3 scale, int sortingLayerID, int sortingOrder, float fadeDuration = 0.5f)
    {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
        transform.position = position;
        transform.localScale = scale;
        _spriteRenderer.sortingLayerID = sortingLayerID;
        _spriteRenderer.sortingOrder = sortingOrder;
        this.fadeDuration = fadeDuration;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        _spriteRenderer.color = Color.red; // Start with red

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            _spriteRenderer.color = Color.Lerp(Color.red, Color.black, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = Color.black; // Ensure it ends at black
        Destroy(gameObject); // Destroy the animation object
    }
}