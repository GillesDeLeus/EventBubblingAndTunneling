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
using System.Windows.Threading;

namespace EventBubblingAndTunneling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _tunnelCounter = 0;
        private int _bubbleCounter = 0;
        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
            _timer.Tick += Timer_Tick;
        }

        //Timer to reset bubble and tunnel counters every second
        private void Timer_Tick(object sender, EventArgs e)
        {
            _tunnelCounter = 0;
            _bubbleCounter = 0;
        }

        /*This method is called when you click the clear button.
        We first get the parent element of the button which is the stackpanel, the document outline shows this clearly
        next we use the VisualTreeHelper to get the Listbox, for this we use the hard-coded index 2 to get the Listbox and store it in the variable messages.Finally we clear the Listbox with the clear method.
        */
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement parent = (FrameworkElement)((Button)sender).Parent;
            ListBox messages = (ListBox)VisualTreeHelper.GetChild(parent, 2);
            messages.Items.Clear();
        }

        /*
        This method shows the Bubbling of events from the child element to the parents.
        it gets the sender and the source of the event and adds it to the messageList listbox.
        the commented method is another way of displaying the information.
        To use the second method simply comment the first method and uncomment the second method
        */
        private void Bubble_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _bubbleCounter += 1;
            string source = e.Source.GetType().Name;
            string sender1 = sender.GetType().Name;
            string message = _bubbleCounter + ": Source: " + source + " => Sender: " + sender1;
            this.messageList.Items.Add(message);
        }
        /*private void Bubble_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _bubbleCounter += 1;
            var message = string.Format($"bubble: {_bubbleCounter}: {sender.GetType().Name}");
            this.messageList.Items.Add(message);
        }*/

        /*This method displays the Tunneling of events from the Parent element down to the child element.
        It uses the same techniques used in the Bubble event above. Note that the messagelist we used to output the messages is messageList2. For simplicity we hard coded the name instead of retreiving the element like we did in the BtnClear_Click method.
        the commented method is another way of displaying the information.
        To use the second method simply comment the first method and uncomment the second method*/
        private void Tunnel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _tunnelCounter += 1;
            string source = e.Source.GetType().Name;
            string sender1 = sender.GetType().Name;
            var message = _tunnelCounter + ": Source: " + source + " => Sender: " + sender1;
            this.messageList2.Items.Add(message);
        }
        /*private void Tunnel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _tunnelCounter += 1;
            var message = string.Format($"Tunnel: {_tunnelCounter}: {sender.GetType().Name}");
            this.messageList2.Items.Add(message);
        }*/

        /*
         This method is used to show the Tunneling event and how to suppress an Event going down the visual tree.
         The method to show the tunneling information in the listbox is the same as above.
         */
        private void Both_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _tunnelCounter += 1;
            string source = e.Source.GetType().Name;
            string sender1 = sender.GetType().Name;
            var message = "Tunnel " + _tunnelCounter + ": Source: " + source + " => Sender: " + sender1;
            this.messageList3.Items.Add(message);
            //create a variable suppressbox which stores the information about the checkbox and wether it is checked or not.
            var suppressBox = (CheckBox)this.FindName("suppressCheckBox");
            /* create a variable clearBtn so that we can exclude the suppressing of events for this button.
            if we leave this out we cannot clear the messageLists anymore.*/
            var clearBtn = (Button)this.FindName("BtnClear3");
            //this is the control at which we want to suppress the event. You can change this to any control but be sure to cast the right type.
            var panel = (StackPanel)this.FindName("panel");
            /*
            here we check multiple conditions:
             - Is the suppress checkbox checked? if this is True, check the next condition
             - Is the source the clear button? if the source is the clear button do not execute the code inside the if statement.
             - Is the source the suppress checkbox? if the source is the checkbox do not execute the code inside the if statement.
             - Finally is the sender equal to the panel, this condition specifies at what control we want to suppress the event tunneling.
            */
            if (suppressCheckBox.IsChecked.Value && e.Source != clearBtn && e.Source != suppressBox && sender == panel)
            {
                //This causes the event to stop travelling down the logical tree
                e.Handled = true;
            }
        }
        /*private void Both_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _tunnelCounter += 1;
            this.messageList3.Items.Add(string.Format($"Tunnel {_tunnelCounter}: {sender.GetType().Name}"));
            var suppressBox = (CheckBox)this.FindName("suppressCheckBox");
            var clearBtn = (Button)this.FindName("BtnClear3");
            var panel = (StackPanel)this.FindName("panel");
            if (suppressCheckBox.IsChecked.Value && e.Source != clearBtn && e.Source != suppressBox && sender == panel)
            {
                e.Handled = true;
            }
        }*/

        /*
         This method is used in combination with the Both_PreviewMouseLeftButtonDown to show the bubbling and tunneling of events in a clear way
         */
        private void Both_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string source = e.Source.GetType().Name;
            string sender1 = sender.GetType().Name;
            string message = "Bubble" + _tunnelCounter + ": Source: " + source + " => Sender: " + sender1;
            this.messageList3.Items.Add(message);
            _tunnelCounter -= 1;
        }
        //private void Both_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    _bubbleCounter += 1;
        //    this.messageList3.Items.Add(string.Format($"Bubble {_bubbleCounter}: {sender.GetType().Name}"));
        //}

        /*
         This method shows how we can use the ClickEvent on a button.
         You can also find an alternate method to display the information below this method.
         */
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            string source = e.Source.GetType().Name;
            string sender1 = sender.GetType().Name;
            string message = "Click:" + " Source: " + source + " => Sender: " + sender1;
            this.messageList3.Items.Add(message);
        }
        //private void Btn_Click(object sender, RoutedEventArgs e)
        //{
        //    this.messageList3.Items.Add(string.Format($"Click : {sender.GetType().Name}"));
        //}
    }
}
