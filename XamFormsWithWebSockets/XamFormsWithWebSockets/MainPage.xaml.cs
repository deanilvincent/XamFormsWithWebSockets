using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websockets;
using Xamarin.Forms;

namespace XamFormsWithWebSockets
{
    public partial class MainPage : ContentPage
    {

        private bool failed, echo;
        private readonly IWebSocketConnection webSocketConn;
        public MainPage()
        {
            InitializeComponent();

            webSocketConn = WebSocketFactory.Create();

            webSocketConn.OnMessage += WebSocketConn_OnMessage;
        }

        private void WebSocketConn_OnMessage(string obj)
        {
            echo = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                ReceivedMessages.Children.Add(new Label{Text = obj});
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            failed = false;
            echo = false;

            webSocketConn.Open("ws://aspnetcorewithwebsockets.azurewebsites.net/");

            TaskDelay();
        }

        
        private async void TaskDelay()
        {
            while (webSocketConn.IsOpen != true && failed != true)
            {
                await Task.Delay(10);
            }
        }

        private void ButtonSend_OnClicked(object sender, EventArgs e)
        {
            echo = false;
            webSocketConn.Send(Message.Text);
            TaskDelay();
        }
    }
}
