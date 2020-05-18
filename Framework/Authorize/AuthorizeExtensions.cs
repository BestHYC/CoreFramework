using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public static class AuthorizeExtensions
    {
        public static Boolean EqualWith(this ActionDetailModel detail, ActionDetailModel other)
        {
            if (other == null) return false;
            return detail.Id == other.Id && detail.Menuid == other.Menuid;
        }
        public static Boolean EqualWith(this MenuDetailModel detail, ActionDetailModel other)
        {
            if (other == null) return false;
            return detail.Id == other.Id;
        }
    }
}
