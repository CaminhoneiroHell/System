//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SecretConsole : MonoBehaviour {

//    private uint phraseSize = 32;
//    private string currentPhrase;

//    #region inputs
//    private void Update() {
//        if (Input.GetKeyDown(KeyCode.Alpha0)) {
//            AddSecretPhraseKey('0');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha1)) {
//            AddSecretPhraseKey('1');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha2)) {
//            AddSecretPhraseKey('2');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha3)) {
//            AddSecretPhraseKey('3');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha4)) {
//            AddSecretPhraseKey('4');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha5)) {
//            AddSecretPhraseKey('5');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha6)) {
//            AddSecretPhraseKey('6');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha7)) {
//            AddSecretPhraseKey('7');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha8)) {
//            AddSecretPhraseKey('8');
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha9)) {
//            AddSecretPhraseKey('9');
//        }
//        if (Input.GetKeyDown(KeyCode.A)) {
//            AddSecretPhraseKey('a');
//        }
//        if (Input.GetKeyDown(KeyCode.B)) {
//            AddSecretPhraseKey('b');
//        }
//        if (Input.GetKeyDown(KeyCode.C)) {
//            AddSecretPhraseKey('c');
//        }
//        if (Input.GetKeyDown(KeyCode.D)) {
//            AddSecretPhraseKey('d');
//        }
//        if (Input.GetKeyDown(KeyCode.E)) {
//            AddSecretPhraseKey('e');
//        }
//        if (Input.GetKeyDown(KeyCode.F)) {
//            AddSecretPhraseKey('f');
//        }
//        if (Input.GetKeyDown(KeyCode.G)) {
//            AddSecretPhraseKey('g');
//        }
//        if (Input.GetKeyDown(KeyCode.H)) {
//            AddSecretPhraseKey('h');
//        }
//        if (Input.GetKeyDown(KeyCode.I)) {
//            AddSecretPhraseKey('i');
//        }
//        if (Input.GetKeyDown(KeyCode.J)) {
//            AddSecretPhraseKey('j');
//        }
//        if (Input.GetKeyDown(KeyCode.K)) {
//            AddSecretPhraseKey('k');
//        }
//        if (Input.GetKeyDown(KeyCode.L)) {
//            AddSecretPhraseKey('l');
//        }
//        if (Input.GetKeyDown(KeyCode.M)) {
//            AddSecretPhraseKey('m');
//        }
//        if (Input.GetKeyDown(KeyCode.N)) {
//            AddSecretPhraseKey('n');
//        }
//        if (Input.GetKeyDown(KeyCode.O)) {
//            AddSecretPhraseKey('o');
//        }
//        if (Input.GetKeyDown(KeyCode.P)) {
//            AddSecretPhraseKey('p');
//        }
//        if (Input.GetKeyDown(KeyCode.Q)) {
//            AddSecretPhraseKey('q');
//        }
//        if (Input.GetKeyDown(KeyCode.R)) {
//            AddSecretPhraseKey('r');
//        }
//        if (Input.GetKeyDown(KeyCode.S)) {
//            AddSecretPhraseKey('s');
//        }
//        if (Input.GetKeyDown(KeyCode.T)) {
//            AddSecretPhraseKey('t');
//        }
//        if (Input.GetKeyDown(KeyCode.U)) {
//            AddSecretPhraseKey('u');
//        }
//        if (Input.GetKeyDown(KeyCode.V)) {
//            AddSecretPhraseKey('v');
//        }
//        if (Input.GetKeyDown(KeyCode.W)) {
//            AddSecretPhraseKey('w');
//        }
//        if (Input.GetKeyDown(KeyCode.X)) {
//            AddSecretPhraseKey('x');
//        }
//        if (Input.GetKeyDown(KeyCode.Y)) {
//            AddSecretPhraseKey('y');
//        }
//        if (Input.GetKeyDown(KeyCode.Z)) {
//            AddSecretPhraseKey('z');
//        }
//    }
//    #endregion

//    private void AddSecretPhraseKey(char c) {
//        currentPhrase += c;
//        if (currentPhrase.Length > phraseSize) {
//            currentPhrase = currentPhrase.Remove(0, 1);
//        }
//        else if (currentPhrase.Contains("console")) {
//            Debug.Log("--- Console Open ---");
//            DevConsole.Console.Open();
//            currentPhrase = string.Empty;
//        }
//    }


//    private void Start()
//    {
//        DevConsole.Console.Open();
//    }
//}
