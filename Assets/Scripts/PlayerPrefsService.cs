using UnityEngine;

namespace JumpingJack
{
    public class PlayerPrefsService: MonoBehaviour
    {
        public int MaxLevel;
        public int[] ExtraLivesLevels;
        public int StartLifeCount;

        private static bool _isCreated = false;

        public void Awake()
        {
            if (!_isCreated)
            {
                DontDestroyOnLoad(gameObject);
                _isCreated = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool ShouldAwardExtraLife()
        {
            int level = GetInt(Prefs.Level);
            for (int i = 0; i < ExtraLivesLevels.Length; i++)
            {
                if(level == ExtraLivesLevels[i])
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsLastLevel()
        {
            return GetInt(Prefs.Level) == MaxLevel;
        }

        public bool WasLastLevel()
        {
            return GetInt(Prefs.Level) > MaxLevel;
        }

        public void SetInt(Prefs pref, int value)
        {
            PlayerPrefs.SetInt(pref.ToString(), value);
        }

        public int GetInt(Prefs pref)
        {
            return PlayerPrefs.GetInt(pref.ToString());
        }

        public void Increment(Prefs pref, int increment)
        {
            SetInt(pref, GetInt(pref) + increment);
        }

        public void ResetToStart()
        {
            SetInt(Prefs.Level, 0);
            SetInt(Prefs.Hazards, 0);
            SetInt(Prefs.Score, 0);
            SetInt(Prefs.Lives, StartLifeCount);
            SetInt(Prefs.GodMode, 0);
        }

        public void SetLevelTo(int level)
        {
            SetInt(Prefs.Level, level);
            SetInt(Prefs.Hazards, level);
            SetInt(Prefs.Score, 0);
            SetInt(Prefs.Lives, StartLifeCount);
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
