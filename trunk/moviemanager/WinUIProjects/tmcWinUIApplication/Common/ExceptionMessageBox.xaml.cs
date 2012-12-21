using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace APP
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ExceptionMessageBox
    {
        readonly string _userExceptionMessage;
        readonly List<string> _exceptionInformationList = new List<string>();


        public ExceptionMessageBox(Exception e, string userExceptionMessage)
        {
            InitializeComponent();

            _userExceptionMessage = userExceptionMessage;
            textBox1.Text = userExceptionMessage;

            TreeViewItem TreeViewItem = new TreeViewItem {Header = "Exception"};
            TreeViewItem.ExpandSubtree();
            BuildTreeLayer(e, TreeViewItem);
            treeView1.Items.Add(TreeViewItem);            
        }

        void BuildTreeLayer(Exception e, TreeViewItem parent)
        {
            String ExceptionInformation = "\n\r\n\r" + e.GetType() + "\n\r\n\r";
            parent.DisplayMemberPath = "Header";
            parent.Items.Add(new TreeViewStringSet() { Header = "Type", Content = e.GetType().ToString() });
            PropertyInfo[] MemberList = e.GetType().GetProperties();
            foreach (PropertyInfo Info in MemberList)
            {
                var Value = Info.GetValue(e, null);
                if (Value != null)
                {
                    TreeViewStringSet TreeViewStringSet = new TreeViewStringSet { Header = Info.Name, Content = Value.ToString() };
                    if (Info.Name == "InnerException")
                    {
                        TreeViewItem TreeViewItem = new TreeViewItem {Header = Info.Name};
                        BuildTreeLayer(e.InnerException, TreeViewItem);
                        parent.Items.Add(TreeViewItem);
                    }
                    else 
                    {
                        parent.Items.Add(TreeViewStringSet);
                        ExceptionInformation += TreeViewStringSet.Header + "\n\r\n\r" + TreeViewStringSet.Content + "\n\r\n\r";
                    }
                }
            }
            _exceptionInformationList.Add(ExceptionInformation);
        }


        private void TreeView1SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue.GetType() == typeof(TreeViewItem) ) textBox1.Text = "Exception";
            else textBox1.Text = e.NewValue.ToString();
        }

        private class TreeViewStringSet
        {
            public string Header { get; set; }
            public string Content { get; set; }

            public override string ToString()
            {
                return Content;
            }
        }

        private void ButtonClipboardClick(object sender, RoutedEventArgs e)
        {
            string ClipboardMessage = _userExceptionMessage + "\n\r\n\r";
            foreach (string Info in _exceptionInformationList) ClipboardMessage += Info;
            Clipboard.SetText(ClipboardMessage);
        }

        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
