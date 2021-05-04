using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Paalo.UnityMiscTools.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Nicer syntax for checking if a game object has component.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null;
        }

        ///<summary>
        /// Adds component if no component of that type is present, otherwise returns the first found component of type.
        ///</summary>
        public static T AddComponentIfNotPresent<T>(this GameObject go, bool addUndo = false) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component == null)
            {
#if UNITY_EDITOR
                if (addUndo)
                {
                    component = UnityEditor.Undo.AddComponent<T>(go);
                }
                else
                {
                    component = go.AddComponent<T>();
                }
#else
                component = go.AddComponent<T>();
#endif
            }

            return component;
        }

        ///<summary>
        /// Works like <see cref="GameObject.GetComponentsInChildren{T}"/>, but does not include the components present on the GameObject which this function is called upon.
        ///</summary>
        public static T[] GetComponentsInChildrenExcludeParent<T>(this GameObject go) where T : Component
        {
            List<T> components = new List<T>();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                var childComponents = go.transform.GetChild(i).GetComponentsInChildren<T>();
                for (int j = 0; j < childComponents.Length; j++)
                {
                    components.Add(childComponents[j]);
                }
            }
            return components.ToArray();
        }

        /// <summary>
        /// Checks if an object has a scene. If not, it is most probably a prefab asset (ie. the selected object is in the Project-/Assets-tab).
        /// Source: https://answers.unity.com/questions/710968/how-to-tell-if-a-gameobject-is-an-instance-or-a-pr.html
        /// </summary>
        public static bool IsSceneObject(this GameObject go)
        {
            return go.scene.rootCount == 0;
        }

        /// <summary>
        /// Shorthand for getting the PrefabAssetType by checking the object's PrefabType against all the available enum values of <see cref="UnityEditor.PrefabAssetType"/>.
        /// </summary>
        public static bool IsPrefab(this GameObject go)
        {
#if UNITY_EDITOR
            return PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab;
#else
            return false;
#endif
        }

        /// <summary>
        /// Returns the full hierarchy name of the game object.
        /// </summary>
        /// <param name="go">The game object.</param>
        public static string GetFullName(this GameObject go)
        {
            string name = go.name;
            while (go.transform.parent != null)
            {
                go = go.transform.parent.gameObject;
                name = go.name + "/" + name;
            }
            return name;
        }

        /// <summary>
        /// Checks if a GameObject has been destroyed.
        /// </summary>
        /// <param name="gameObject">GameObject reference to check for destructedness</param>
        /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }

        public static bool IsDestroyed(this MonoBehaviour gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }
    }
}
