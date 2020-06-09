using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework
{
    public static class CommonHelper
    {
        private static readonly PhoneNumbers.PhoneNumberUtil Instance = PhoneNumbers.PhoneNumberUtil.GetInstance();
        public static Boolean PhoneFormater(String mobile, out String phone, out String areacode)
        {
            try
            {
                var p = Instance.Parse(mobile, "CN");
                if (!Instance.IsValidNumber(p))
                {
                    LogHelper.Warn($"号码[{mobile}]不合法。");
                    phone = "";
                    areacode = "";
                    return false;
                }
                //phone = Instance.Format(p, PhoneNumbers.PhoneNumberFormat.E164);
                phone = p.NationalNumber.ToString();
                areacode = p.CountryCode.ToString();
                LogHelper.Info($"解析手机号码[{mobile}]为：[{phone}]，[{areacode}]");
                return true;
            }
            catch (Exception)
            {
                LogHelper.Warn($"号码[{mobile}]不合法。");
                phone = "";
                areacode = "";
                return false;
            }
        }
        public static bool EmailFormater(String email)
        {
            Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
            Match m = RegEmail.Match(email);
            return m.Success;
        }
    }
}
