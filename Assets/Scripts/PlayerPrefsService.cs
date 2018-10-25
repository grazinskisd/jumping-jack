using UnityEngine;

namespace JumpingJack
{
    public class PlayerPrefsService: MonoBehaviour
    {
        public static void SetInt(Prefs pref, int value)
        {
            PlayerPrefs.SetInt(pref.ToString(), value);
        }

        public static int GetInt(Prefs pref)
        {
            return PlayerPrefs.GetInt(pref.ToString());
        }

        public static void IncrementPref(Prefs pref, int increment)
        {
            SetInt(pref, GetInt(pref) + increment);
        }

        public static void ResetToStart()
        {
            SetInt(Prefs.Level, 1);
            SetInt(Prefs.Hazards, 0);
            SetInt(Prefs.Score, 0);
            SetInt(Prefs.Lives, 0);
            SetInt(Prefs.GodMode, 0);
        }

        public static void SetLevelTo(int level)
        {
            SetInt(Prefs.Level, level);
            SetInt(Prefs.Hazards, level-1);
            SetInt(Prefs.Score, 0);
            SetInt(Prefs.Lives, 0);
        }
    }

    public enum Prefs
    {
        Level,
        Hazards,
        Score,
        Highscore,
        Lives,
        GodMode
    }
}
