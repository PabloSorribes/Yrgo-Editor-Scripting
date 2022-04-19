using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
	[Header("Object Setup")]
	public GameObject backgroundPrefab;
	public Color color = Color.white;
	public Sprite[] sprites;

	[Header("Generator setting")]
	public float xRange = 8f;
	public float yRange = 12f;
	public int backgroundObjects = 3;
	public float maxScale = 3f;

	// Start is called before the first frame update
	void Start()
	{
		Generate();
	}

	public void Generate()
	{
		for (int i = 0; i < backgroundObjects; i++)
		{
			var newBG = Instantiate(backgroundPrefab, transform);

//#if UNITY_EDITOR
//			UnityEditor.Undo.RegisterCreatedObjectUndo(newBG, "Created obj");
//#endif

			var spriteRenderer = newBG.GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
			spriteRenderer.color = color;
			RandomFlip(spriteRenderer);

			newBG.transform.position = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), 0);
			newBG.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
			float scale = Random.Range(1f, maxScale);
			newBG.transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	void RandomFlip(SpriteRenderer spriteRenderer)
	{
		//Add variation to the art.
		float r = Random.Range(0, 1f);
		if (r > 0.5f)
			spriteRenderer.flipX = true;
		if (r < 0.75f && r > 0.25f)
			spriteRenderer.flipY = true;
	}

	public void Clear()
	{
		foreach (Transform child in transform)
		{
			DestroyImmediate(child.gameObject);

			//#if UNITY_EDITOR
			//			UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
			//#else
			//			DestroyImmediate(child.gameObject);
			//#endif
		}
		if (transform.childCount > 0)
		{
			Clear();
		}
	}
}
