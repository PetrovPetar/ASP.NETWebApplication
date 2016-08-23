<<<<<<< HEAD
﻿
namespace WebApplication.Classes
{
    public class Utils
    {
        public static string CutText(string text, int maxLength = 450)
        {
            if (text == null || text.Length <= maxLength)
            {
                return text;
            }
            var shortText = text.Substring(0, maxLength - 3) + "...";
            return shortText;
        }

    }
=======
﻿
namespace WebApplication.Classes
{
    public class Utils
    {
        public static string CutText(string text, int maxLength = 450)
        {
            if (text == null || text.Length <= maxLength)
            {
                return text;
            }
            var shortText = text.Substring(0, maxLength - 3) + "...";
            return shortText;
        }

    }
>>>>>>> d792e6801b2b6d3c9a4d9063835e002fabf139f1
}