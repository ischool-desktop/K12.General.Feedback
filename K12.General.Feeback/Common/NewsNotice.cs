using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using K12.General.Feedback.Feature;
using System.Xml;
using FISCA.DSAUtil;
using FISCA.Presentation.Controls;
using System.Windows.Forms;

namespace K12.General.Feedback
{
    public class NewsNotice
    {
        private BackgroundWorker _newsLoader;
        private DateTime _serverTime = DateTime.MinValue;
        private DateTime _lastViewTime = DateTime.MinValue;

        public NewsNotice()
        {
            _newsLoader = new BackgroundWorker();
            _newsLoader.DoWork += new DoWorkEventHandler(_newsLoader_DoWork);
            _newsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_newsLoader_RunWorkerCompleted);
            _newsLoader.RunWorkerAsync();
        }

        private void _newsLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<DSXmlHelper> HelperList = e.Result as List<DSXmlHelper>;
            if (HelperList.Count > 0)
            {
                Dictionary<string, name> MessageDic = new Dictionary<string, name>();
                foreach (DSXmlHelper each in HelperList)
                {
                    DateTime time = DateTime.Parse(each.GetText("PostTime"));
                    string _time = string.Format("{0}  {1}", time.ToShortDateString(), time.ToShortTimeString());
                    if (!MessageDic.ContainsKey(_time))
                    {
                        name n = new name();
                        n._key = _time;
                        n._value = each.GetText("Message");
                        n._link = each.GetText("Url");
                        MessageDic.Add(_time, n);
                    }
                }
                IsViewForm view = new IsViewForm(MessageDic);
               // view.TopMost = true;
                view.ShowDialog();
            }

            SavePreference();
        }

        private void _newsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);

            LoadPreference();
            GetServerDateTime();


            List<DSXmlHelper> newsList = new List<DSXmlHelper>();

            DSXmlHelper helper;
            try
            {
                helper = Service.GetNewsForUsers();
            }
            catch (Exception ex)
            {
                CurrentUser.ReportError(ex);
                helper = new DSXmlHelper("BOOM");
            }

            foreach (XmlElement news in helper.GetElements("News"))
            {
                DSXmlHelper newsHelper = new DSXmlHelper(news);
                if (IsIncorrect(newsHelper.GetText("To"))) continue;

                if (DateTime.Parse(newsHelper.GetText("PostTime")) > _lastViewTime)
                    newsList.Add(newsHelper);
            }

            e.Result = newsList;

        }

        private bool IsIncorrect(string user)
        {
            CurrentUser current = CurrentUser.Instance;
            if (user == "*/*") return false;
            if (user == current.AccessPoint + "/*") return false;
            if (user == current.AccessPoint + "/" + current.UserName) return false;
            return true;
        }

        private void GetServerDateTime()
        {
            try
            {
                DSXmlHelper helper = Service.GetDateTimeNow();
                DateTime a;
                if (!DateTime.TryParse(helper.GetText("DateTime"), out a))
                    _serverTime = DateTime.Now;
                _serverTime = a;
            }
            catch (Exception ex)
            {
                CurrentUser.ReportError(ex);
                _serverTime = DateTime.Now;
            }
        }

        private void LoadPreference()
        {
            _lastViewTime = DateTime.MaxValue;
            try
            {
                XmlElement p = CurrentUser.Instance.Preference["News"];
                if (p != null)
                {
                    DateTime a;
                    if (DateTime.TryParse(p.GetAttribute("LastViewTime"), out a))
                        _lastViewTime = a;
                    //_lastViewTime = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SavePreference()
        {
            XmlElement p = CurrentUser.Instance.Preference["News"];
            if (p == null)
            {
                p = new XmlDocument().CreateElement("News");
            }
            p.SetAttribute("LastViewTime", "" + _serverTime.ToString("yyyy/MM/dd HH:mm:ss"));
            CurrentUser.Instance.Preference["News"] = p;
        }
    }
}
