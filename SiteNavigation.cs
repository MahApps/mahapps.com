using System;
using System.Linq;

namespace MahApps.Docs
{
    /// <summary>
    /// The grouping behind the top navigation and the left sidebar.
    /// </summary>
    /// <remarks>
    /// The page tree cannot express it: Dialogs and Helper Classes are siblings
    /// of Controls, not children. Both _RightNavigation.cshtml and
    /// _Sidebar.cshtml read it from here so the two never drift apart.
    /// </remarks>
    public static class SiteNavigation
    {
        /// <summary>
        /// Each group, keyed by the section that heads it. The first entry of a
        /// group is also its top level navigation entry.
        /// </summary>
        public static readonly string[][] Groups =
        {
            new[] { "docs/controls", "docs/dialogs", "docs/helper" },
            new[] { "docs/styles", "docs/themes", "docs/stylevariants" },
            new[] { "docs/guides" }
        };

        /// <summary>
        /// The group the given output path belongs to, or <c>null</c> when the
        /// page sits outside the grouped sections (the home page, About, the
        /// API reference).
        /// </summary>
        /// <param name="destination">
        /// The rendered page's output path, with or without a leading slash,
        /// for example <c>docs/controls/splitview.html</c>.
        /// </param>
        public static string[]? GroupOf(string? destination)
        {
            if (string.IsNullOrEmpty(destination))
            {
                return null;
            }

            string path = destination.TrimStart('/');
            return Groups.FirstOrDefault(group => group.Any(section => IsInSection(path, section)));
        }

        /// <summary>
        /// Whether the given output path is the index of <paramref name="section"/>
        /// or one of the pages below it.
        /// </summary>
        public static bool IsInSection(string? destination, string section)
        {
            if (string.IsNullOrEmpty(destination))
            {
                return false;
            }

            string path = destination.TrimStart('/');
            return path.Equals(section + "/index.html", StringComparison.OrdinalIgnoreCase)
                || path.Equals(section + ".html", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith(section + "/", StringComparison.OrdinalIgnoreCase);
        }
    }
}
