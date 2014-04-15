using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FISCA.Authentication;

namespace K12.General.Feedback
{
    internal class CurrentUser
    {
        public static CurrentUser Instance
        {
            get
            {
                return new CurrentUser();
            }
        }

        public static void ReportError(Exception ex)
        {
        }

        public PreferenceCollection Preference
        {
            get
            {
                return new PreferenceCollection();
            }
        }

        public string AccessPoint
        {
            get
            {
                return DSAServices.AccessPoint;
            }
        }

        public string UserName
        {
            get
            {
                return DSAServices.UserAccount;
            }
        }

        public string SchoolChineseName
        {
            get
            {
                return Program.Extension.SchoolChineseName;
            }
        }
    }
}
