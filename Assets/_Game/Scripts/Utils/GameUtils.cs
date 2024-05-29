using System;
using UnityEngine;

namespace Scripts.Utils
{
    public class GameUtils
    {
        public static RuntimePlatform Platform 
        { 
            get
            {
#if UNITY_EDITOR || UNITY_ANDROID
                return RuntimePlatform.Android;
#elif UNITY_IPHONE
                return RuntimePlatform.IPhonePlayer;
#endif
            }
        }
    
        public static bool IsActionAvailable(string action, int time, bool availableFirstTime = true)
        {
            if (!PlayerPrefs.HasKey(action + "_time")) // First time.
            {
                if (availableFirstTime == false)
                {
                    SetActionTime(action);
                }
                return availableFirstTime;
            }

            var delta = (int)(GetCurrentTime() - GetActionTime(action));
            return delta >= time;
        }

        public static int GetLastActionTimeDiff(string action)
        {
            if (!PlayerPrefs.HasKey(action + "_time"))
            {
                return 0;
            }
            
            var delta = (int)(GetCurrentTime() - GetActionTime(action));
            return delta;
        }
    
        public static double GetCurrentTime()
        {
            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return span.TotalSeconds;
        }
        
        public static void SetActionTime(string action)
        {
            SetDouble(action + "_time", GetCurrentTime());
        }
        
        public static double GetActionTime(string action)
        {
            return GetDouble(action + "_time");
        }
    
        public static int GetCurrentSessionCount()
        {
            return PlayerPrefs.GetInt("session_count", 0);
        }
        
        public static void IncrementSessionCount()
        {
            var currentCount = GetCurrentSessionCount();
            PlayerPrefs.SetInt("session_count", ++currentCount);
            PlayerPrefs.Save();
        }

        public static string GetRegisterDay()
        {
            return PlayerPrefs.GetString("register_day", "");
        }

        public static void SetRegisterDate()
        {
            if (GetRegisterDay().Length > 0) return;
        
            var today = DateTime.Now;
        
            PlayerPrefs.SetString("register_day", today.ToString());
            PlayerPrefs.Save();
        }

        public static string GetStoreUrl()
        {
            string url;
#if (UNITY_ANDROID || UNITY_EDITOR)
            url = "https://play.google.com/";
#elif UNITY_IPHONE
            url = "https://itunes.apple.com/";
#endif
            return url;
        }
        
        #region Double
        public static void SetDouble(string key, double value)
        {
            PlayerPrefs.SetString(key, DoubleToString(value));
        }

        public static double GetDouble(string key, double defaultValue)
        {
            var defaultVal = DoubleToString(defaultValue);
            return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
        }

        public static double GetDouble(string key)
        {
            return GetDouble(key, 0d);
        }

        private static string DoubleToString(double target)
        {
            return target.ToString("R");
        }

        private static double StringToDouble(string target)
        {
            if (string.IsNullOrEmpty(target))
                return 0d;

            return double.Parse(target);
        }
        #endregion

        #region Bool
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            var defaultVal = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(key, defaultVal) == 1;
        }
        #endregion
    }
}
