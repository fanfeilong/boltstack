using System.Windows.Forms;

namespace ComicDown.UI.Core.WinForm
{
    public static class WinFormExtension
    {
        public static DialogResult ToDialogResult(this string dialogResultStr)
        {
            switch (dialogResultStr)
            {
                case "Abort":
                    return DialogResult.Abort;
                case "Cancel":
                    return DialogResult.Cancel;
                case "Ignore":
                    return DialogResult.Ignore;
                case "No":
                    return DialogResult.No;
                case "None":
                    return DialogResult.None;
                case "OK":
                    return DialogResult.OK;
                case "Retry":
                    return DialogResult.Retry;
                case "Yes":
                    return DialogResult.Yes;
                default:
                    return DialogResult.None;
            }
        }
    }
}
