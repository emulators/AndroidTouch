using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace m8916touch
{
    public class TextPutter : ITextPutter
    {
        Char[] splitter = { ' ', '`', '&', '"', '<', '>', ';', '|', '*', '\'', '^', '#', '(', ')', '\\' };
        Form1 mForm1;
        string[] splitted;
        string mStringToSend;

        public TextPutter(Form1 reference)
        {
            mForm1 = reference;
        }

        //TODO: experimental, delete after finish
        public void putText2()
        {
            //makeBatchFileAndRun("");
            string str = "abcde";
            str = str.Insert(2, "xy");
            mForm1.set_txtbox_debugmessage("abcde insert xy at index 2 = "+str);
        }
        
        bool ITextPutter.putText(string textToPut)
        {
            mStringToSend = textToPut;
            bool containsIllegalCharacter = checkIllegalCharacter(textToPut);
            if(false)// ( (!containsIllegalCharacter) && mForm1!=null )
            {
                // send "adb shell input text @textToPut"
                Process adbProcess = new Process();
                mForm1.setProcessStartInfo(adbProcess, "shell input text " + textToPut);
                adbProcess.Start();
                adbProcess.WaitForExit();
                return true;
            }

            splitted = mStringToSend.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            // experiment
            StringBuilder sb = new StringBuilder("str to send: '");
            foreach (string str in splitted)
            {
                sb.Append(str);
                sb.Append(" - ");
            }
            sb.Append("'");
            mForm1.set_txtbox_debugmessage(sb.ToString());

            return false;
        } // putText()

        private bool checkIllegalCharacter(string text)
        {
            /*
             * characters not supported directly: 
               ` & " < > ; |
             * 
             */
            //TODO: identify the unsupported characters and break them into 
            //      separate strings and ADB cmds then do the sequential input. 
            foreach (char c in text)
            {
                switch (c)
                {
                    case '`':
                        
                        break;
                    case '&':
                        
                        break;
                    case '"':

                        break;
                    case '<':
                        
                        break;
                    case '>':
                        
                        break;
                    case ';':
                        
                        break;
                    case '|':
                        
                        break;
                    case ' ':
                        break;
                    default:
                        return false;
                }
            }
            return true;
        } // checkIllegalCharacter ()

        public void sendSpecialChar(char charToDevice)
        {
            int key_code=0;
            switch (charToDevice)
            {
                case '`': 
                    sendRegularString("\"\\`\"");
                    return;
                case '&':
                    sendRegularString("\"\\&\"");
                    return;
                case '"':
                    sendRegularString("'\\\"'");
                    return;
                case '<':
                    sendRegularString("\"\\<\"");
                    return;
                case '>':
                    sendRegularString("\"\\>\"");
                    return;
                case '|':
                    sendRegularString("\"\\|\"");
                    return;
                case '^':
                    sendRegularString("'^'");
                    return;
                case '(':
                    sendRegularString("'('");
                    return;
                case ')':
                    sendRegularString("')'");
                    return;
                case '\\':
                    sendRegularString("'\\'");
                    return;
                case ' ':
                    key_code = 62;
                    break;
                case '\'':
                    key_code = 75;
                    break;
                case '*':
                    key_code = 17;
                    break;
                case '#':
                    key_code = 18;
                    break;
                case ';':
                    key_code = 74;
                    break;
            }
            StringBuilder sb = new StringBuilder("shell input keyevent ");
            Process adbProcess = new Process();
            sb.Append(key_code);
            mForm1.setProcessStartInfo(adbProcess, sb.ToString());
            adbProcess.Start();
            adbProcess.WaitForExit();
        } // sendSpecialChar ()


        public void sendRegularString(string strToDevice)
        {
            Process adbProcess = new Process();
            mForm1.setProcessStartInfo(adbProcess, "shell input text " + strToDevice);
            mForm1.set_txtbox_debugmessage("sending  adb shell input text " +strToDevice);
            adbProcess.Start();
            adbProcess.WaitForExit();
        } // sendRegularString ()


        //public void 


        public void makeBatchFileAndRun(string param)
        {
            string FilePath = System.IO.Directory.GetCurrentDirectory();
            string fileName = FilePath + "\\requiredFiles\\sendChar.bat";
            Process process = new Process();
            string adb_exec = AppConst.FolderNameForADB + "\\sendChar.bat";
            process.StartInfo.FileName = adb_exec;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
        }


    } // class TextPutter
}
