#if UNITY_EDITOR
namespace LucasBaran.Bootstrap
{
    using UnityEditor;

    public static class BootstrapEditorPrefs
    {
        private const string SCENARIO_TO_LOAD_KEY = "Bootstrap/ScenatioToLoad";
        private const string DISABLE_BOOTSTRAP_PLAYMODE = "Bootstrap/DisablePlayMode";

        public static bool EnableBootstrapAtPlayMode
        {
            get => EditorPrefs.HasKey(DISABLE_BOOTSTRAP_PLAYMODE);
            set
            {
                if (value)
                {
                    EditorPrefs.SetBool(DISABLE_BOOTSTRAP_PLAYMODE, true);
                }
                else
                {
                    EditorPrefs.DeleteKey(DISABLE_BOOTSTRAP_PLAYMODE);
                }
            }
        }

        public static void SetScenarioToLoad(Scenario scenario)
        {
            GUID guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(scenario));
            EditorPrefs.SetString(SCENARIO_TO_LOAD_KEY, guid.ToString());
        }

        public static bool TryGetScenarioToLoad(out Scenario scenario)
        {
            string guid_string = EditorPrefs.GetString(SCENARIO_TO_LOAD_KEY, string.Empty);

            if (string.IsNullOrEmpty(guid_string) || !GUID.TryParse(guid_string, out GUID guid))
            {
                scenario = null;
                return false;
            }

            scenario = AssetDatabase.LoadAssetByGUID<Scenario>(guid);
            return scenario != null;
        }

        public static void DeleteScenarioToLoad()
        {
            EditorPrefs.DeleteKey(SCENARIO_TO_LOAD_KEY);
        }
    }
}
#endif
