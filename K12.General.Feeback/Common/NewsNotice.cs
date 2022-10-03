using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using K12.General.Feedback.Feature;
using System.Xml;
using FISCA.DSAUtil;
using FISCA.Presentation.Controls;
using System.Windows.Forms;
using System.Drawing;
using Campus.Message;

namespace K12.General.Feedback
{
    public class NewsNotice
    {
        private BackgroundWorker _newsLoader;
        private DateTime _serverTime = DateTime.MinValue;
        private DateTime _lastViewTime = DateTime.MinValue;

        //Dictionary<string, name> MessageDic { get; set; }

        AlertCustom m_AlertOnLoad { get; set; }

        TaskbarNotifier taskbarNotifier3 { get; set; }

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

                foreach (DSXmlHelper each in HelperList)
                {
                    DateTime time = DateTime.Parse(each.GetText("PostTime"));

                    name n = new name();
                    n._Time = time.ToString();
                    n._key = time.ToString();
                    n._value = each.GetText("Message");
                    n._link = each.GetText("Url");

                    CustomRecord cr = new CustomRecord();
                    cr.Title = "<b>最新消息</b>";
                    if (n._value.Contains("(*)"))
                    {
                        cr.Type = CrType.Type.Star;
                    }
                    else if (n._value.Contains("(!)"))
                    {
                        cr.Type = CrType.Type.Warning_Blue;
                    }
                    else if (n._value.Contains("(!!)"))
                    {
                        cr.Type = CrType.Type.Warning_Red;
                    }
                    else if (n._value.Contains("(#)"))
                    {
                        cr.Type = CrType.Type.Error;
                    }
                    else
                    {
                        cr.Type = CrType.Type.News;
                    }
                    cr.Content = time + "\n" + n._value;

                    IsViewForm_Open open = new IsViewForm_Open(n);
                    cr.OtherMore = open;

                    Campus.Message.MessageRobot.AddMessage(cr);

                }

                //第一改版內容
                //taskbarNotifier3 = new TaskbarNotifier();
                //taskbarNotifier3.SetBackgroundBitmap(Properties.Resources.廣告5, Color.FromArgb(255, 0, 255));
                //taskbarNotifier3.SetCloseBitmap(Properties.Resources.close, Color.FromArgb(255, 0, 255), new Point(440, 10));
                //taskbarNotifier3.TitleRectangle = new Rectangle(20, 235, 300, 180);
                //taskbarNotifier3.ContentRectangle = new Rectangle(110, 170, 350, 300);
                //taskbarNotifier3.TitleClick += taskbarNotifier3_TitleClick;
                //taskbarNotifier3.ContentClick += taskbarNotifier3_ContentClick;
                //taskbarNotifier3.CloseClick += taskbarNotifier3_CloseClick;
                //taskbarNotifier3.Show("", string.Format("(共{0}筆)立即前往>>", MessageDic.Keys.Count.ToString()), 400, 120000, 1000);
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
