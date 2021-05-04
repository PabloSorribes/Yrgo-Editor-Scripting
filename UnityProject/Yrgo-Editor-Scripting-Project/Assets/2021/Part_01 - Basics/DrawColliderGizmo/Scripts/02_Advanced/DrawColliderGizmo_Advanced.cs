// Code written by Pablo Sorribes Bernhard and Victor Engström (Sonigon AB)
// Copyright 2021 (MIT License)

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR // || UNITY_DEVELOPMENT

namespace Sonigon
{
	public static class DrawColliderGizmo_Advanced_Toggle
	{
		// If you wanna disable all of them globally, just change this to false
		public static bool isEnabled = true;
	}

	/// <summary>
	/// Used to draw help debug mesh for (audio) trigger placement
	/// Supports BoxCollider, SphereCollider, CapsuleCollider
	/// Capsule components are a bit bugged, doesnt follow offsets etc
	/// </summary>
	[ExecuteInEditMode]
	public class DrawColliderGizmo_Advanced : MonoBehaviour
	{
		public bool showDebugMesh = true;

		public Color color = Color.cyan;

		[Range(0, 1)]
		public float alpha = 0.3f;

		public bool showInGame = false;

		[Header("Assign \"SonigonDebugMeshMaterial\" material here if you want to show in game")]
		public Material inGameMaterial = null;

		[HideInInspector]
		public Collider cachedCollider = null;

		private void OnEnable()
		{
			if (!DrawColliderGizmo_Advanced_Toggle.isEnabled)
			{
				return;
			}
			if (cachedCollider == null)
			{
				cachedCollider = GetComponent<Collider>();
			}

			// Spawn a mesh with the size of the trigger if "showInGame" is true
			if (showInGame && Application.isPlaying && cachedCollider != null)
			{
				var meshFilter = gameObject.AddComponent<MeshFilter>();

				if (cachedCollider is BoxCollider)
					meshFilter.mesh = SonigonPrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Cube);

				if (cachedCollider is SphereCollider)
					meshFilter.mesh = SonigonPrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Sphere);

				if (cachedCollider is CapsuleCollider)
					meshFilter.mesh = SonigonPrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Capsule);

				var meshRenderer = gameObject.AddComponent<MeshRenderer>();

				// Added nullcheck
				if (inGameMaterial != null)
				{
					Material material = new Material(inGameMaterial);
					Color debugColorTemp = new Color(color.r, color.g, color.b, alpha);

					material.color = debugColorTemp;
					meshRenderer.material = material;
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (!DrawColliderGizmo_Advanced_Toggle.isEnabled)
			{
				return;
			}
			if (showDebugMesh && cachedCollider != null)
			{
				Color debugColorTemp = new Color(color.r, color.g, color.b, alpha);
				Gizmos.color = debugColorTemp;
				Gizmos.matrix = transform.localToWorldMatrix;

				if (cachedCollider is BoxCollider boxCollider)
				{
					Gizmos.DrawCube(boxCollider.center, boxCollider.size);
					Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
				}

				if (cachedCollider is SphereCollider sphereCollider)
				{
					Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
					Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
				}

				if (cachedCollider is CapsuleCollider capsuleCollider)
				{
					Mesh debugMesh = SonigonPrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Capsule);
					Gizmos.DrawMesh(debugMesh);
					Gizmos.DrawWireMesh(debugMesh);
				}
			}
		}
	}

	// Source: http://answers.unity.com/answers/1120060/view.html
	public static class SonigonPrimitiveHelper
	{
		private static Dictionary<PrimitiveType, Mesh> primitiveMeshes = new Dictionary<PrimitiveType, Mesh>();

		public static GameObject CreatePrimitive(PrimitiveType type, bool withCollider)
		{
			if (withCollider) { return GameObject.CreatePrimitive(type); }

			GameObject gameObject = new GameObject(type.ToString());
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = SonigonPrimitiveHelper.GetPrimitiveMesh(type);
			gameObject.AddComponent<MeshRenderer>();

			return gameObject;
		}

		public static Mesh GetPrimitiveMesh(PrimitiveType type)
		{
			if (!primitiveMeshes.ContainsKey(type))
			{
				CreatePrimitiveMesh(type);
			}

			return primitiveMeshes[type];
		}

		private static Mesh CreatePrimitiveMesh(PrimitiveType type)
		{
			GameObject gameObject = GameObject.CreatePrimitive(type);
			Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

#if UNITY_EDITOR
			GameObject.DestroyImmediate(gameObject);
#else
			GameObject.Destroy(gameObject);
#endif

			primitiveMeshes[type] = mesh;
			return mesh;
		}
	}
}
#endif
