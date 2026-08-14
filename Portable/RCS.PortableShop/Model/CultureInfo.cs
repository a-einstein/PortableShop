using System.Diagnostics;

namespace RCS.PortableShop.Model
{
    [DebuggerDisplay("{Name} - {DisplayName}")]
    public readonly struct CultureInfo
    {
        public CultureInfo(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public string Name { get; }

        public string DisplayName { get; }
    }
}