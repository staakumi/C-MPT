using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace Praktosik2Csharp
{


    public partial class MainWindow : Window
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Note> _notes;
        public ObservableCollection<Note> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Notes)));
            }
        }

        private Note _selectedNote;
        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNote)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
           
            Notes = SerializationHelper.Deserialize<ObservableCollection<Note>>("notes.json");
        }

        private void NotesListBox_SelectionChanged(object sender, EventArgs e)
        {
            SelectedNote = (Note)notesListBox.SelectedItem;
            noteDetailsPanel.Visibility = Visibility.Visible;
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            
            SerializationHelper.Serialize(Notes, "notes.json");
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            
            Notes.Remove(SelectedNote);
            SerializationHelper.Serialize(Notes, "notes.json");
            SelectedNote = null;
            noteDetailsPanel.Visibility = Visibility.Collapsed;
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
           
            Notes.Add(new Note { Title = newNoteTitle.Text, Description = newNoteDescription.Text, DueDate = newNoteDueDate.SelectedDate ?? DateTime.Now });
            SerializationHelper.Serialize(Notes, "notes.json");
            newNoteTitle.Text = "";
            newNoteDescription.Text = "";
            newNoteDueDate.SelectedDate = DateTime.Now;
        }
        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (notesListBox.SelectedItem != null)
            {
                
                Note selectedNote = (Note)notesListBox.SelectedItem;

                
                TextBlock selectedNoteTitle = FindVisualChild<TextBlock>(noteDetailsPanel, "SelectedNoteTitle");
                TextBox noteDescriptionTextBox = FindVisualChild<TextBox>(noteDetailsPanel, "NoteDescription");
                DatePicker noteDueDatePicker = FindVisualChild<DatePicker>(noteDetailsPanel, "NoteDueDate");

                if (selectedNoteTitle != null && noteDescriptionTextBox != null && noteDueDatePicker != null)
                {
                    
                    selectedNoteTitle.Text = selectedNote.Title;
                    noteDescriptionTextBox.Text = selectedNote.Description;
                    noteDueDatePicker.SelectedDate = selectedNote.DueDate;

                    
                    noteDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }

        
        private T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is FrameworkElement frameworkElement && frameworkElement.Name == name)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child, name);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

    }
    
    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }

    public static class SerializationHelper
    {
        public static void Serialize<T>(T data, string filePath)
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, jsonString);
        }

        public static T Deserialize<T>(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonString);
        }

    }

}









