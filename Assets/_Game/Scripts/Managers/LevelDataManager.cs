using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Data.ValueObject;
using Scripts.Keys;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.Managers
{
    public class LevelDataManager
    {
        public static int currentLevelNumber;
        public static LevelController CurrentLevelController;
        public static LevelFinishParams levelFinishParams;
        private static string currentLevelTitle;

        //private static AsyncOperationHandle<TextAsset> _asyncOperationHandle;

        public static void SetLevelNumber(int levelNumber)
        {
            currentLevelNumber = levelNumber;
        }
        public static LevelController GetLevelData()
        {
            return CurrentLevelController;
        } 
        public static async Task<LevelController> GetLevelData(int i)
        {

            AsyncOperationHandle<GameObject> _asyncOperationHandle
                = Addressables.LoadAssetAsync<GameObject>($"Levels/Level_0.prefab");
            
            await _asyncOperationHandle.Task;
            GameObject levelObject = GameObject.Instantiate(_asyncOperationHandle.Result.gameObject);
            LevelController currentLevelController = levelObject.GetComponent<LevelController>();
            CurrentLevelController = currentLevelController;
            return CurrentLevelController;
        }

        public void ReleaseAsset()
        {
            //Addressables.Release(_asyncOperationHandle);
        }
        
        public static int GetLevelScore(int levelNumber)
        {
            return PlayerPrefs.GetInt("LHS" + levelNumber, -1);
        }
        public static void SetLevelHighScore(int levelNumber,int highScore)
        {
            PlayerPrefs.SetInt("LHS" + levelNumber, highScore);
            PlayerPrefs.Save();
        }

        public static void NewUnLock(int levelNumber)
        {
            PlayerPrefs.SetInt("NewUnLock",levelNumber);
            PlayerPrefs.Save();
        }

        public static int GetNewUnlock()
        {
            return PlayerPrefs.GetInt("NewUnLock",-1);
        }
    }
}