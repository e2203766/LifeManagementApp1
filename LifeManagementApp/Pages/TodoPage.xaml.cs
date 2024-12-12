using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace LifeManagementApp.Pages
{
    public partial class TodoPage : ContentPage
    {
        private ObservableCollection<string> _tasks;

        public TodoPage(ObservableCollection<string> tasks)
        {
            InitializeComponent();
            _tasks = tasks; // Привязка к переданному списку
            TodoListView.ItemsSource = _tasks;
        }

        private void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskEntry.Text))
            {
                _tasks.Add(NewTaskEntry.Text);
                NewTaskEntry.Text = string.Empty;
            }
        }
    }
}




