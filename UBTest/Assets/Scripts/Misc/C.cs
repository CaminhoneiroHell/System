//using System.Collections.Generic;
//using DevConsole;
//using UnityEngine;

//public static class C {

//	[Command]
//    static void CloseConsole() {
//        Debug.Log("--- Console Closed ---");
//        Console.Close();
//    }
//    [Command]
//    static void CheckArduino() {
//        //Debug.Log(GameManager.instance.microcontroller.isPresent);
//    }
//    [Command]
//    static void CheckDateTime() {
//        Debug.Log(System.DateTime.Now);
//    }
//    [Command]
//    static void CheckIsAuthorizedToRun() {
//        //Debug.Log(GameManager.Instance.authorization.authorized);
//    }
//    /// <summary>
//    /// Test
//    /// </summary>
//    /// <param name="id"></param>
//    /// <param name="data"></param>
//    [Command]
//    static void CreateGame_ID_GameName_ThumbName_BatName_ExeName(string data) {
//        string formatedData = data.Replace('&', ' ');
//        string[] dataList = formatedData.Split(',');
//        GameData gd = new GameData();
//        gd.id = int.Parse(dataList[0]);
//        gd.gameName = dataList[1];
//        gd.thumbName = dataList[2];
//        gd.batName = dataList[3];

//        gd.exeName = string.Empty;
//        for (int i = 4; i < dataList.Length; i++) {
//            gd.exeName += dataList[i];
//            if (i < dataList.Length - 1) gd.exeName += ",";
//        }
//        GameManager.instance.database.CreateGameData(gd);
//    }

//    [Command]
//    static void KillGame(string exe) {
//        GameKiller.KillGame(exe);
//    }

//    [Command]
//    static void CheckIsOnFocus() {
//        Debug.Log(GameManager.instance.gameFocus.isOnFocus);
//    }

//    [Command]
//    static void ProcessStart(string path) {
//        try {
//            System.Diagnostics.Process.Start(path);
//        }
//        catch (System.Exception e) {
//            Debug.Log("error: " + e.Message);
//        }
//    }
//}
