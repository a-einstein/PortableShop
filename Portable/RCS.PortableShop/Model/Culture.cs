namespace RCS.PortableShop.Model
{
    public readonly struct Culture
    {
        public Culture(string displayName, string name)
        {
            DisplayName = displayName;
            Name = name;
        }

        public string DisplayName { get; }

        public string Name { get; }
    }
}