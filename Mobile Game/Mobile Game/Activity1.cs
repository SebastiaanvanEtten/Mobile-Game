using Android.App;
using Android.Content.PM;
using Android.InputMethodServices;
using Android.OS;
using Android.Views;
using Java.Lang;
using System;

namespace Mobile_Game
{
    [Activity(Label = "Mobile Game"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    


    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        private void HideSystemUI()
        {
            if (Android.OS.Build.VERSION.SdkInt >= (Android.OS.BuildVersionCodes)19)
            {
                View decorView = Window.DecorView;
                var uiOptions = (int)decorView.SystemUiVisibility;
                var newUiOptions = (int)uiOptions;

                newUiOptions |= (int)SystemUiFlags.LowProfile;
                newUiOptions |= (int)SystemUiFlags.Fullscreen;
                newUiOptions |= (int)SystemUiFlags.HideNavigation;
                newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

                decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
                
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new Game1(() => HideSystemUI());
            SetContentView((View)g.Services.GetService(typeof(View)));
            
            g.Run();
        }

        
    }
}

