using System.Diagnostics;

namespace RCS.PortableShop.Common.Styles.Themes
{
    [DebuggerDisplay("{Theme} - {DisplayName}")]
    public readonly struct ThemeInfo
    {
        public ThemeInfo(Theme theme, string displayName)
        {
            Theme = theme;
            DisplayName = displayName;
        }

        public Theme Theme { get; }

        public string DisplayName { get; }
    }
}