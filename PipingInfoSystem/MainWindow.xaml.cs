using PipingInfoSystem.model;
using PipingInfoSystem.module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Office.Interop.Word;
using System.Threading;

namespace PipingInfoSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loginUser.UserName = "未注册用户";
            loginUser.UserType = -1;
            this.DataContext = loginUser;
        }
        public static User loginUser = new User();
        private void Hover(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lb = sender as System.Windows.Controls.Label;
            lb.Foreground =new SolidColorBrush(Color.FromRgb(155, 155, 155));
            //lb.BorderThickness = new Thickness(1, 1, 1, 1);
            //lb.BorderBrush = new SolidColorBrush(Color.FromRgb(155, 155, 155));
        }
        private void Unhover(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Label lb = sender as System.Windows.Controls.Label;
            lb.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //lb.BorderThickness = new Thickness(1, 1, 1, 1);
            //lb.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }
        private void showAddWindow(object sender, RoutedEventArgs e)
        {   if (loginUser.UserType > -1)
            {
                new AddInfomation().ShowDialog();
                LoadComboBox(sender, e);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("临时用户权限不够，请先登陆或注册");
            }
        }
        private void Modify(object sender, RoutedEventArgs e)
        {
            PipingDetectionInfo detectionInfo = (PipingDetectionInfo)pipingInfoData.SelectedItem;
 
            if (loginUser.UserType > -1 && detectionInfo != null)
            {
                new AddInfomation(detectionInfo).ShowDialog();
                LoadComboBox(sender, e);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Format("请先选择一行或登陆"));
            }
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Mini(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            DbModel handle = new DbModel();
            var s = startNo.Text;
            var l = layingDate.SelectionBoxItem.ToString();
            var c = caizhi.SelectionBoxItem.ToString();
            var z = zhijing.SelectionBoxItem.ToString();
            var t = type.SelectionBoxItem.ToString();

            List<PipingDetectionInfo> result = handle.PipingDetectionInfoes.ToList().FindAll(p => p.StartWellNo.Contains(s) && p.LayingYear.Contains(l) && p.TubulationMaterial.Contains(c) && p.TubulationDiameter.Contains(z) && p.TubulationType.Contains(t));

            pipingInfoData.ItemsSource = result;
            if (result == null || result.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("数据库无符合条件数据"));
            }
        }

        /// <summary>
        /// 生成Word
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Print(object sender, RoutedEventArgs e)
        {
            if (loginUser.UserType > -1)
            {
                try
                {
                    PipingDetectionInfo detectionInfo = (PipingDetectionInfo)pipingInfoData.SelectedItem;

                    DbModel handle = new DbModel();
                    List<PipingPictureInfo> result = handle.PipingPictureInfoes.ToList().FindAll(p => p.PipingID == detectionInfo.PipingID);

                    string mbWord = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + "PipingInfo.doc";
                    string printWordPath;
                    FolderBrowserDialog dilog = new FolderBrowserDialog();
                    dilog.Description = "请选择文件夹";
                    if (dilog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        printWordPath = string.Format(@"{0}\{1}.doc", dilog.SelectedPath, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        Report report = new Report();

                        report.CreateNewDocument(mbWord);

                        // 检测信息
                        report.InsertValue("t1", detectionInfo.VideoFile);
                        report.InsertValue("t2", detectionInfo.StartWellNo);
                        report.InsertValue("t3", detectionInfo.EndWellNo);
                        report.InsertValue("t4", detectionInfo.LayingYear);
                        report.InsertValue("t5", detectionInfo.StartPointDepth);
                        report.InsertValue("t6", detectionInfo.EndPointDepth);
                        report.InsertValue("t7", detectionInfo.TubulationType);
                        report.InsertValue("t8", detectionInfo.TubulationMaterial);
                        report.InsertValue("t9", detectionInfo.TubulationDiameter);
                        report.InsertValue("t10", detectionInfo.DetectionDirect);
                        report.InsertValue("t11", detectionInfo.TubulationLength);
                        report.InsertValue("t12", detectionInfo.DetectionLength);
                        report.InsertValue("t13", detectionInfo.DetectionAddress);
                        report.InsertValue("t14", detectionInfo.DetectionTime);
                        report.InsertValue("XH", string.Format("{0}→{1}", detectionInfo.StartWellNo, detectionInfo.EndWellNo));
                        report.InsertValue("JCFF", detectionInfo.DetectionFun);
                        report.InsertValue("remark", result.Count > 0 ? result[0].Remark : string.Empty);

                        result.OrderBy(p => p.PictureID);

                        // 工程图片信息
                        PipingPictureInfo picInfo = result.Find(p => p.PictureType == 0);
                        if (picInfo != null)
                        {
                            report.InsertPicture("gcTP", picInfo.PictureFilePath, 220, 220);
                            result.RemoveAll(p => p.PictureType == 0);
                        }

                        Table picRemarkTable = report.Document.Content.Tables[2];
                        Table remarkTable = report.Document.Content.Tables[3];
                        Table picTable = report.Document.Content.Tables[4];

                        // 添加图片信息描述 + 添加图片
                        for (int i = 0; i < result.Count; i++)
                        {
                            if (i > 0)
                            {
                                report.AddRow(2, 1);
                            }

                            report.InsertCell(picRemarkTable, i + 2, 1, result[i].Distance);
                            report.InsertCell(picRemarkTable, i + 2, 2, result[i].DefectCode);
                            report.InsertCell(picRemarkTable, i + 2, 3, result[i].Score);
                            report.InsertCell(picRemarkTable, i + 2, 4, result[i].Grade);
                            report.InsertCell(picRemarkTable, i + 2, 5, result[i].PipingInternalState);
                            report.InsertCell(picRemarkTable, i + 2, 6, result[i].PictureID);

                            if (i == 0)
                            {
                                report.InsertPicture("TP1", result[i].PictureFilePath, 220, 220);
                                report.InsertValue("TP1MS", result[i].PictureRemark);
                            }
                            else
                            {
                                report.InsertPicture(string.Format("TP{0}", i + 1), result[i].PictureFilePath, 220, 220);
                                report.InsertValue(string.Format("TP{0}MS", i + 1), result[i].PictureRemark);
                            }
                        }

                        // 图片的数量
                        int picCount = result.Count;

                        // 删除多余的行
                        for (int i = 0; i < (5 - (picCount) / 2) * 2; i++)
                        {
                            report.DeleteRow(4, 12 - i);
                        }

                        report.Application.Visible = true;
                        report.Application.Activate();
                        report.Application.ShowMe();

                        object what = WdGoToItem.wdGoToBookmark;
                        object name = "KG1";
                        object missing = System.Reflection.Missing.Value;
                        report.Application.Selection.GoTo(what, missing, missing, name);
                        SendKeys.SendWait("{DEL}");

                        Thread.Sleep(10);

                        name = "KG2";
                        report.Application.Selection.GoTo(what, missing, missing, name);
                        SendKeys.SendWait("{DEL}");

                        Thread.Sleep(10);

                        report.SaveDocument(printWordPath);

                        System.Windows.Forms.MessageBox.Show(string.Format("生成成功！文件地址：{0}", printWordPath));

                    }
                    else
                    {
                        dilog.Dispose();
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("生成失败！"));

                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Format("请先登录！"));
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Delete(object sender, RoutedEventArgs e)
        {

            if (loginUser.UserType > -1)
            {
                DbModel handle = new DbModel();
                foreach (var i in pipingInfoData.SelectedItems)
                {
                    PipingDetectionInfo detectionInfo = i as PipingDetectionInfo;
                    if (detectionInfo != null)
                    {
                        handle.PipingDetectionInfoes.Remove(handle.PipingDetectionInfoes.ToList().Find(p => p.PipingID == detectionInfo.PipingID));
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("请先选择一条数据！");
                    }
                }
                if (System.Windows.Forms.MessageBox.Show("确认删除选中数据？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //调用删除函数
                    int result = handle.SaveChanges();
                    if (result > 0)
                    {
                        pipingInfoData.ItemsSource = handle.PipingDetectionInfoes.ToList();
                        LoadComboBox(sender, e);
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请先登录！");
            }

        }

        /// <summary>
        /// 记载数据
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void LoadComboBox(object sender, RoutedEventArgs e)
        {
            List<string> layingYearData = new List<string>() { "" };
            DbModel handle = new DbModel();
            var layingYear = (from q in handle.PipingDetectionInfoes
                              select q.LayingYear).Distinct();

            layingYearData.AddRange(layingYear.ToList());
            layingDate.ItemsSource = layingYearData;

            List<string> tubulationMaterialData = new List<string>() { "" };
            var tubulationMaterial = (from q in handle.PipingDetectionInfoes
                                      orderby q.LayingYear
                                      select q.TubulationMaterial).Distinct();
            tubulationMaterialData.AddRange(tubulationMaterial.ToList());
            caizhi.ItemsSource = tubulationMaterialData;

            List<string> tubulationDiameterData = new List<string>() { "" };
            var tubulationDiameter = (from q in handle.PipingDetectionInfoes
                                      select q.TubulationDiameter).Distinct();
            tubulationDiameterData.AddRange(tubulationDiameter.ToList());
            zhijing.ItemsSource = tubulationDiameterData;

            List<string> tubulationTypeData = new List<string>() { "" };
            var tubulationType = (from q in handle.PipingDetectionInfoes
                                  select q.TubulationType).Distinct();
            tubulationTypeData.AddRange(tubulationType.ToList());
            type.ItemsSource = tubulationTypeData;
        }

        private void Drag(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Register(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Panel.SetZIndex(reg, 1);
            reg.Opacity = 1;
        }

        private void CloseLogin(object sender, RoutedEventArgs e)
        {
            login.Opacity = 0;
            System.Windows.Controls.Panel.SetZIndex(login,-1);
        }


        private void ShowLogin(object sender, MouseButtonEventArgs e)
        {
            login.Opacity =1;
            System.Windows.Controls.Panel.SetZIndex(login,1);
        }

        private void CloseReg(object sender, RoutedEventArgs e)
        {
            reg.Opacity = 0;
            System.Windows.Controls.Panel.SetZIndex(reg, -1);
        }

        private void Regist(object sender, RoutedEventArgs e)
        {
            string username = RegUserName.Text.Trim();
            string password = MD5Helper.MD5Encrypt32(RegPassword.Text.Trim());
            string number = RegNumber.Text.Trim();
            string truename = RegTrueName.Text.Trim();
            string id = RegID.Text.Trim();

            DbModel db = new DbModel();
            var res = from q in db.UserInfoes
                      where q.UserName == username
                      select q;
            if (res.Count() < 1)
            {
                db.UserInfoes.Add(new UserInfo()
                {
                    UserName = username,
                    Password = password,
                    PhoneNumber = number,
                    TrueName = truename,
                    ID = id,
                    KeID = Guid.NewGuid().ToString("N"),
                    ApplyDate = System.DateTime.Now,
                    IsDelete =0,
                    UserType = 0,
                    UserID = Guid.NewGuid().ToString("N")
                });
                var result = db.SaveChanges();
                if (result > 0)
                {
                    loginUser.UserName = username;
                    loginUser.UserType = 0;
                    System.Windows.Forms.MessageBox.Show("注册成功，请等待管理员审核");

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("注册失败，请联系管理员");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("存在相同用户名，请更改用户名后重试");
            }
            CloseReg(sender,e);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string username = UserName.Text.Trim();
            string password = MD5Helper.MD5Encrypt32(Password.Text.Trim());
            DbModel db = new DbModel();

            var res = from q in db.UserInfoes
                      where q.Password == password && q.UserName == username
                      select q;
            if (res.Count() == 1)
            {
                loginUser.UserName = username;
                loginUser.UserType = res.FirstOrDefault().UserType;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("登陆失败");
            }
            CloseLogin(sender, e);
        }

        private void MoreInfo(object sender, MouseButtonEventArgs e)
        {
            //new MoreInfomation().Show();
        }
    }
}
