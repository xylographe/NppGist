﻿using NppNetInf;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NppGist.Forms
{
    public partial class dlgAuthorization : Form
    {
        bool closeDialog = true;

        public dlgAuthorization()
        {
            InitializeComponent();
        }

        private void linkGetAccessToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool error = false;
            User user = null;
            try
            {
                var response = Utils.SendRequest($"{Main.ApiUrl}/user?access_token={tbAccessToken.Text}", out var responseHeaders);
                user = JsonSerializer.DeserializeFromString<User>(response);
                if (!responseHeaders.TryGetValue("X-OAuth-Scopes", out var scopes) || !scopes.Contains("gist"))
                {
                    MessageBox.Show("Entered access token does not contains gist scopes");
                    error = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to send access token: " + ex.Message);
                error = true;
            }

            if (!error)
            {
                Main.Login = user.Login;
                Main.Token = tbAccessToken.Text;
                Win32.WritePrivateProfileString("Settings", "Login", Main.Login, Main.IniFileName);
                Win32.WritePrivateProfileString("Settings", "AccessToken", AccessToken.EncryptToken(Main.Token), Main.IniFileName);
                closeDialog = true;
            }
            else
                closeDialog = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeDialog = true;
        }

        private void frmAuthorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None && !closeDialog)
                e.Cancel = true;
        }
    }
}