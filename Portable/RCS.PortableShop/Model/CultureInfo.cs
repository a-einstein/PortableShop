namespace RCS.PortableShop.Model
{
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