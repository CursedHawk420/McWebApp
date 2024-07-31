using MudBlazor;

namespace Highgeek.McWebApp.Common.Helpers
{
    public class ThemeHelper
    {

        public static string MainAppBackgroud = Colors.Gray.Darken4;

        public static MudTheme CustomTheme = new MudTheme()
        {
            
            PaletteDark = new PaletteDark()
            {
                Dark = "111111",
                AppbarBackground = Colors.Gray.Darken4,

                Primary = "1B5E20",

                Surface = Colors.Gray.Darken4,

                Secondary = Colors.Green.Accent4,


                Black = Colors.Cyan.Default,

                TextPrimary = Colors.Gray.Lighten4,

                TextSecondary = Colors.Gray.Lighten1,
            },

            PaletteLight = new PaletteLight()
            {
                Dark = "111111",
                AppbarBackground = Colors.Gray.Darken4,

                Primary = "1B5E20",

                Surface = Colors.Gray.Darken4,

                Secondary = Colors.Green.Accent4,


                Black = Colors.Cyan.Default,

                TextPrimary = Colors.Gray.Lighten4,

                TextSecondary = Colors.Gray.Lighten1,
            }
        };

        /*public static Palette CustomPalette = new Palette()
        {
            Primary = Colors.Blue.Default,
            Secondary = Colors.Green.Accent4,
            AppbarBackground = Colors.Red.Default,
            Black = Colors.Cyan.Default,
        };*/
    }
}
