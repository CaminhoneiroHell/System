using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QFSW.QC.Utilities
{
    public static class GameObjectExtensions
    {
        private static List<GameObject> _rootGameObjectBuffer = new List<GameObject>();
        public static GameObject Find(string name, bool includeInactive = false)
        {
            GameObject obj = GameObject.Find(name);
            if (obj)
            {
                return obj;
            }

            if (includeInactive)
            {
                int sceneCount = SceneManager.sceneCountInBuildSettings;
                for (int i = 0; i < sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneByBuildIndex(i);
                    if (scene.isLoaded)
                    {
                        _rootGameObjectBuffer.Clear();
                        scene.GetRootGameObjects(_rootGameObjectBuffer);

                        foreach (GameObject root in _rootGameObjectBuffer)
                        {
                            obj = Find(name, root);
                            if (obj)
                            {
                                return obj;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static GameObject Find(string name, GameObject root)
        {
            if (root.name == name)
            {
                return root;
            }

            for (int i = 0; i < root.transform.childCount; i++)
            {
                GameObject obj = Find(name, root.transform.GetChild(i).gameObject);
                if (obj)
                {
                    return obj;
                }
            }

            return null;
        }
    }
}