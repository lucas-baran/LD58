using System.Collections.Generic;
using UnityEditor.Toolbars;

namespace LucasBaran.Bootstrap.Toolbar
{
    internal static class BootstrapToolbar
    {
        private const string ELEMENTS_PATH = "Bootstrap";

        [MainToolbarElement(ELEMENTS_PATH, defaultDockPosition = MainToolbarDockPosition.Middle)]
        private static IEnumerable<MainToolbarElement> GetToolbarElements()
        {
            yield return PlayButtonFactory.Create();
            yield return OpenButtonFactory.Create();
        }
    }
}
