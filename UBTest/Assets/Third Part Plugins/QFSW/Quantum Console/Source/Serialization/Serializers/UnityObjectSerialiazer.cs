using UnityEngine;

namespace QFSW.QC.Serializers
{
    public class UnityObjectSerialiazer : PolymorphicQcSerializer<Object>
    {
        public override string SerializeFormatted(Object value, QuantumTheme theme)
        {
            return value.name;
        }
    }
}
