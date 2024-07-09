using MudBlazor;

namespace Highgeek.McWebApp.Common.Helpers
{
    public class ThemeHelper
    {

        public static string MainAppBackgroud = Colors.Grey.Darken4;

        public static MudTheme CustomTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Dark = "111111",
                AppbarBackground = Colors.Grey.Darken4,

                Primary = "1B5E20",

                Secondary = Colors.Green.Accent4,


                Black = Colors.Cyan.Default,

                TextPrimary = Colors.Grey.Lighten4,

                TextSecondary = Colors.Grey.Lighten1,
            }
        };

        public static Palette CustomPalette = new Palette()
        {
            Primary = Colors.Blue.Default,
            Secondary = Colors.Green.Accent4,
            AppbarBackground = Colors.Red.Default,
            Black = Colors.Cyan.Default,
        };
    }
}
