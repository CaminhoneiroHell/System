using System;
using System.Linq;
using UnityEngine;

namespace QFSW.QC.Extras
{
    public static class UtilCommands
    {
        [Command("get-object-info", "Finds the specified GameObject and displays its transform and component data")]
        private static string ExtractObjectInfo(GameObject target)
        {
            string info = $"Extracted info for object '{target.name}'";
            info += $"\nTransform data:";
            info += $"\n   - position: {target.transform.position}";
            info += $"\n   - rotation: {target.transform.localRotation}";
            info += $"\n   - scale: {target.transform.localScale}";
            if (target.transform.childCount > 0) { info += $"\n   - child count: {target.transform.childCount}"; }
            if (target.transform.parent) { info += $"\n   - parent: {target.transform.parent.name}"; }

            Component[] components = target.GetComponents<Component>().OrderBy(x => x.GetType().Name).ToArray();

            if (components.Length > 0)
            {
                info += $"\nComponent data:";
                for (int i = 0; i < components.Length; i++)
                {
                    int componentCount = 1;
                    Type componentType = components[i].GetType();
                    info += $"\n   - {componentType.Name}";
                    while (i + 1 < components.Length && components[i + 1].GetType() == componentType)
                    {
                        componentCount++;
                        i++;
                    }

                    if (componentCount > 1) { info += $" ({componentCount})"; }
                }
            }

            if (target.transform.childCount > 0)
            {
                info += $"\nChildren:";

                int childCount = target.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    info += $"\n   - {target.transform.GetChild(i).name}";
                }
            }

            return info;
        }

        [Command("add-component", "Adds a component of type T to the specified GameObject")]
        private static void AddComponent<T>(GameObject target) where T : Component { target.AddComponent<T>(); }

        [Command("destroy-component", "Destroys the component of type T on the specified GameObject")]
        private static void DestroyComponent<T>(T target) where T : Component { GameObject.Destroy(target); }

        [Command("destroy", "Destroys a GameObject")]
        private static void DestroyGO(GameObject target) { GameObject.Destroy(target); }

        [Command("instantiate", "Instantiates a GameObject")]
        private static void InstantiateGO([CommandParameterDescription("The original GameObject to instantiate a copy of.")]GameObject original,
                                          [CommandParameterDescription("The position of the instantiated GameObject.")]Vector3 position,
                                          [CommandParameterDescription("The rotation of the instantiated GameObject.")]Quaternion rotation)
        { GameObject.Instantiate(original, position, rotation); }

        [Command("instantiate", "Instantiates a GameObject")]
        private static void InstantiateGO(GameObject original, Vector3 position) { GameObject.Instantiate(original).transform.position = position; }

        [Command("instantiate", "Instantiates a GameObject")]
        private static void InstantiateGO(GameObject original) { GameObject.Instantiate(original); }

        [Command("teleport", "Teleports a GameObject")]
        private static void TeleportGO(GameObject target, Vector3 position) { target.transform.position = position; }

        [Command("teleport-relative", "Teleports a GameObject by a relative offset to its current position")]
        private static void TeleportRelativeGO(GameObject target, Vector3 offset) { target.transform.Translate(offset); }

        [Command("rotate", "Rotates a GameObject")]
        private static void RotateGO(GameObject target, Quaternion rotation) { target.transform.Rotate(rotation.eulerAngles); }

        [Command("set-active", "Activates/deactivates a GameObject")]
        private static void SetGOActive(GameObject target, bool active) { target.SetActive(active); }

        [Command("set-parent", "Sets the parent of the targert transform.")]
        private static void SetGOParent(Transform target, Transform parentTarget) { target.SetParent(parentTarget); }

        [Command("send-message", "Calls the method named 'methodName' on every MonoBehaviour in the target GameObject")]
        private static void SendGOMessage(GameObject target, string methodName) { target.SendMessage(methodName); }
    }
}
