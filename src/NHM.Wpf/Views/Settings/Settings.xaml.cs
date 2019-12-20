﻿using NHM.Common;
using NHMCore;
using NHMCore.Configs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static NHMCore.Translations;

namespace NHM.Wpf.Views.Settings
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private bool _isGeneral = true;

        public Settings()
        {
            InitializeComponent();
            OnScreenChange(_isGeneral);

            ConfigManager.ShowRestartRequired += ShowRestartRequired;
        }

        protected void OnScreenChange(bool isGeneral)
        {
            if (isGeneral)
            {
                GeneralButton.IsChecked = true;
                AdvancedButton.IsChecked = false;
                GeneralTab.IsSelected = true;
            }
            else
            {
                GeneralButton.IsChecked = false;
                AdvancedButton.IsChecked = true;
                AdvancedTab.IsSelected = true;
            }
        }

        private void Btn_GeneralSettings_Click(object sender, RoutedEventArgs e)
        {
            _isGeneral = true;
            OnScreenChange(_isGeneral);
        }

        private void Btn_AdvancedSettings_Click(object sender, RoutedEventArgs e)
        {
            _isGeneral = false;
            OnScreenChange(_isGeneral);
        }

        private async void Btn_default_Click(object sender, RoutedEventArgs e)
        {
            // keep message box here
            var result = MessageBox.Show(Tr("Are you sure you would like to set everything back to defaults? This will restart {0} automatically.", NHMProductInfo.Name),
                Tr("Set default settings?"),
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Translations.SelectedLanguage = "en";
                ConfigManager.SetDefaults();
                await ApplicationStateManager.RestartProgram();
            }
        }

        private void ShowRestartRequired(object sender, bool e)
        {
            btn_restart.Visibility = e ? Visibility.Visible : Visibility.Collapsed;
        }

        // RESTART doesn't work with debug console and mining running
        private async void Btn_restart_Click(object sender, RoutedEventArgs e)
        {
            await ApplicationStateManager.RestartProgram();
        }
    }
}
