﻿#pragma checksum "..\..\ChatWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F11DAC85B98932FC40A94C5223D2C8040DF3505ABF10AD0D3DBDF5CF8C42A296"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Chat;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Chat {
    
    
    /// <summary>
    /// ChatWindow
    /// </summary>
    public partial class ChatWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MessPanelGrid;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock chatTitle;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollViewerMess;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel messagePlace;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MessPanel;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox messageTextBox;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sentButton;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon PlusButton;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchBox;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listView1;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollViewerListItem;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem listViewItem_1;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush userImg;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock userName;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lastMessage;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border messageBorderCount;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock messageCount;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel InfoPanel;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon winStateImg;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock chatName;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock descriptionInfo;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock aboutMe;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Username;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Phone;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\ChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock mail;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Chat;component/chatwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ChatWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\ChatWindow.xaml"
            ((Chat.ChatWindow)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MessPanelGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.chatTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            
            #line 29 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 5:
            this.scrollViewerMess = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 6:
            this.messagePlace = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.MessPanel = ((System.Windows.Controls.Border)(target));
            
            #line 43 "..\..\ChatWindow.xaml"
            this.MessPanel.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Border_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.messageTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 51 "..\..\ChatWindow.xaml"
            this.messageTextBox.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.MessageTextBox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.sentButton = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\ChatWindow.xaml"
            this.sentButton.Click += new System.Windows.RoutedEventHandler(this.SentButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 68 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_7);
            
            #line default
            #line hidden
            return;
            case 11:
            this.PlusButton = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            return;
            case 12:
            this.searchBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 72 "..\..\ChatWindow.xaml"
            this.searchBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 81 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_6);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 87 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_4);
            
            #line default
            #line hidden
            return;
            case 15:
            this.listView1 = ((System.Windows.Controls.ListView)(target));
            return;
            case 16:
            this.scrollViewerListItem = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 17:
            this.listViewItem_1 = ((System.Windows.Controls.ListViewItem)(target));
            
            #line 96 "..\..\ChatWindow.xaml"
            this.listViewItem_1.Selected += new System.Windows.RoutedEventHandler(this.ListViewItem_1_Selected);
            
            #line default
            #line hidden
            return;
            case 18:
            this.userImg = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 19:
            this.userName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 20:
            this.lastMessage = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 21:
            this.messageBorderCount = ((System.Windows.Controls.Border)(target));
            return;
            case 22:
            this.messageCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 23:
            this.InfoPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 24:
            
            #line 127 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_3);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 130 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.winStateImg = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            return;
            case 27:
            
            #line 133 "..\..\ChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            return;
            case 28:
            this.chatName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 29:
            this.descriptionInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 30:
            this.aboutMe = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 31:
            this.Username = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 32:
            this.Phone = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 33:
            this.mail = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

