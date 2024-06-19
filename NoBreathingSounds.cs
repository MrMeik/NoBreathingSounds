using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NoBreathingSounds
{
    [HarmonyPatch]
    public class NoBreathingSounds : ModBehaviour
    {
        public static NoBreathingSounds Instance;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(NoBreathingSounds)} is loaded!", MessageType.Success);

            new Harmony("MrMeik.NoBreathingSounds").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

            StartCoroutine(PatchAudio());
        }

        private IEnumerator PatchAudio()
        {
            yield return new WaitForSecondsRealtime(1);

            Dictionary<int, AudioLibrary.AudioEntry> dict = (Dictionary<int, AudioLibrary.AudioEntry>)AccessTools.Field(typeof(AudioManager), "_audioLibraryDict").GetValue(Locator.GetAudioManager());
            dict.Remove((int)AudioType.PlayerBreathing_LP);
            dict.Remove((int)AudioType.PlayerBreathing_LowOxygen_LP);

            Instance.ModHelper.Console.WriteLine($"All sounds patched!", MessageType.Success);
        }
    }

}
