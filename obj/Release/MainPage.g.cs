﻿#pragma checksum "C:\Users\hyp3rs0nik\Documents\Visual Studio 2010\Projects\EZTVMetro\EZTVMetro\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "33FACA3C6606F5ACA912B5D05CE83E0C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace EZTVMetro {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot pivot;
        
        internal System.Windows.Controls.ListBox listings;
        
        internal System.Windows.Controls.ListBox downloads;
        
        internal Microsoft.Phone.Controls.PhoneTextBox filter;
        
        internal System.Windows.Controls.Button show_filter;
        
        internal System.Windows.Controls.ListBox shows;
        
        internal System.Windows.Controls.ListBox clients;
        
        internal Microsoft.Phone.Controls.PhoneTextBox client_add_name;
        
        internal Microsoft.Phone.Controls.ListPicker clientAddClientType;
        
        internal System.Windows.Controls.TextBlock ClientSelection;
        
        internal Microsoft.Phone.Controls.PhoneTextBox client_add_host;
        
        internal Microsoft.Phone.Controls.PhoneTextBox client_add_port;
        
        internal Microsoft.Phone.Controls.PhoneTextBox client_add_username;
        
        internal Microsoft.Phone.Controls.PhoneTextBox client_add_password;
        
        internal System.Windows.Controls.CheckBox client_add_auth;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/EZTVMetro;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pivot")));
            this.listings = ((System.Windows.Controls.ListBox)(this.FindName("listings")));
            this.downloads = ((System.Windows.Controls.ListBox)(this.FindName("downloads")));
            this.filter = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("filter")));
            this.show_filter = ((System.Windows.Controls.Button)(this.FindName("show_filter")));
            this.shows = ((System.Windows.Controls.ListBox)(this.FindName("shows")));
            this.clients = ((System.Windows.Controls.ListBox)(this.FindName("clients")));
            this.client_add_name = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("client_add_name")));
            this.clientAddClientType = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("clientAddClientType")));
            this.ClientSelection = ((System.Windows.Controls.TextBlock)(this.FindName("ClientSelection")));
            this.client_add_host = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("client_add_host")));
            this.client_add_port = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("client_add_port")));
            this.client_add_username = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("client_add_username")));
            this.client_add_password = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("client_add_password")));
            this.client_add_auth = ((System.Windows.Controls.CheckBox)(this.FindName("client_add_auth")));
        }
    }
}

