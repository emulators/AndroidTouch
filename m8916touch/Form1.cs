using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace m8916touch
{
    public partial class Form1 : Form
    {
        // landscape: (w,h)=846, 606
        // portrait:  (w,h)=496, 993
        int XmouseDown;
        int YmouseDown;
        int XmouseUp;
        int YmouseUp;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        static double scaleFactorH = 0.0;
        static double scaleFactorW = 0.0;
        static int screenshotW=0, screenshotH=0;
        static int screenW = 0, screenH = 0;
        public delegate void myDelegate(int inputOrientation);
        public myDelegate md;
        int orientation =0;
        

        public Form1()
        {
            InitializeComponent();
            settingUp();
        } /* Form1() constructor */

        private void settingUp()
        {
            text_debugmessage.Text = "Starting application....";
            text_debugmessage.ReadOnly = true;
            pbox_screen.SizeMode = PictureBoxSizeMode.StretchImage;
            pbox_screen.Width = 450;
            pbox_screen.Height = 800;
            md = new myDelegate(rotateScreenshot);

            this.pbox_screen.MouseDown += new MouseEventHandler(pbox_screen_MouseDown);
            this.pbox_screen.MouseUp += new MouseEventHandler(pbox_screen_MouseUp);
            chkbox_touch.Checked = true;
            screenW = pbox_screen.Width;
            screenH = pbox_screen.Height;
            timer_updateScreen.Interval = 2 * 1000;
            this.Load += new EventHandler(onFormLoaded);
            this.Shown += new EventHandler(initScreen);
            this.FormClosing += new FormClosingEventHandler(onFormClosing);
            if (System.IO.File.Exists("screenshot.png"))
            {
                System.IO.File.Delete("screenshot.png");
            }
        } /* settingUp() */

        public void initScreen(object sender, EventArgs e)
        {
            Thread.Sleep(200);
            initScreen();
        }

        public void onFormClosing(object sender, EventArgs e)
        {
            // Delete the resource files extracted from the tool
            /*
            if (System.IO.Directory.Exists(AppConst.FolderNameForADB))
            {
                try
                {
                    System.IO.DirectoryInfo folderInfo = new System.IO.DirectoryInfo(AppConst.FolderNameForADB);
                    foreach (System.IO.FileInfo file in folderInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    System.IO.Directory.Delete(AppConst.FolderNameForADB);
                }
                catch (Exception ee)
                {
                    EventLog.Write("onFormClosing(): "+ee.ToString() );
                }
            }
             */
        }//  onFormClosing(object sender, EventArgs e)


        public void onFormLoaded(object sender, EventArgs e)
        {
            // Create the folder for placing the ADB
            /*
            if (!System.IO.Directory.Exists(AppConst.FolderNameForADB))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(AppConst.FolderNameForADB);
                }
                catch (Exception ee)
                {
                    text_debugmessage.Text = "onFormLoaded: "+ee.ToString();
                }
            }
            // Extract the resource file (adb.exe and its required DLLs) for usage
            for (int resourceIndex = 0; resourceIndex < AppConst.resourceName.Length; resourceIndex++)
            {
                string targetPath = AppConst.FolderNameForADB + "\\" + AppConst.resourceName[resourceIndex];
                ExtractResource(AppConst.resourceName[resourceIndex], targetPath);
            }
            */
        }/* onFormLoaded(object sender, EventArgs e)*/

        public void initScreen()
        {
            // get device orientation
            try
            {
                var detectOrientationProcess = new Process();
                setProcessStartInfo(detectOrientationProcess, @"shell ""dumpsys input | grep SurfaceOrientation""");
                detectOrientationProcess.Start();
                string output = detectOrientationProcess.StandardOutput.ReadToEnd();
                detectOrientationProcess.WaitForExit();
                foreach (char C in output) { if (C == '0' || C == '1' || C == '2' || C == '3')orientation = short.Parse(C.ToString()); }



                var downloadScreenshotProcess = new Process();
                var captureScreenshotProcess = new Process();

                setProcessStartInfo(captureScreenshotProcess, @"shell /system/bin/screencap -p /sdcard/screenshot.png");
                captureScreenshotProcess.Start();
                captureScreenshotProcess.WaitForExit();

                setProcessStartInfo(downloadScreenshotProcess, @"pull /sdcard/screenshot.png screenshot.png");
                downloadScreenshotProcess.Start();
                downloadScreenshotProcess.WaitForExit();
            }
            catch (Exception e)
            {
                set_txtbox_debugmessage("initScreen(): exception, did you place adb.exe at the same path?");
                return;
            }
            if (System.IO.File.Exists(AppConst.ScreenshotFileName))
            {
                try
                {
                    System.IO.FileStream fs = new System.IO.FileStream(AppConst.ScreenshotFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    calculateScaleFactor(fs);

                    System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                    switch (orientation)
                    {
                        case 3:
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 1:
                            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                        case 0: 
                            break;
                    }
                    rotateScreenshot(orientation);
                    pbox_screen.Image = img;
                    fs.Close();
                }
                catch (Exception e)
                {
                    System.Text.StringBuilder eMsg = new System.Text.StringBuilder();
                    eMsg.Append("calculateScaleFactor: ");
                    eMsg.Append(e.ToString());
                    EventLog.Write(eMsg.ToString());
                    text_debugmessage.Text = e.ToString() + "@ initScreen()";
                }
                text_debugmessage.Text = "Application started successfully.";
            } /* if (System.IO.File.Exists("screenshot.png")) */
            else
            {
                text_debugmessage.Text = "Device not online. Connect device via USB and power on it.";
            }
        } /* public void initScreen() */

        

        private void chkbox_autorefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_autorefresh.Checked)
                timer_updateScreen.Start();
            else
                timer_updateScreen.Stop();
        }

        public void inClassUpdateScreen(int everyMilliSecond)
        {
            System.Threading.Thread.Sleep(everyMilliSecond);

            var downloadScreenshotProcess = new Process();
            var captureScreenshotProcess = new Process();
            var detectOrientationProcess = new Process();
            setProcessStartInfo(detectOrientationProcess, @"shell ""dumpsys input | grep SurfaceOrientation""");
            detectOrientationProcess.Start();
            string output = detectOrientationProcess.StandardOutput.ReadToEnd();
            detectOrientationProcess.WaitForExit();
            int orient = 0;
            foreach (char C in output) { if (C == '0' || C == '1' || C == '2' || C == '3')orient = int.Parse(C.ToString()); }

            setProcessStartInfo(captureScreenshotProcess, @"shell /system/bin/screencap -p /sdcard/screenshot.png");
            captureScreenshotProcess.Start();
            captureScreenshotProcess.WaitForExit();

            setProcessStartInfo(downloadScreenshotProcess, @"pull /sdcard/screenshot.png screenshot.png");
            downloadScreenshotProcess.Start();
            downloadScreenshotProcess.WaitForExit();

            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(AppConst.ScreenshotFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                switch (orient)
                {
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        rotateScreenshot(orient);
                        break;
                    case 1:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        rotateScreenshot(orient);
                        break;
                    case 0:
                        rotateScreenshot(orient);
                        break;
                }
                pbox_screen.Image = img;
                fs.Close();
            }
            catch (Exception ee)
            {
                System.Text.StringBuilder eMsg = new System.Text.StringBuilder();
                eMsg.Append("inClassUpdateScreen: ");
                eMsg.Append(ee.ToString());
                EventLog.Write(eMsg.ToString());
                text_debugmessage.Text = ee.ToString() + "@ inClassUpdateScreen()";
            }
        } /*public void inClassUpdateScreen(int everyMilliSecond)*/


        public void setProcessStartInfo(Process process, string P_argument)
        {
            //string adb_exec = AppConst.FolderNameForADB + "\\" + AppConst.resourceName[0];
            string adb_exec = AppConst.resourceName[0];
            process.StartInfo.FileName = adb_exec;
            process.StartInfo.Arguments = P_argument;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        } // setProcessStartInfo()


        protected void pbox_screen_MouseDown(object sender, EventArgs e)
        {
            MouseEventArgs eM = (MouseEventArgs)e;
            XmouseDown = eM.X;
            YmouseDown = eM.Y;
            sw.Start();
            text_debugmessage.Text = "mouse down at (" + XmouseDown + "," + YmouseDown + ")";
        }/*protected void pbox_screen_MouseDown(object sender, EventArgs e)*/

        protected void pbox_screen_MouseUp(object sender, EventArgs e)
        {
            MouseEventArgs eM = (MouseEventArgs)e;
            XmouseUp = eM.X;
            YmouseUp = eM.Y;
            text_debugmessage.Text = "mouse up at (" + XmouseUp + "," + YmouseUp + ")";
            sw.Stop();
            int click_elapsed = (int)sw.Elapsed.TotalMilliseconds;
            if (XmouseUp == XmouseDown && YmouseDown == YmouseUp)
            {
                if (click_elapsed <= 320)
                {
                    sendTapPosition(XmouseUp, YmouseUp, click_elapsed);
                }
                else
                {
                    sendSwipe((double)XmouseDown, (double)YmouseDown, (double)XmouseUp, (double)YmouseUp, click_elapsed);
                }
            }
            else
            {
                sendSwipe((double)XmouseDown, (double)YmouseDown, (double)XmouseUp, (double)YmouseUp, click_elapsed);
            }
            sw.Reset();
        }// protected void pbox_screen_MouseDown(object sender, EventArgs e)

        public void sendTapPosition(int x, int y, int duration)
        {
            if (chkbox_touch.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                double dX;
                double dY;
                if (orientation == 0)
                {
                    dX = (double)x * scaleFactorW;
                    dY = (double)y * scaleFactorH;
                }
                else
                {
                    dX = (double)x * scaleFactorH;
                    dY = (double)y * scaleFactorW;
                }
                int realX = (int)dX;
                int realY = (int)dY;
                string messagesToShow = String.Format("tapped on ({0},{1})", realX, realY);

                var process = new Process();
                setProcessStartInfo(process, String.Format("shell input tap {0} {1}", realX, realY));
                process.Start();
                process.WaitForExit();

                text_debugmessage.Text = messagesToShow;
                inClassUpdateScreen(AppConst.threadSleepMilliSec);             
                Cursor.Current = Cursors.Default;
            }
        }// public void sendTapPosition(int x, int y)
        
        public void sendSwipe(double startX, double startY, double endX, double endY, int duration)
        {
            if (chkbox_touch.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                int realSX, realSY, realEX, realEY;
                if (orientation == 0)
                {
                    realSX = (int)(startX * scaleFactorW);
                    realSY = (int)(startY * scaleFactorH);
                    realEX = (int)(endX * scaleFactorW);
                    realEY = (int)(endY * scaleFactorH);
                }
                else
                {
                    realSX = (int)(startX * scaleFactorH);
                    realSY = (int)(startY * scaleFactorW);
                    realEX = (int)(endX * scaleFactorH);
                    realEY = (int)(endY * scaleFactorW);
                }
                string messagesToShow = String.Format("swiped from ({0},{1}) to ({2},{3})", realSX, realSY, realEX, realEY);

                var process = new Process();
                setProcessStartInfo(process, String.Format("shell input swipe {0} {1} {2} {3}", realSX, realSY, realEX, realEY) );
                process.Start();
                process.WaitForExit();
                text_debugmessage.Text = messagesToShow;

                inClassUpdateScreen(AppConst.threadSleepMilliSec);  
                Cursor.Current = Cursors.Default;
            }
        }// public void sendSwipe(double startX, double startY, double endX, double endY)


        private void updateScreen()
        {
            ScreenUpdater updater = new ScreenUpdater(pbox_screen,this);
            Thread updateThread = new Thread(new ThreadStart(updater.getScreenshot) );
            updateThread.Name = "updateScreen";
            updateThread.IsBackground = true;
            updateThread.Start();
        } // updateScreen

        private void timer_updateScreen_Tick(object sender, EventArgs e)
        {
            updateScreen();
        }

        

        public static void calculateScaleFactor(System.IO.FileStream fs)
        {
            try
            {
                screenshotW = System.Drawing.Image.FromStream(fs).Width;
                screenshotH = System.Drawing.Image.FromStream(fs).Height;
                scaleFactorH = (double)screenshotH / (double)screenH;
                scaleFactorW = (double)screenshotW / (double)screenW;
            }
            catch (Exception e)
            {
                System.Text.StringBuilder eMsg = new System.Text.StringBuilder();
                eMsg.Append("calculateScaleFactor: ");
                eMsg.Append(e.ToString());
                EventLog.Write(eMsg.ToString());
            }
        } // calculateScaleFactor 

        public void rotateScreenshot(int inputOrientation)
        {
            orientation = inputOrientation;
            if (3 == orientation)
            {
                int w = pbox_screen.Width, h = pbox_screen.Height;
                if (w < h)
                    pbox_screen.Size = new Size(pbox_screen.Height, pbox_screen.Width);
                else
                    pbox_screen.Size = new Size(pbox_screen.Width, pbox_screen.Height);
                this.Size = new System.Drawing.Size(846, 606);
            }
            else if (1 == orientation)
            {
                int w = pbox_screen.Width, h = pbox_screen.Height;
                if (w < h)
                    pbox_screen.Size = new Size(pbox_screen.Height, pbox_screen.Width);
                else
                    pbox_screen.Size = new Size(pbox_screen.Width, pbox_screen.Height);
                this.Size = new System.Drawing.Size(846, 606);
            }
            else
            {
                int w = pbox_screen.Width, h = pbox_screen.Height;
                if (w > h)
                    pbox_screen.Size = new Size(pbox_screen.Height, pbox_screen.Width);
                else
                    pbox_screen.Size = new Size(pbox_screen.Width, pbox_screen.Height);
                this.Size = new System.Drawing.Size(496, 993);

            }
        } // void rotateScreenshot() 


        private void btn_back_Click(object sender, EventArgs e)
        {
            if (chkbox_touch.Checked)
            {
                var process = new Process();
                Cursor.Current = Cursors.WaitCursor;
                setProcessStartInfo(process,"shell input keyevent 4");
                process.Start();
                process.WaitForExit(); 
                inClassUpdateScreen(AppConst.threadSleepMilliSec);
                Cursor.Current = Cursors.Default;
            }
        }// private void btn_back_Click(object sender, EventArgs e)

        private void btn_task_Click(object sender, EventArgs e)
        {
            TextPutter tp = new TextPutter(this);
            tp.putText2();
            //Cursor.Current = Cursors.WaitCursor;
            //inClassUpdateScreen(AppConst.threadSleepMilliSec);
            //Cursor.Current = Cursors.Default;
        } // btn_task_Click


        public void ExtractResource(string res, string path)
        {
            try
            {
                System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream stream = _assembly.GetManifestResourceStream("m8916touch." + res);
                byte[] bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                System.IO.File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                EventLog.Write("ExtractResource(): "+e);
                set_txtbox_debugmessage(e+" @ ExtractResource()");
            }
        } // ExtractResource()


        private void btn_home_Click(object sender, EventArgs e)
        {
            if (chkbox_touch.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                var process = new Process();
                setProcessStartInfo(process,"shell input keyevent 3");
                process.Start();
                process.WaitForExit(); 
                inClassUpdateScreen(AppConst.threadSleepMilliSec + 500);
                Cursor.Current = Cursors.Default;
            }
        }// btn_home_Click

        private void btn_appswitch_Click(object sender, EventArgs e)
        {
            if (chkbox_touch.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                var process = new Process();
                setProcessStartInfo(process, "shell input keyevent 187");
                process.Start();
                process.WaitForExit(); 
                inClassUpdateScreen(AppConst.threadSleepMilliSec + 500);
                Cursor.Current = Cursors.Default;
            } // if (chkbox_touch.Checked) 
        }// btn_appswitch_Click


        //@@Arshing+++: send text function is not perfect yet. So disable it.
        private void btn_sendText_Click(object sender, EventArgs e)
        {
            string textToSend = text_input.Text;
            sendTextToDevice(textToSend);
        } // btn_sendText_Click()

        private void sendTextToDevice(string inputStr)
        {
            Cursor.Current = Cursors.WaitCursor;
            ITextPutter putter = new TextPutter(this);
            bool sendOK = putter.putText(inputStr);
            //if (!sendOK) set_txtbox_debugmessage("send not OK");
            Cursor.Current = Cursors.Default;
        } // sendTextToDevice()
        //@@Arshing---: send text function is not perfect yet. So disable it.

        public void set_txtbox_debugmessage(String txt)
        {
            text_debugmessage.Text = txt;
        }

        public string get_txtbox_debugmessage()
        {
            return text_debugmessage.Text;
        }

        private void btn_power_Click(object sender, EventArgs e)
        {
            if (chkbox_touch.Checked)
            {
                Cursor.Current = Cursors.WaitCursor;
                var process = new Process();
                setProcessStartInfo(process, "shell input keyevent 26");
                process.Start();
                process.WaitForExit();
                inClassUpdateScreen(AppConst.threadSleepMilliSec);
                Cursor.Current = Cursors.Default;
            }
        } // btn_power_Click()

        private void btn_swipe_Click(object sender, EventArgs e)
        {
            if (chkbox_touch.Checked)
            {
                sendSwipe((double)202, (double)478, (double)202, (double)90, 330);
            }
        } // btn_swipe_Click()

        
    }// public partial class Form1 : Form

}
