using System;
using System.Collections.Generic;
using System.Text;

namespace MavcaDetection.Constants
{
    public class MessageBoxConstant
    {
        public const string Info = "Info";
        public const string Confirmation = "Confirmation";
        public const string Success = "Success";
        public const string Warning = "Warning";
        public const string Error = "Error";

        public const string ConfirmMessage = "Are you sure to delete?";
        public const string SuccessAddMessage = "Add new success!";
        public const string SuccessEditMessage = "Edit success!";
        public const string SuccessDeleteMessage = "Delete Success!";
        public const string Invalid = "Invalid!";
        public const string ErrorMessage = "Error happens!";
    }

    public enum MessageType
    {
        Info,
        Confirmation,
        Success,
        Warning,
        Error,
    }
    public enum MessageButtons
    {
        OkCancel,
        YesNo,
        Ok,
    }
}
