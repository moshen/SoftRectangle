using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using SoftRectangle.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SoftRectangle;

public partial class MainWindow : Window
{
    private Boolean isOperational = false;
    private KeyboardHook hook;
    private ViGEmClient controllerClient;
    private IXbox360Controller controller;
    private KeyState keyState;

    private AppConfig appConfig;
    private KeyConfig keyConfig;

    public MainWindow() 
    {
        // @TODO: Replace with actually reading from a config file, this is just
        // to confirm that the configuration code works
        appConfig = new AppConfig();
        appConfig.PassthroughKeysConfig = new List<string> { "LWin", "Tab" };

        hook = new KeyboardHook(appConfig);
        hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
        hook.KeyUp += new KeyboardHook.HookEventHandler(OnHookKeyUp);
        controllerClient = new ViGEmClient();
        controller = controllerClient.CreateXbox360Controller();
        controller.AutoSubmitReport = false;
        controller.Connect();

        // @TODO: Replace with actually reading from a config file, this is just
        // to confirm that the configuration code works
        string tempAppConfigJson = appConfig.Serialize();
        // @REMOVEME: Just checking our serialized config
        Debug.WriteLine(tempAppConfigJson);
        appConfig = AppConfig.Deserialize(tempAppConfigJson);

        // @TODO: Check for saved default config, otherwise load default config
        keyConfig = KeyConfig.GetDefaultMelee();

        string tempKeyConfigJson = keyConfig.Serialize();

        // @REMOVEME: Just checking our serialized config
        Debug.WriteLine(tempKeyConfigJson);

        // @TODO: Replace with actually reading from a config file, this is just
        // to confirm that the configuration code works
        keyConfig = KeyConfig.Deserialize(tempKeyConfigJson);

        keyState = new KeyState(keyConfig, controller);

        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        if (isOperational)
        {
            isOperational = false;
            button.Content = "Disabled";
            keyState.isTesting = true;
            hook.Uninstall();
        } else
        {
            isOperational = true;
            button.Content = "Enabled";
            keyState.isTesting = false;
            hook.Install();
        }
    }

    void OnHookKeyDown(object sender, HookEventArgs e)
    {
        // @REMOVEME
        Debug.WriteLine("Key: {0}, KeyCode: {1}", e.Key , (int)e.Key);

        if (keyConfig.KeyButtonMapping.ContainsKey(e.Key) )
        {
            keyState.SetActionState(keyConfig.KeyButtonMapping[e.Key]);

            keyState.Update();
        }
    }
    void OnHookKeyUp(object sender, HookEventArgs e)
    {
        // @REMOVEME
        Debug.WriteLine("Key: {0}, KeyCode: {1}", e.Key , (int)e.Key);

        if (appConfig.DisableKeys.Contains(e.Key))
        {
            Button_Click(this.FindName("EnableButton"), new RoutedEventArgs());
        }

        if (keyConfig.KeyButtonMapping.ContainsKey(e.Key) )
        {
            keyState.UnsetActionState(keyConfig.KeyButtonMapping[e.Key]);

            keyState.Update();
        }
    }

    // @TODO: Confirm we don't need any more cleanup
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Cleanup keyboard hook
        hook.Uninstall();

        // Cleanup controller
        controller.Disconnect();
    }

    // @TODO: Test right stick and remove
    private void XValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        keyState.testX = (short)e.NewValue;
        keyState.Update();
    }

    // @TODO: Test right stick and remove
    private void YValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        keyState.testY = (short)e.NewValue;
        keyState.Update();
    }
}

