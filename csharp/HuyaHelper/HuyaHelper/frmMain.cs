﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuyaHelper
{
    public partial class frmMain : Form
    {
        private AtomicInt msg_cnt = new AtomicInt();
        private AtomicInt closing = new AtomicInt();

        private bool isActived = false;

        private delegate void delegateChatMsg(string nickname, string content);
        private delegate void delegateGiftMsg(string nickname, string itemName, int itemCount);

        public frmMain()
        {
            closing.Set(0);
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public void appendChatMsg(string nickname, string content)
        {
            if (this.Handle == null || closing.GetAndAdd(0) == 1)
            {
                return;
            }

            if (chatContent.InvokeRequired)
            {
                delegateChatMsg delegates = new delegateChatMsg(appendChatMsg);
                chatContent.Invoke(delegates, nickname, content);
            }
            else
            {
                int cnt = msg_cnt.Increment();
                if (cnt > 500)
                {
                    chatContent.Clear();
                    chatContent.ClearUndo();
                    msg_cnt.Set(0);
                    GC.Collect();
                }

                if (!isActived && (cnt % 10) == 9)
                {
                    chatContent.Focus();
                    chatContent.Select(chatContent.TextLength, 0);
                    chatContent.ScrollToCaret();
                }

                string text;
                //text = string.Format("[{0}]: {1}\n", nickname, content);
                //text = cnt.ToString() + "\n";
                //chatContent.AppendText(text);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("[");
                
                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Blue;
                chatContent.AppendText(nickname);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                text = string.Format("]: {0}\n", content);
                chatContent.AppendText(text);

                //chatContent.Focus();
            }
        }

        public void appendGiftMsg(string nickname, string itemName, int itemCount)
        {
            if (this.Handle == null || closing.GetAndAdd(0) == 1)
            {
                return;
            }

            if (chatContent.InvokeRequired)
            {
                delegateGiftMsg delegates = new delegateGiftMsg(appendGiftMsg);
                chatContent.Invoke(delegates, nickname, itemName, itemCount);
            }
            else
            {
                int cnt = msg_cnt.Increment();
                if (cnt > 500)
                {
                    chatContent.Clear();
                    chatContent.ClearUndo();
                    msg_cnt.Set(0);
                    GC.Collect();
                }

                if (!isActived && (cnt % 10) == 9)
                {
                    chatContent.Focus();
                    chatContent.Select(chatContent.TextLength, 0);
                    chatContent.ScrollToCaret();
                }

                string text;
                //text = string.Format("[{0}]: {1} x {2}\n",
                //                     nickname, itemName, itemCount);
                //text = cnt.ToString() + "\n";
                //chatContent.AppendText(text);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("[");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Blue;
                chatContent.AppendText(nickname);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("]: ");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Red;
                chatContent.AppendText(itemName);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                text = string.Format(" x {0}\n", itemCount);
                chatContent.AppendText(text);

                //chatContent.Focus();
            }
        }

        /*
        protected override void DefWndProc(ref System.Windows.Forms.Message messsage)
        {
            SimpleNoticeMessage noticeData;
            SimpleGiftMessage giftData;
            switch (messsage.Msg)
            {
                //接收CopyData消息，读取发送过来的数据
                case Messages.WM_COPYDATA:
                    {
                        COPYDATASTRUCT cds = new COPYDATASTRUCT();
                        Type type = cds.GetType();
                        cds = (COPYDATASTRUCT)messsage.GetLParam(type);
                        uint flag = (uint)(cds.dwData);
                        //byte[] data = new byte[cds.cbData];
                        //Marshal.Copy(cds.lpData, data, 0, data.Length);

                        if (flag <= Messages.TypeFlag)
                        {
                            // String
                        }
                        else if (flag == Messages.NoticeType)
                        {
                            // Binary bytes
                            noticeData = new SimpleNoticeMessage();
                            noticeData = (SimpleNoticeMessage)Marshal.PtrToStructure(cds.lpData, typeof(SimpleNoticeMessage));
                            appendChatMsg(noticeData.sendNick, noticeData.content);
                        }
                        else if (flag == Messages.GiftType)
                        {
                            // Binary bytes
                            giftData = new SimpleGiftMessage();
                            giftData = (SimpleGiftMessage)Marshal.PtrToStructure(cds.lpData, typeof(SimpleGiftMessage));
                            appendGiftMsg(giftData.sendNick, giftData.itemName, giftData.sendItemCount);
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref messsage);
                    break;
            }
        }
        //*/

        private void frmMain_Activated(object sender, EventArgs e)
        {
            isActived = true;
        }

        private void frmMain_Deactivate(object sender, EventArgs e)
        {
            isActived = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            int result;
            HuyaChatRoom chatroom = new HuyaChatRoom();
            chatroom.setParent(this);

            isActived = true;

            // Nv wang yan
            result = chatroom.run("626813");

            // La feng long
            //result = chatroom.run("520880");

            // Ka'er
            //result = chatroom.run("521000");

            // Shen tu
            //result = chatroom.run("666007");

            // Dong xiao sa
            //result = chatroom.run("908400");

            // Hou'ge
            //result = chatroom.run("931827");

            // Guozi
            //result = chatroom.run("15382773");
        }
    }
}
