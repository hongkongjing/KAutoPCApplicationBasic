using KAutoPCApplicationBasic.Model;
using KAutoPCApplicationBasic.Util;
using KAutoPCApplicationBasic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KAutoPCApplicationBasic.ViewModel
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        #region Bien

        public ScreenCapture ScreenCapture { get; set; } = new ScreenCapture();
        public string? headerTitle = "đây là title";
        private string bottomStatus;

        public string BottomStatus
        {
            get { return bottomStatus; }
            set { bottomStatus = value; OnPropertyChanged("BottomStatus"); }
        }
        private ObservableCollection<DevicesInfo> listDevices;

        public ObservableCollection<DevicesInfo> ListDevices
        {
            get { return listDevices; }
            set { listDevices = value;OnPropertyChanged("ListDevices"); }
        }
        // selected list
        //public DevicesInfo DevicesSelected = new DevicesInfo();
        private DevicesInfo deviceSelected;

        public DevicesInfo DeviceSelected
        {
            get { return deviceSelected; }
            set { deviceSelected = value; OnPropertyChanged("DeviceSelected"); }
        }

        public LDPlayer LDControler = new LDPlayer();
        public List<PhoneModel> ListPhones = new List<PhoneModel>();
        #endregion
        // chup anh
        
        public void HandleClick()
        { 
            var task1 = WindowHandlerHelper.ControlLClickAsync((IntPtr)2035230, new System.Drawing.Point(55, 100));
            task1.Wait();
        }

        // Get list không cần ADB
        public void GetDevice3()
        {
            ListDevices = new ObservableCollection<DevicesInfo>();
            // get list handle với dnplayer
            var listproc = ScreenCapture.GetWindowHandles("dnplayer");
            // get list device dang open bang len cua LD
            var list = LDControler.GetDevices3_Running();
            foreach (var item in list)
            {
                ListDevices.Add(new DevicesInfo(item, listproc.FirstOrDefault(x => x.Value == item.name).Key));
            }
        }

        //Get Device with ADB
        //public void GetDevice2()
        //{
        //    MessageBox.Show("đã vào Get Device 2");
        //    ListDevices = new ObservableCollection<DevicesInfo>();
        //    var listproc = ScreenCapture.GetWindowHandles("dnplayer");
        //    MessageBox.Show("Get List Window handles " + listproc.Count.ToString());
        //    var listadb = ADBHelper.GetDevices();
        //    MessageBox.Show("adb devices " + listadb.Count.ToString());

        //    var list = LDControler.GetDevices2_Running();
        //    MessageBox.Show("Get List device list2 "+list.Count.ToString());
        //    foreach (var item in list)
        //    {
        //        ListDevices.Add(new DevicesInfo(item, listproc.FirstOrDefault(x => x.Value == item.name).Key));

        //    }
        //    MessageBox.Show(ListDevices.Count().ToString());
        //}
        
        public void ScreenShotOne()
        {
            CaptureScreenAsync(DeviceSelected.HandleWindow);
        }
        public void ScreenShotAll()
        {
            List<IntPtr> source = new List<IntPtr>();
            // Check null property
            if (ListDevices == null) return;
            if (ListDevices.Count == 0) return;
            foreach (DevicesInfo device in ListDevices) { source.Add(device.HandleWindow); }
            foreach (var item in source)
            {
                Task.Run(() => { CaptureScreenAsync(item); });
            }
            
        }
        private async void CaptureScreenAsync(IntPtr hwnd)
        {
            await Task.Run( () =>
            {
                BottomStatus = "Đang chụp ảnh";
                // Không dùng ADB nữa
                //var MyImage = ADBHelper.ScreenShoot(name, true);
                var imageHw = ScreenCapture.GetScreenshot(hwnd);
                if (imageHw != null)
                {
                    VisionHelper.SaveImage(imageHw, hwnd.ToString());
                }
                else
                {
                    BottomStatus = "Không Chụp Được ảnh";
                }
                    
            });
        }
        public MainViewModel()
        {
            ListDevices = new ObservableCollection<DevicesInfo>();
            DeviceSelected = new DevicesInfo();
        }
        public  void StartAuto()
        {

            //var listproce = ScreenCapture.GetAllWindowHandleNames();
            //var nameProc = ScreenCapture.GetWindowHandle("dnplayer");
            //var image = ScreenCapture.GetScreenshot(nameProc);
            //var title = WindowHandlerHelper.GetWindowTitle(nameProc);
            //ScreenCapture.WriteBitmapToFile("C:\\Users\\Bon\\Desktop\\Result\\captureimage1.jpg", image);


            ListPhones = new List<PhoneModel>();
            foreach (var item in ListDevices)
            {
                ListPhones.Add(new PhoneModel(item));
            }

            Parallel.ForEach(ListPhones, phone => { phone.Dowork(); });
        }
        #region PropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion PropertyChange
    }
}
