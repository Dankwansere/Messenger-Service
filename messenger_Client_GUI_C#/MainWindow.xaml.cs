using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using messengerClient1.messengerWebService;
using System.Threading;
using System.ComponentModel;

namespace messengerClient1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        /*
         * retrieve  messenger service wsdl stub
         * 
         */
        messengerWebServiceService msgService = new messengerWebServiceService();
        String userName;
        Boolean retrieveMessageLoop = true;
        Boolean contactUpDate = true;
        

        public MainWindow()
        {           
            InitializeComponent();
        }

        #region add user
        /*
         * Add user to chat server
         * display logged in message
         * disable "Enter chat" button if user was succesfully added
         * 
         */
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {

                this.userName = userNameTextBox.Text;
                msgService.addUser(this.userName);
                this.contactList_Click();
                this.retrieveMessage();
                this.LoggedInMessage.Content = "You are currently logged in as: " + this.userName;
                this.LoggedInMessage.IsEnabled = true;

                //this.userListBox.IsEnabled = true;
                this.messageBox.IsEnabled = true;
                this.sendMessageButton.IsEnabled = true;
                this.logoutButton.IsEnabled = true;
                messageListBox.Text = string.Empty;

                this.addUser.IsEnabled = false;
            }
            catch (System.Net.WebException ex)
            {
                 messageListBox.Text = "Could not connect to remote server. \n Please make sure SimpleMessenger web service is running";
            }
        }
        #endregion

        #region contact list update Async
        /*
         * Using background worker to work on a seperate thread to constanly
         * update the contact list textbox UI to display users currently logged in
         * 
         */
        private void contactList_Click()
        {
            BackgroundWorker contactListUpdate = new BackgroundWorker();
            contactListUpdate.WorkerReportsProgress = true;
            contactListUpdate.DoWork += DoWorkRretrieveContacts;
            contactListUpdate.ProgressChanged += ProgresschangedRetrieveContacts;
            contactListUpdate.RunWorkerAsync(contactUpDate);
        }

        /*
         * Background worker to update contact list 
         *  
         */
        private void DoWorkRretrieveContacts(object sender, DoWorkEventArgs e)
        {
            while (this.contactUpDate)
            {
                String[] listOfNames = msgService.getUsers();
                (sender as BackgroundWorker).ReportProgress(1, listOfNames);
                System.Threading.Thread.Sleep(100);
            }
        }

        /*
         * Background worker for contact list update.
         * This method will update the contact textbox asynchronously 
         * 
         */
        private void ProgresschangedRetrieveContacts(object sender, ProgressChangedEventArgs e)
        {
            String[] listOfNames = (String[])e.UserState;
            
            //List of strings to store user list to update ListBox UI
            List<String> items = new List<String>();
            try
            {
                for (int i = 0; i < listOfNames.Length; i++)
                {
                    //userListBox.Text += listOfNames[i].ToString() + "\n";
                    items.Add(listOfNames[i].ToString());
                }
                this.userListBox1.ItemsSource = items;
            }

            catch (Exception ex)
            {

            }


        }
        #endregion

        #region message Retrieval Async
        /*
         * Using background worker to work on a seperate thread to constanly
         * update the message list textbox UI to display sent and received messages
         */

        private void retrieveMessage()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork_retrieveMessage;
            worker.ProgressChanged += worker_ProgressChanged_retrieveMessage;
            worker.RunWorkerAsync(retrieveMessageLoop);
        }

        /*
         * Background worker for contact list update.
         * This method will update the contact textbox asynchronously 
         * retrieveMessage service returns an array of strings 1. From User name. 2. Message
         * 
         */
        private void worker_DoWork_retrieveMessage(object sender, DoWorkEventArgs e)
        {
            String[] retrieveMessageResponse = new String[2];
            while (this.retrieveMessageLoop)
            {
                retrieveMessageResponse = msgService.retrieveMessage(this.userName);
                (sender as BackgroundWorker).ReportProgress(1, retrieveMessageResponse);
                System.Threading.Thread.Sleep(1);
            }
        }

        private void worker_ProgressChanged_retrieveMessage(object sender, ProgressChangedEventArgs e)
        {
            //If there is no message in the response do nothing
            String[] retrieveMessageResponse = e.UserState as String[];
            if (String.IsNullOrEmpty(retrieveMessageResponse[1]))
            {
            }
            else
            {
                messageListBox.Text +=  retrieveMessageResponse[1] + ": " + retrieveMessageResponse[0] + "\n";
                messageListBox.Foreground = Brushes.Blue;
            }
        }
        #endregion

        #region Send Message
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            String destinationUser = (String)userListBox1.SelectedItem;
            Boolean result = msgService.sendMessage(destinationUser, this.userName, messageBox.Text);

            if (result == true)
            {
                messageListBox.Text += this.userName +": " + messageBox.Text + "\n";
                
                messageBox.Text = string.Empty;
            }
        }
        #endregion


        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {

            if (msgService.removeUser(this.userName))
            {
               this.LoggedInMessage.Content = "User " + this.userName + " has successfully logged out";
               this.contactUpDate = false;
               this.retrieveMessageLoop = false;
              
               this.messageBox.Text = string.Empty; 
                          
               this.messageListBox.IsEnabled = false;
               this.messageBox.IsEnabled = false; 
                               
               this.sendMessageButton.IsEnabled = false;
               this.addUser.IsEnabled = true;
               this.logoutButton.IsEnabled = false;

            }
        }

    }
}
