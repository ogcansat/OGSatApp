using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

using Xamarin.Forms;
using Timer = System.Timers.Timer;

namespace OGSatApp.Pages.Behaviors
{
    public static class GUIAnimations
    {
        public static CancellationTokenSource DotLoadingAnimation(Label label, string title, int length, double interval)
        {

            var token = new CancellationTokenSource();
            Timer timer = new Timer(interval);
            timer.Elapsed += (s, a) =>
            {
                if (token.IsCancellationRequested)
                {
                    Device.InvokeOnMainThreadAsync(() => label.Text = "");
                    timer.Stop();
                    return;
                }

                Device.InvokeOnMainThreadAsync(() =>
                {
                    label.Text = !label.Text.StartsWith(title) ? title : label.Text;
                    label.Text += ".";
                    if (label.Text.Length >= title.Length + length) label.Text = title;
                });

                
            };
            timer.Start();

            return token;
           
        }
    }
}
