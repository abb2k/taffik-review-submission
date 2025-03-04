using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    Card card { get { return target as Card; } }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        if (card.icon != null)
        {
            Type type = GetType("UnityEditor.SpriteUtility");
            if (type != null)
            {
                MethodInfo method = type.GetMethod("RenderStaticPreview", new Type[4]
                {
                    typeof(Sprite),
                    typeof(Color),
                    typeof(int),
                    typeof(int)
                });
                if (method != null)
                {
                    CardGlobals.getColorsBasedOnDifficulty(card.difficulty, out Color fillColor, out Color textColor);

                    object obj = method.Invoke("RenderStaticPreview", new object[4]
                    {
                        card.icon,
                        fillColor,
                        width,
                        height
                    });
                    if (obj is Texture2D)
                    {
                        return obj as Texture2D;
                    }
                }
            }
        }

        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    private static Type GetType(string typeName)
    {
        Type type = Type.GetType(typeName);
        if (type != null)
        {
            return type;
        }

        AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        for (int i = 0; i < referencedAssemblies.Length; i++)
        {
            Assembly assembly = Assembly.Load(referencedAssemblies[i]);
            if (assembly != null)
            {
                type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }
        }

        return null;
    }
}


