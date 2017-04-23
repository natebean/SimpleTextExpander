
using SimpleTextExpander;
using SimpleTextExpander.Properties;
using System;
using System.Windows.Forms;

namespace SimpleTextExpander
{
    class Program
    {
        static void Main(string[] args)
        {

            //Application.Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new TheAppContext());
        }
    }
}//namespace



public class TheAppContext : ApplicationContext
{
    private NotifyIcon trayIcon;

    public TheAppContext()
    {
        KeyHandler ourKeyHandler = new KeyHandler();
        // Initialize Tray Icon
        trayIcon = new NotifyIcon()
        {
            Icon = Resources.AppIcon,
            ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
            Visible = true
        };
    }

    void Exit(object sender, EventArgs e)
    {
        // Hide tray icon, otherwise it will remain shown until user mouses over it
        trayIcon.Visible = false;

        Application.Exit();
    }
}