using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Campus.Configuration;

namespace K12.General.Feedback
{
    internal class PreferenceCollection
    {
        public const string FeedbackPreferenceName = "FeedbackPreference";

        public XmlElement this[string name]
        {
            get
            {
                if (Config.User != null)
                {
                    return Config.User[FeedbackPreferenceName].GetXml(name, null);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Config.User != null)
                {
                    ConfigData cd = Config.User[FeedbackPreferenceName];
                    cd.SetXml(name, value);
                    cd.Save();
                }
                else
                {

                }
            }
        }
    }
}
