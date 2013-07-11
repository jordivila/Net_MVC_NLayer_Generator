using System;

namespace $safeprojectname$.Enumerations
{

    [Serializable]
    [Flags]
    public enum SiteRoles
    {
        Administrator = 1,
        Guest = 2
    }
    public enum MediaType
    {
        javascript,
        stylesheet,
        none
    }
    public enum ControllerResourceType
    {
        Javascript,
        Stylesheet
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public enum Position
    {
        Horizontal,
        Vertical,
    }
    public enum ThemesAvailable
    {
        Base,
        Black_Tie,
        Blitzer,
        Cupertino,
        Dark_Hive,
        Dot_Luv,
        Eggplant,
        Excite_Bike,
        Flick,
        Hot_Sneaks,
        Humanity,
        Le_Frog,
        Mint_Choc,
        Overcast,
        Pepper_Grinder,
        Redmond,
        Smoothness,
        South_Street,
        Start,
        Sunny,
        Swanky_Purse,
        Trontastic,
        UI_Darkness,
        UI_Lightness,
        Vader
    }
    public static class LoggerCategories
    {
        public static string WCFGeneral = "WCFGeneral";
        public static string WCFBeginRequest = "WCFBeginRequest";
        public static string UIGeneral = "UIGeneral";
        public static string UIBeginRequest = "UIBeginRequest";
        public static string UIServerSideUnhandledException = "UIServerSideUnhandledException";
        public static string UIClientSideJavascriptError = "UIClientSideJavascriptError";
    }
    public enum PageSizesAvailable : int
    {
        RowsPerPage10 = 10,
        RowsPerPage20 = 20,
        RowsPerPage30 = 30,
        RowsPerPage40 = 40,
        RowsPerPage50 = 50,
    }
    public enum DataResultMessageType : int
    {
        Success = 0,
        Warning = 1,
        Error = 2,
        Confirm = 3
    }

}