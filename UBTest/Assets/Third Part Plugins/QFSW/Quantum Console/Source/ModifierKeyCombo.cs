using UnityEngine;
using UnityEngine.Serialization;

namespace QFSW.QC
{
    [System.Serializable]
    public struct ModifierKeyCombo
    {
        [FormerlySerializedAs("key")]
        public KeyCode Key;
        [FormerlySerializedAs("ctrl")]
        public bool Ctrl;
        [FormerlySerializedAs("alt")]
        public bool Alt;
        [FormerlySerializedAs("shift")]
        public bool Shift;

        public bool IsPressed()
        {
            bool ctrlDown = !Ctrl || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
            bool altDown = !Alt || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            bool shiftDown = !Shift || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            return ctrlDown && altDown && shiftDown && Input.GetKeyDown(Key);
        }

        public static implicit operator ModifierKeyCombo(KeyCode key)
        {
            return new ModifierKeyCombo() { Key = key };
        }
    }
}
