﻿using Faculti.Services.FacultiDB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Faculti.UI.Cards
{
    public partial class CommentCard : UserControl
    {
        public string CommentId;
        private Image _commentPicture = Properties.Resources.default_profile;
        private string _commentBody;
        private string _firstName;
        private string _lastName;
        private DateTime _postTime = new DateTime();
        private string _picName;

        public CommentCard(string commentId)
        {
            CommentId = commentId;

            InitializeComponent();
            UpdateData();
        }

        public void UpdateData()
        {
            if (!CommentWorker.IsBusy) CommentWorker.RunWorkerAsync();
        }

        private void CommentWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DatabaseClient client = new DatabaseClient();
                var cmdText = $"select text, user_id, post_time from comments where comment_id = {CommentId}";
                OracleCommand cmd = new OracleCommand(cmdText, client.Conn);
                OracleDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                _commentBody = rdr.GetString(0);
                var authorId = rdr.GetString(1);
                _postTime = rdr.GetOracleDate(2).Value;

                cmdText = $"select first_name, last_name from all_users where user_id = {authorId}";
                cmd = new OracleCommand(cmdText, client.Conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();

                _firstName = rdr.GetString(0);
                _lastName = rdr.GetString(1);

                cmdText = $"select picture, pic_name from all_users where first_name = '{_firstName}' and last_name = '{_lastName}'";
                cmd = new OracleCommand(cmdText, client.Conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    var picName = rdr.IsDBNull(1) ? null : rdr.GetString(1);
                    if (picName != _picName)
                    {
                        byte[] image = rdr.IsDBNull(0) ? null : (byte[])rdr["picture"];
                        if (image != null)
                        {
                            MemoryStream ms = new MemoryStream(image);
                            Image pic = Image.FromStream(ms);
                            _commentPicture = pic;
                        }

                        _picName = picName;
                    }
                }

                client.Close();
            }
            catch (Exception)
            {
                e.Cancel = true;
            }
        }

        private void CommentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                DisplayCommentInfo();
            }
            else
            {
                CommentWorker.RunWorkerAsync();
            }
        }

        public void DisplayCommentInfo()
        {
            CommentPictureBox.Image = _commentPicture;
            CommentAuthorLabel.Text = $"{_firstName} {_lastName}";
            CommentBodyLabel.Text = $"{_commentBody}";
            CommentContainer.Height = CommentBodyLabel.Height + 40;

            TimeSpan lastSpan = DateTime.Now - _postTime;
            double lastUpdate = lastSpan.TotalSeconds / 60;
            string lastUpdateString;

            if (lastUpdate >= 1440)
            {
                lastUpdate /= 1440;
                lastUpdateString = $"{Convert.ToInt32(lastUpdate)}d";
            }
            else if (lastUpdate >= 60)
            {
                lastUpdate /= 60;
                lastUpdateString = $"{Convert.ToInt32(lastUpdate)}h";
            }
            else if (Convert.ToInt32(lastUpdate) == 0)
            {
                lastUpdateString = "Now";
            }
            else
            {
                lastUpdateString = $"{Convert.ToInt32(lastUpdate)}m";
            }

            TimeLabel.Text = lastUpdateString;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!CommentWorker.IsBusy) CommentWorker.RunWorkerAsync();
        }
    }
}
