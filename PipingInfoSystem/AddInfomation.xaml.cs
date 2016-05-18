using BasicService;
using PipingInfoSystem.module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PipingInfoSystem
{

    /// <summary>
    /// AddInfomation.xaml 的交互逻辑
    /// </summary>
    public partial class AddInfomation : Window
    {
        EFService service = EFService.GetClient();
        public AddInfomation()
        {
            InitializeComponent();
        }
        string pipingId = null;
        string keyId = null;
        public AddInfomation(string pipingid)
        {
            InitializeComponent();
            PipingDetailInfo detailInfo = service.GetInfo(MainWindow.loginUser.UserName, pipingid).ext;
            keyId = detailInfo.PipingDetectionInfo.KeyID;
            pipingId = pipingid;
            TextBox1.Text = detailInfo.PipingDetectionInfo.VideoFile;
            TextBox2.Text = detailInfo.PipingDetectionInfo.LayingYear;
            TextBox3.Text = detailInfo.PipingDetectionInfo.TubulationType;
            TextBox4.Text = detailInfo.PipingDetectionInfo.DetectionDirect;
            TextBox5.Text = detailInfo.PipingDetectionInfo.DetectionAddress;
            TextBox6.Text = detailInfo.PipingDetectionInfo.StartWellNo;
            TextBox7.Text = detailInfo.PipingDetectionInfo.StartPointDepth;
            TextBox8.Text = detailInfo.PipingDetectionInfo.TubulationMaterial;
            TextBox9.Text = detailInfo.PipingDetectionInfo.TubulationLength;
            TextBox10.Text = detailInfo.PipingDetectionInfo.DetectionFun;
            TextBox11.Text = detailInfo.PipingDetectionInfo.EndWellNo;
            TextBox12.Text = detailInfo.PipingDetectionInfo.EndPointDepth;
            TextBox13.Text = detailInfo.PipingDetectionInfo.TubulationDiameter;
            TextBox14.Text = detailInfo.PipingDetectionInfo.DetectionLength;
            TextBox15.Text = detailInfo.PipingDetectionInfo.DetectionTime;
            //drawing.Text = gcPictInfo.PictureFilePath;
            List<PipingPictureInfo> picInfo = detailInfo.PipingPictureInfoes;
            PipingPictureInfo gcPic = new PipingPictureInfo();
            foreach (var i in picInfo)
            {
                if (i.PictureType == 1)
                {
                    AddImgGrid(i.PictureFilePath);
                }
                else
                {
                    drawing.Text = i.PictureFilePath;
                    gcPic = i;
                }
                imgList.Add(i.PictureFilePath);
            }
            picInfo.Remove(gcPic);
            // 加载图片信息
            for (int i = 0; i <= pictureInfo.Children.Count - 1; i++)
            {
                StackPanel infos = (StackPanel)((Grid)pictureInfo.Children[i]).Children[2];

                ((TextBox)infos.Children[3]).Text = picInfo[i].Grade;
                ((TextBox)infos.Children[2]).Text = picInfo[i].Score;
                ((TextBox)infos.Children[4]).Text = picInfo[i].PipingInternalState;
                ((TextBox)infos.Children[5]).Text = picInfo[i].PictureRemark;
                ((TextBox)infos.Children[1]).Text = picInfo[i].DefectCode;
                ((TextBox)infos.Children[0]).Text = picInfo[i].Distance;
                remark.Text = picInfo[i].Remark;
            }
        }

        private static List<string> imgList = new List<string>();

        //选择图片 可多选
        private void AddPicture(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "图片文件(*.jpg,*.gif,*.bmp,*.png)|*.jpg;*.gif;*.bmp;*.png"
            };
            openFileDialog.Multiselect = true;
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                foreach (string i in openFileDialog.FileNames)
                {
                    try
                    {
                        imgList.Add(i);
                        this.Dispatcher.BeginInvoke(new Action(() => AddImgGrid(i)));
                    }
                    catch
                    {
                        MessageBox.Show("请检查文件格式！");
                    }
                }
            }
        }

        //添加图片及描述框
        void AddImgGrid(string i)
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(2, 2, 2, 2);
            Image img = new Image();
            BitmapImage imgBrush = new BitmapImage(new Uri(i, UriKind.Absolute));
            img.Stretch = Stretch.Fill;
            img.Width = 160;
            img.Height = 160;
            img.Source = imgBrush;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetColumn(img, 0);
            grid.Children.Add(img);

            StackPanel info = new StackPanel();
            info.Orientation = Orientation.Vertical;
            info.Children.Add(new Label() { Content = "距离(m)", Height = 25, HorizontalAlignment = HorizontalAlignment.Right });
            info.Children.Add(new Label() { Content = "缺陷名称代码", Height = 25, HorizontalAlignment = HorizontalAlignment.Right });
            info.Children.Add(new Label() { Content = "分值", Height = 25, HorizontalAlignment = HorizontalAlignment.Right });
            info.Children.Add(new Label() { Content = "等级", Height = 25, HorizontalAlignment = HorizontalAlignment.Right });
            info.Children.Add(new Label() { Content = "内部状况描述", Height = 25, HorizontalAlignment = HorizontalAlignment.Right });
            info.Children.Add(new Label() { Content = "照片详情", Height = 25, HorizontalAlignment = HorizontalAlignment.Right });
            Grid.SetColumn(info, 1);
            grid.Children.Add(info);
            StackPanel info2 = new StackPanel();
            info2.Orientation = Orientation.Vertical;
            info2.Children.Add(new TextBox() { Width = 150, Height = 25 });
            info2.Children.Add(new TextBox() { Width = 150, Height = 25 });
            info2.Children.Add(new TextBox() { Width = 150, Height = 25 });
            info2.Children.Add(new TextBox() { Width = 150, Height = 25 });
            info2.Children.Add(new TextBox() { Width = 150, Height = 25 });
            info2.Children.Add(new TextBox() { Width = 150, Height = 25 });
            Grid.SetColumn(info2, 2);
            grid.Children.Add(info2);
            pictureInfo.Children.Add(grid);
        }

        //保存数据到数据库
        private void Save(object sender, RoutedEventArgs e)
        {
            PipingDetailInfo detailInfo = new PipingDetailInfo();
            string pipingID = pipingId ?? string.Format("{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));


            List<PipingPictureInfo> pictInfoList = new List<PipingPictureInfo>();
            PipingDetectionInfo detectionInfo = new PipingDetectionInfo();

            // 加载图片信息
            for (int i = 1; i <= pictureInfo.Children.Count; i++)
            {
                StackPanel infos = (StackPanel)((Grid)pictureInfo.Children[i - 1]).Children[2];

                PipingPictureInfo pictInfo = new PipingPictureInfo();
                pictInfo.PipingID = pipingID;
                pictInfo.PictureID = i.ToString();
                pictInfo.Grade = ((TextBox)infos.Children[3]).Text.Trim();
                pictInfo.Score = ((TextBox)infos.Children[2]).Text.Trim();
                pictInfo.PipingInternalState = ((TextBox)infos.Children[4]).Text;
                pictInfo.PictureRemark = ((TextBox)infos.Children[5]).Text;
                pictInfo.PictureType = 1;
                pictInfo.DefectCode = ((TextBox)infos.Children[1]).Text;
                pictInfo.Distance = ((TextBox)infos.Children[0]).Text;
                pictInfo.Remark = remark.Text;
                pictInfo.PictureFilePath = imgList[i - 1];
                pictInfo.IsDelete = 0;
                pictInfo.IsEnable = 1;
                pictInfo.AddPerson = MainWindow.loginUser.UserName;
                pictInfo.AddTime = System.DateTime.Now;
                pictInfo.KeyID = Guid.NewGuid().ToString("N");


                pictInfoList.Add(pictInfo);
            }

            if (!string.IsNullOrEmpty(drawing.Text))
            {
                // 工程图纸信息
                PipingPictureInfo gcPictInfo = new PipingPictureInfo();
                gcPictInfo.PictureID = string.Format("0");
                gcPictInfo.PictureFilePath = drawing.Text;
                gcPictInfo.PictureRemark = "工程图纸";
                gcPictInfo.AddPerson = MainWindow.loginUser.UserName;
                gcPictInfo.PipingID = pipingID;
                gcPictInfo.PictureType = 0;
                gcPictInfo.KeyID = Guid.NewGuid().ToString("N");
                pictInfoList.Add(gcPictInfo);
            }

            // 管道检测信息
            detectionInfo.PipingID = pipingID;
            detectionInfo.VideoFile = TextBox1.Text;
            detectionInfo.LayingYear = TextBox2.Text;
            detectionInfo.TubulationType = TextBox3.Text;
            detectionInfo.DetectionDirect = TextBox4.Text;
            detectionInfo.DetectionAddress = TextBox5.Text;
            detectionInfo.StartWellNo = TextBox6.Text;
            detectionInfo.StartPointDepth = TextBox7.Text;
            detectionInfo.TubulationMaterial = TextBox8.Text;
            detectionInfo.TubulationLength = TextBox9.Text;
            detectionInfo.DetectionFun = TextBox10.Text;
            detectionInfo.EndWellNo = TextBox11.Text;
            detectionInfo.EndPointDepth = TextBox12.Text;
            detectionInfo.TubulationDiameter = TextBox13.Text;
            detectionInfo.DetectionLength = TextBox14.Text;
            detectionInfo.DetectionTime = TextBox15.Text;
            detectionInfo.AddPerson = MainWindow.loginUser.UserName;
            detectionInfo.AddTime = System.DateTime.Now;
            detectionInfo.IsDelete = 0;
            detectionInfo.IsEnable = 1;
            detectionInfo.KeyID = Guid.NewGuid().ToString("N");
            ResponseMessage result = new ResponseMessage();


            if (null != pictInfoList.Find(p => !string.IsNullOrEmpty(p.PictureFilePath)))
            {
                foreach (var i in pictInfoList)
                {
                    detailInfo.PipingPictureInfoes.Add(i);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Format("数据录入失败，数据格式错误！"));
                return;
            }

            detailInfo.PipingDetectionInfo = detectionInfo;   
            result = service.AddOrModify(detailInfo);
            if (result.code == "0")
            {
                System.Windows.Forms.MessageBox.Show(string.Format("数据录入成功！"));
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Format("数据录入失败，数据格式错误！"));
            }
        }

        //加载图纸连接
        private void LoadDrawing(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.jpg)|*.jpg"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                drawing.Text = openFileDialog.FileName;
            }
        }

        //选择excel
        private void LoadExcel(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xls，*.xlsx)|*.xls;*.xlsx"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    LoadBasicData(Read(openFileDialog.FileName));
                }
                catch
                {
                    MessageBox.Show("请检查文件格式！");
                }
            }
        }

        //excel 转datatable
        public DataTable Read(string file)
        {
            string ConnectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + file + ";" +
                    "Extended Properties='Excel 8.0;HDR=no;IMEX=1';";
            var con = new System.Data.OleDb.OleDbConnection(ConnectionString);
            try
            {
                con.Open();
                var tables = con.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { });
                con.Close();
                if (tables.Rows.Count == 0)
                {
                    throw new Exception("Excel必须包含一个表");
                }
                var firstTableName = tables.Rows[0]["TABLE_NAME"].ToString();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("select * from [" + firstTableName + "] ", con);
                System.Data.OleDb.OleDbDataAdapter apt = new System.Data.OleDb.OleDbDataAdapter(cmd);
                var dt = new System.Data.DataTable();
                apt.Fill(dt);
                if (dt.Rows.Count < 2)
                { throw new Exception("表必须包含数据"); }
                var headRow = dt.Rows[1];
                foreach (DataColumn c in dt.Columns)
                {
                    var headValue = (headRow[c.ColumnName] == DBNull.Value || headRow[c.ColumnName] == null) ? "" : headRow[c.ColumnName].ToString().Trim();
                    if (headValue.Length == 0)
                    {
                        throw new Exception("必须输入列标题");
                    }

                    if (dt.Columns.Count != 15)
                    {
                        throw new Exception("不符合规范的表格：" + headValue);
                    }
                    c.ColumnName = headValue;
                }
                dt.Rows.RemoveAt(0);
                return dt;
            }
            catch (Exception ee)
            { throw ee; }
            finally
            { con.Close(); }
        }

        //将excel数据写入界面
        private void LoadBasicData(DataTable table)
        {
            TextBox1.Text = table.Columns[0].ToString();
            TextBox2.Text = table.Columns[1].ToString();
            TextBox3.Text = table.Columns[2].ToString();
            TextBox4.Text = table.Columns[3].ToString();
            TextBox5.Text = table.Columns[4].ToString();
            TextBox6.Text = table.Columns[5].ToString();
            TextBox7.Text = table.Columns[6].ToString();
            TextBox8.Text = table.Columns[7].ToString();
            TextBox9.Text = table.Columns[8].ToString();
            TextBox10.Text = table.Columns[9].ToString();
            TextBox11.Text = table.Columns[10].ToString();
            TextBox12.Text = table.Columns[11].ToString();
            TextBox13.Text = table.Columns[12].ToString();
            TextBox14.Text = table.Columns[13].ToString();
            TextBox15.Text = table.Columns[14].ToString();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearData(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            pictureInfo.Children.Clear();
            drawing.Text = "";
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox11.Text = "";
            TextBox12.Text = "";
            TextBox13.Text = "";
            TextBox14.Text = "";
            TextBox15.Text = "";
            imgList.Clear();
        }

        private void ClearImg(object sender, RoutedEventArgs e)
        {
            pictureInfo.Children.Clear();
            imgList.Clear();
        }
    }
}
