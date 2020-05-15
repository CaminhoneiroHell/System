using System.Collections.Generic;
using UnityEngine;

namespace QFSW.QC.Extras
{
    public static class ScreenCommands
    {
        [Command("fullscreen", "fullscreen state of the application.")]
        private static bool Fullscreen
        {
            get => Screen.fullScreen;
            set => Screen.fullScreen = value;
        }

        [Command("screen-dpi", "dpi of the current device's screen.")]
        private static float DPI => Screen.dpi;

        [Command("screen-orientation", "the orientation of the screen.")]
        [CommandPlatform(Platform.MobilePlatforms)]
        private static ScreenOrientation Orientation
        {
            get => Screen.orientation;
            set => Screen.orientation = value;
        }

        [Command("current-resolution", "current resolution of the application or window.")]
        private static Resolution GetCurrentResolution()
        {
            Resolution resolution = new Resolution
            {
                width = Screen.width,
                height = Screen.height,
                refreshRate = Screen.currentResolution.refreshRate
            };

            return resolution;
        }

        [Command("supported-resolutions", "all resolutions supported by this device in fullscreen mode.")]
        private static IEnumerable<Resolution> GetSupportedResolutions()
        {
            foreach (Resolution resolution in Screen.resolutions)
            {
                yield return resolution;
            }
        }

        [Command("set-resolution")]
        private static void SetResolution(int x, int y)
        {
            SetResolution(x, y, Screen.fullScreen);
        }

        [Command("set-resolution", "sets the resolution of the current application, optionally setting the fullscreen state too.")]
        private static void SetResolution(int x, int y, bool fullscreen)
        {
            Screen.SetResolution(x, y, fullscreen);
        }
    }
}
