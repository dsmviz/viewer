﻿using System.Windows;

namespace Dsmviz.Viewer.View.Editing
{
    /// <summary>
    /// Interaction logic for ElementEditDialog.xaml
    /// </summary>
    public partial class ElementEditDialog
    {
        public ElementEditDialog()
        {
            InitializeComponent();
        }

        private void OnOkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
