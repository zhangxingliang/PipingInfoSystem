using PipingInfoSystem.module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Threading;
using BasicService;
using System.Windows.Input;

namespace PipingInfoSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        EFService service = EFService.GetClient();
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
            lb.Foreground = new SolidColorBrush(Color.FromRgb(155, 155, 155));
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
        {
            if (loginUser.UserType > -1)
            {
                new AddInfomation().ShowDialog();
                LoadComboBox(sender, e);
                Search(sender, e);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("临时用户权限不够，请先登陆或注册");
            }
        }
        private void Modify(object sender, RoutedEventArgs e)
        {
            PipingInfo detectionInfo = (PipingInfo)pipingInfoData.SelectedItem;

            if (loginUser.UserType > -1 && detectionInfo != null)
            {
                new AddInfomation(detectionInfo.PipingID).ShowDialog();
                LoadComboBox(sender, e);
                Search(sender, e);
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
            SearchRequst requst = new SearchRequst();
            requst.startNo = startNo.Text;
            requst.year = layingDate.SelectionBoxItem.ToString();
            requst.texture = caizhi.SelectionBoxItem.ToString();
            requst.diameter = zhijing.SelectionBoxItem.ToString();
            requst.type = type.SelectionBoxItem.ToString();

            ResponseMessage<List<PipingInfo>> result = service.Search(loginUser.UserName, requst);
            pipingInfoData.ItemsSource = result.ext;
            if (IsOver.IsChecked == true)
            {
                ShowOnlyOrNot(sender, e);
            }
            SortResult();
            if (result == null || result.ext.Count == 0)
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
            ResponseMessage result = new ResponseMessage();
            if (loginUser.UserType > -1)
            {
                try
                {
                    PipingInfo detectionInfo = (PipingInfo)pipingInfoData.SelectedItem;
                    string printWordPath;
                    FolderBrowserDialog dilog = new FolderBrowserDialog();
                    dilog.Description = "请选择文件夹";
                    if (dilog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        printWordPath = string.Format(@"{0}\{1}.doc", dilog.SelectedPath, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        result = service.Report(loginUser.UserName, detectionInfo.PipingID, printWordPath);
                        if (result.code == "0")
                        {
                            System.Windows.Forms.MessageBox.Show(result.msg);
                        }
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
            ResponseMessage result = new ResponseMessage();
            if (loginUser.UserType > -1 && System.Windows.Forms.MessageBox.Show("确认删除选中数据？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {

                foreach (var i in pipingInfoData.SelectedItems)
                {
                    PipingInfo delectionInfo = i as PipingInfo;
                    if (delectionInfo != null)
                    {
                        result = service.Delete(loginUser.UserName, delectionInfo.PipingID);
                        if (result.code == "0")
                        {
                            pipingInfoData.ItemsSource = service.Search(loginUser.UserName, new SearchRequst()).ext;
                            LoadComboBox(sender, e);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("请先选择一条数据！");
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
            List<PipingInfo> result = service.Search(loginUser.UserName, new SearchRequst()).ext;
            List<string> layingYearData = new List<string>() { "" };
            var layingYear = (from q in result
                              select q.LayingYear).Distinct();

            layingYearData.AddRange(layingYear.ToList());
            layingDate.ItemsSource = layingYearData;

            List<string> tubulationMaterialData = new List<string>() { "" };
            var tubulationMaterial = (from q in result
                                      orderby q.LayingYear
                                      select q.TubulationMaterial).Distinct();
            tubulationMaterialData.AddRange(tubulationMaterial.ToList());
            caizhi.ItemsSource = tubulationMaterialData;

            List<string> tubulationDiameterData = new List<string>() { "" };
            var tubulationDiameter = (from q in result
                                      select q.TubulationDiameter).Distinct();
            tubulationDiameterData.AddRange(tubulationDiameter.ToList());
            zhijing.ItemsSource = tubulationDiameterData;

            List<string> tubulationTypeData = new List<string>() { "" };
            var tubulationType = (from q in result
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
            System.Windows.Controls.Panel.SetZIndex(login, -1);
        }


        private void ShowLogin(object sender, MouseButtonEventArgs e)
        {
            login.Opacity = 1;
            System.Windows.Controls.Panel.SetZIndex(login, 1);
        }

        private void CloseReg(object sender, RoutedEventArgs e)
        {
            reg.Opacity = 0;
            System.Windows.Controls.Panel.SetZIndex(reg, -1);
        }

        private void Regist(object sender, RoutedEventArgs e)
        {
            ResponseMessage result = new ResponseMessage();
            Regist user = new Regist();
            user.username = RegUserName.Text.Trim();
            user.password = RegPassword.Text.Trim();
            user.number = RegNumber.Text.Trim();
            user.truename = RegTrueName.Text.Trim();
            user.id = RegID.Text.Trim();

            result = service.Regist(user);
            if (result.code == "0")
            {

                loginUser.UserName = user.username;
                loginUser.UserType = 0;
                System.Windows.Forms.MessageBox.Show("注册成功，请等待管理员审核");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("存在相同用户名，请更改用户名后重试");
            }
            CloseReg(sender, e);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            ResponseMessage<User> result = new ResponseMessage<User>();
            string username = UserName.Text.Trim();
            string password = Password.Text.Trim();
            result = service.Login(username, password);
            if (result.code == "0")
            {
                loginUser.UserName = username;
                loginUser.UserType = result.ext.UserType;
                loginUser.UserToken = result.ext.UserToken;
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

        private void ShowOnlyOrNot(object sender, RoutedEventArgs e)
        {
            if (pipingInfoData.ItemsSource != null)
            {
                if (IsOver.IsChecked == true)
                {
                    pipingInfoData.ItemsSource = (pipingInfoData.ItemsSource as List<PipingInfo>).FindAll(p => p.IsOverTime == true);
                }
                else
                {
                    Search(sender, e);
                }
            }
        }

        private void Sort(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (pipingInfoData != null)
            {
                SortResult();
            }
        }
        private void SortResult()
        {
            switch (sort.SelectedIndex)
            {
                case 0:
                    {
                        List<PipingInfo> data = pipingInfoData.ItemsSource as List<PipingInfo>;
                        data.Sort((x, y) => x.LayingYear.CompareTo(y.LayingYear));
                        pipingInfoData.ItemsSource = null;
                        pipingInfoData.ItemsSource = data;
                        break;
                    }
                case 1:
                    {
                        List<PipingInfo> data = pipingInfoData.ItemsSource as List<PipingInfo>;
                        data.Sort((x, y) => x.TubulationMaterial.CompareTo(y.TubulationMaterial));
                        pipingInfoData.ItemsSource = null;
                        pipingInfoData.ItemsSource = data;
                        break;
                    }
                case 2:
                    {
                        List<PipingInfo> data = pipingInfoData.ItemsSource as List<PipingInfo>;
                        data.Sort((x, y) => x.TubulationDiameter.CompareTo(y.TubulationDiameter));
                        pipingInfoData.ItemsSource = null;
                        pipingInfoData.ItemsSource = data;
                        break;
                    }
                case 3:
                    {
                        List<PipingInfo> data = pipingInfoData.ItemsSource as List<PipingInfo>;
                        data.Sort((x, y) => x.TubulationType.CompareTo(y.TubulationType));
                        pipingInfoData.ItemsSource = null;
                        pipingInfoData.ItemsSource = data;
                        break;
                    }
            }
        }

        private void showMapWindow(object sender, RoutedEventArgs e)
        {
            List<PipingInfo> list = pipingInfoData.ItemsSource as List<PipingInfo>;
            new Map(list).ShowDialog();
        }

        private void ShowOnePoint(object sender, MouseButtonEventArgs e)
        {
            PipingInfo info = pipingInfoData.SelectedItem as PipingInfo;
            List<PipingInfo> list = new List<PipingInfo>();
            list.Add(info);
            new Map(list).ShowDialog();
        }
    }
}
