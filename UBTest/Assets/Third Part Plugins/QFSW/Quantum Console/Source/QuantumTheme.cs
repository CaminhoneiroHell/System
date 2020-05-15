using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace QFSW.QC
{
    [CreateAssetMenu(fileName = "Untitled Theme", menuName = "Quantum Console/Theme")]
    public class QuantumTheme : ScriptableObject
    {
        [FormerlySerializedAs("fontTMP")]
        [SerializeField] public TMP_FontAsset Font = null;
        [FormerlySerializedAs("panelMaterial")]
        [SerializeField] public Material PanelMaterial = null;
        [FormerlySerializedAs("panelColor")]
        [SerializeField] public Color PanelColor = Color.white;

        [FormerlySerializedAs("commandLogColor")]
        [SerializeField] public Color CommandLogColor = new Color(0, 1, 1);
        [FormerlySerializedAs("selectedSuggestionColor")]
        [SerializeField] public Color SelectedSuggestionColor = new Color(1, 1, 0.55f);
        [FormerlySerializedAs("suggestionColor")]
        [SerializeField] public Color SuggestionColor = Color.gray;
        [FormerlySerializedAs("errorColor")]
        [SerializeField] public Color ErrorColor = Color.red;
        [FormerlySerializedAs("warningColor")]
        [SerializeField] public Color WarningColor = new Color(1, 0.5f, 0);
        [FormerlySerializedAs("successColor")]
        [SerializeField] public Color SuccessColor = Color.green;

        [FormerlySerializedAs("defaultReturnValueColor")]
        [SerializeField] public Color DefaultReturnValueColor = Color.white;
        [FormerlySerializedAs("typeFormatters")]
        [SerializeField] public List<TypeColorFormatter> TypeFormatters = new List<TypeColorFormatter>(0);
        [FormerlySerializedAs("collectionFormatters")]
        [SerializeField] public List<CollectionFormatter> CollectionFormatters = new List<CollectionFormatter>(0);

        private T FindTypeFormatter<T>(List<T> formatters, Type type) where T : TypeFormatter
        {
            foreach (T formatter in formatters)
            {
                if (type == formatter.Type || type.IsGenericTypeOf(formatter.Type))
                {
                    return formatter;
                }
            }

            foreach (T formatter in formatters)
            {
                if (formatter.Type.IsAssignableFrom(type))
                {
                    return formatter;
                }
            }

            return null;
        }

        public string ColorizeReturn(string data, Type type)
        {
            TypeColorFormatter formatter = FindTypeFormatter(TypeFormatters, type);
            if (formatter == null) { return data.ColorText(DefaultReturnValueColor); }
            else { return data.ColorText(formatter.Color); }
        }

        public void GetCollectionFormatting(Type type, out string leftScoper, out string seperator, out string rightScoper)
        {
            CollectionFormatter formatter = FindTypeFormatter(CollectionFormatters, type);
            if (formatter == null)
            {
                leftScoper = "[";
                seperator = ",";
                rightScoper = "]";
            }
            else
            {
                leftScoper = formatter.LeftScoper.Replace("\\n", "\n");
                seperator = formatter.SeperatorString.Replace("\\n", "\n");
                rightScoper = formatter.RightScoper.Replace("\\n", "\n");
            }
        }
    }
}
