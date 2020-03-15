public enum State
{
    Init, //Load important stuff
    MainScreen, //VR y/n option //Check locked/unlocked scenes //Load Score Stats and others persistent data
    Menu, //Select track  / exit
    VRMenu, //Select track using target crosshair / exit
    SelectTrack, //UI 
    SelectTrackVR,//UI
    MooMooFarm_race,//Stage
    Underwater_race,//Unlockable Stage
    RainbowRoad_race,//Unlockable Stage
    Stats, // Status scores and other data
    Credits // Credits 
}