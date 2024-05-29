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
        public static LevelData currentLevelData;
        public static LevelController currentGameObject;
        public static LevelFinishParams levelFinishParams;
        private static string currentLevelTitle;

        //private static AsyncOperationHandle<TextAsset> _asyncOperationHandle;

        public static void SetLevelNumber(int levelNumber)
        {
            currentLevelNumber = levelNumber;
        }
        public static LevelData GetLevelData()
        {
            return currentLevelData;
        } 
        public static async Task<LevelData> GetLevelData(int i)
        {

            AsyncOperationHandle<GameObject> _asyncOperationHandle
                = Addressables.LoadAssetAsync<GameObject>($"Levels/Level_0.prefab");
            
            await _asyncOperationHandle.Task;
            //Debug.Log("_asyncOperationHandle"+i+"finished");

            currentGameObject = _asyncOperationHandle.Result.gameObject.GetComponent<LevelController>();
            LevelData levelData = currentGameObject.levelData;
            currentLevelData = levelData;
            return levelData;
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