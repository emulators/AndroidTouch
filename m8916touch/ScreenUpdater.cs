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
    public class ScreenUpdater
    {
        public PictureBox pbox;
        public Form1 targetForm = null;
        private static bool nowBusyProcessing = false;
        public ScreenUpdater(PictureBox inp)
        {
            pbox = inp;
        }

        public ScreenUpdater(PictureBox inp, Form1 inputForm)
        {
            pbox = inp;
            targetForm = inputForm;
        }

        public void getScreenshot()
        {
            //string adb_exec = AppConst.FolderNameForADB/ + "\\" + AppConst.resourceName[0];
            string adb_exec =  AppConst.resourceName[0];

            // If an Updater thread is not finished yet when a new Updater thread is created and trying to 
            // access the same file, it will skip the part below due to nowBusyProcessing flag is true. 
            //
            if (!nowBusyProcessing)
            {
                nowBusyProcessing = true;
                var detectOrientationProcess = new Process();
                //detectOrientationProcess.StartInfo.FileName = AppConst.FolderNameForADB + "\\" + AppConst.resourceName[0];
                detectOrientationProcess.StartInfo.FileName =  AppConst.resourceName[0];
                detectOrientationProcess.StartInfo.Arguments = @"shell ""dumpsys input | grep SurfaceOrientation""";
                detectOrientationProcess.StartInfo.RedirectStandardOutput = true;
                detectOrientationProcess.StartInfo.RedirectStandardError = true;
                detectOrientationProcess.StartInfo.UseShellExecute = false;
                detectOrientationProcess.StartInfo.CreateNoWindow = true;
                detectOrientationProcess.Start();
                string output = detectOrientationProcess.StandardOutput.ReadToEnd();
                detectOrientationProcess.WaitForExit();
                int orient = 0;
                foreach (char C in output) { if (C == '0' || C == '1' || C == '2' || C == '3')orient = int.Parse(C.ToString()); }

                var downloadScreenshotProcess = new Process();
                var captureScreenshotProcess = new Process();

                captureScreenshotProcess.StartInfo.FileName = adb_exec;
                captureScreenshotProcess.StartInfo.Arguments = @"shell /system/bin/screencap -p /sdcard/screenshot.png";
                captureScreenshotProcess.StartInfo.UseShellExecute = false;
                captureScreenshotProcess.StartInfo.RedirectStandardError = true;
                captureScreenshotProcess.StartInfo.RedirectStandardInput = true;
                captureScreenshotProcess.StartInfo.RedirectStandardOutput = true;
                captureScreenshotProcess.StartInfo.CreateNoWindow = true;
                captureScreenshotProcess.Start();
                captureScreenshotProcess.WaitForExit();

                downloadScreenshotProcess.StartInfo.FileName = adb_exec;
                downloadScreenshotProcess.StartInfo.Arguments = @"pull /sdcard/screenshot.png screenshot.png";
                downloadScreenshotProcess.StartInfo.RedirectStandardError = true;
                downloadScreenshotProcess.StartInfo.RedirectStandardInput = true;
                downloadScreenshotProcess.StartInfo.RedirectStandardOutput = true;
                downloadScreenshotProcess.StartInfo.UseShellExecute = false;
                downloadScreenshotProcess.StartInfo.CreateNoWindow = true;
                downloadScreenshotProcess.Start();
                downloadScreenshotProcess.WaitForExit();
                if (System.IO.File.Exists(AppConst.ScreenshotFileName))
                {
                    try
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(AppConst.ScreenshotFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                        switch (orient)
                        {
                            case 3:
                                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                targetForm.Invoke(targetForm.md, orient);
                                break;
                            case 1:
                                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                targetForm.Invoke(targetForm.md, orient);
                                break;
                            case 0:
                                targetForm.Invoke(targetForm.md, orient);
                                break;
                        }
                        pbox.Image = img;
                        fs.Close();
                    }
                    catch (Exception e)
                    {
                        if (targetForm == null)
                        {
                            EventLog.Write("targetForm = null" + e.ToString());
                        }
                        if (targetForm.md == null)
                        {
                            EventLog.Write("targetForm.md" + e.ToString());
                        }
                        EventLog.Write("ScreenUpdater:getScreenshot() " + e.ToString());
                    }
                }

                nowBusyProcessing = false;
            }/* if (!nowBusyProcessing) */

        }/*public void getScreenshot()*/
    }/*public class ScreenUpdater*/
}
