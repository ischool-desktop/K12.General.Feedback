using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FISCA;
using FISCA.Presentation;
using System.IO;
using FISCA.DSAUtil;
using K12.General.Feedback.Feature;
using System.Xml;

namespace K12.General.Feedback
{
    public static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [MainMethod()]
        public static void Main()
        {
            string URL最新消息 = "ischool/高中系統/共用/系統/最新消息";

            FISCA.Features.Register(URL最新消息, arg =>
            {
                new NewsForm().ShowDialog();
            });

            MotherForm.StartMenu["最新消息"].Image = Properties.Resources.speech_balloon_64;
            MotherForm.StartMenu["最新消息"].Click += delegate
            {
                Features.Invoke(URL最新消息);
            };

            if (K12.General.Feedback.Program.Extension == null)
                K12.General.Feedback.Program.Extension = new General.Feedback.ExtensionData();

            K12.General.Feedback.Program.Extension.SchoolChineseName = K12.Data.School.ChineseName;

            //K12.General.Feedback.Program.RegisterStartButton();

            new K12.General.Feedback.NewsNotice();
        }

        public static void RegisterStartButton()
        {
            //MotherForm.StartMenu["使用者回饋"]["問題回報與建議"].Click += delegate
            //{
            //    new FeedbackForm().ShowDialog();
            //};

            #region 做罷啦...
            //bool IsNews = false;
            //DSXmlHelper helper = Service.GetNewsForUsers();
            //foreach (XmlElement news in helper.GetElements("News"))
            //{
            //    DSXmlHelper newsHelper = new DSXmlHelper(news);
            //    DateTime dt = DateTime.Parse(newsHelper.GetText("PostTime"));
            //    if (dt.CompareTo(DateTime.Today.AddDays(-2)) == 1)
            //    {
            //        IsNews = true;
            //    }
            //}
            //if (IsNews)
            //{
            //    MotherForm.StartMenu["使用者回饋(最新消息)"].Image = Properties.Resources.speech_balloon_64;
            //}
            //else
            //{
            //} 
            #endregion



            //MotherForm.StartMenu["使用者回饋"]["功能投票"].Click += delegate
            //{
            //    new VoteForm().ShowDialog();
            //};
        }

        /// <summary>
        /// 延申資料。
        /// </summary>
        public static ExtensionData Extension { get; set; }

        public static bool DevelopmentMode
        {
            get { return File.Exists(Path.Combine(Application.StartupPath, "測試使用者回饋")); }
        }
    }
}
