using UnityEngine;

/// <summary>
/// Add to an object with a BoxCollider which you want to visualize better in the inspector.
/// </summary>
public class DrawColliderGizmo_Basic : MonoBehaviour
{
	[Range(0f, 1f)]
	public float alpha = 0.3f;
	public Color defaultGizmoColor = Color.cyan;
	public Color selectedGizmoColor = Color.yellow;

	private BoxCollider boxCollider = null;

	private void DrawBoxColliderGizmo(Color gizmoBoxColor)
	{
		//Get a reference to the Box Collider, which we'll use as a base for drawing the gizmo-box.
		if (boxCollider == null)
			boxCollider = GetComponent<BoxCollider>();

		//Save the color in a temporary variable to not overwrite changes in the inspector.
		var color = gizmoBoxColor;

		//Draws the edges of the BoxCollider 
		Gizmos.color = color;
		Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size);

		//Draws the sides/insides of the BoxCollider, with a tint to the original color.
		color.a = alpha;
		Gizmos.color = color;
		Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
	}


	//OnDrawGizmos() is a magic function that is called by Unity when the Editors SceneView is rendered.
	//This way we can draw Debug Data right into the SceneView. If you press the "Gizmos" button in the
	//GameView these Gizmos are also drawn into the GameView so you can see your visual debug objects while
	//playing the game
	private void OnDrawGizmos()
	{
		DrawBoxColliderGizmo(defaultGizmoColor);
	}

	/// <summary>
	/// Same as OnDrawGizmos, but when the object is selected. 
	/// Examples: Change color of the gizmo, perform more rendering-heavy debug gizmos only when the object is selected, etc.
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		DrawBoxColliderGizmo(selectedGizmoColor);
	}
}
