using System.ComponentModel;

namespace EAgendaMedica.Dominio.ModuloMedico
{
    public static class PrefixoEnumExtensionsBase
    {
        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                return attribute.Description;

            return "Anotação não informada";
        }
    }
}