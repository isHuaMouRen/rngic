using Microsoft.Win32;
using Rngic.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rngic.Demo.Windows
{
    /// <summary>
    /// WindowMain.xaml 的交互逻辑
    /// </summary>
    public partial class WindowMain : Window
    {
        private Bitmap? _currentBitmap;

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        private void SetPreview(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            try
            {
                image_preview.Source = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }     
        

        private bool CheckState()
        {
            if (File.Exists(textbox_inputPath.Text) && _currentBitmap != null && int.TryParse(textbox_passwd.Text, out var result))
                if (result.ToString() == textbox_passwd.Text)
                    return true;
            return false;
        }

        public WindowMain()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {

            };

            button_inputBrowse.Click += (s, e) =>
            {
                var dialog = new OpenFileDialog
                {
                    Multiselect = false
                };
                if (dialog.ShowDialog() != true)
                    return;

                textbox_inputPath.Text = dialog.FileName;
                _currentBitmap = new Bitmap(dialog.FileName);
                SetPreview(_currentBitmap);
            };

            textbox_passwd.PreviewTextInput += (s, e) =>
            {
                e.Handled = !e.Text.All(char.IsDigit);
            };


            button_confuse.Click += (s, e) =>
            {
                if (!CheckState())
                {
                    MessageBox.Show("无法执行混淆操作\n\n请检查:\n  传入一个合法的图像文件\n  密码必须为整数", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(textbox_passwd.Text, out var passwd))
                {
                    MessageBox.Show("无法转换类型 string -> int");
                    return;
                }

                var confuser = new ImageConfuser(_currentBitmap!, passwd);
                var result = confuser.Confuse();
                SetPreview(result);
                _currentBitmap = result;
            };

            button_deconfuse.Click += (s, e) =>
            {
                if (!CheckState())
                {
                    MessageBox.Show("无法执行反混淆操作\n\n请检查:\n  传入一个合法的图像文件\n  密码必须为整数", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(textbox_passwd.Text, out var passwd))
                {
                    MessageBox.Show("无法转换类型 string -> int");
                    return;
                }

                var confuser = new ImageConfuser(_currentBitmap!, passwd);
                var result = confuser.Deconfuse();
                SetPreview(result);
                _currentBitmap = result;
            };

            button_save.Click += (s, e) =>
            {
                if (_currentBitmap == null)
                {
                    MessageBox.Show("当前图像为空，无法保存", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dialog = new SaveFileDialog();
                if (dialog.ShowDialog() != true)
                    return;

                _currentBitmap.Save(dialog.FileName);
            };
        }
    }
}
