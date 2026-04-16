using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace IPC.Cornerstone
{
    /// <summary>
    /// The primary interface for the IPC Cornerstone utility.
    /// Handles user interaction and real-time logging of the Preservation Protocol.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        // ObservableCollection allows the UI (ListView/ItemsControl) to update automatically
        private ObservableCollection<string> _logs = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            LogView.ItemsSource = _logs;
            AddLog("Cornerstone Initialized. Awaiting Protocol initiation...");
        }

        private void AddLog(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            _logs.Add($"[{timestamp}] {message}");

            // Auto-scroll to the bottom
            LogView.ScrollIntoView(_logs[_logs.Count - 1]);
        }

        /// <summary>
        /// Event handler for the primary "Initiate Protocol" button.
        /// </summary>
        private async void PreservationButton_Click(object sender, RoutedEventArgs e)
        {
            PreservationButton.IsEnabled = false;
            await Task.Run(() =>
            {
                PreservationEngine.ExecuteDeepProtocol(msg =>
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        AddLog(msg);
                    });
                });
            });
        }
    }
}