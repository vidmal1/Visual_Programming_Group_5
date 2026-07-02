 using System;
using System.Windows.Forms;
using FocusTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            try
            {
                using var context = new AppDbContext();

                try
                {
                    context.Database.Migrate();
                }
                catch
                {
                    context.Database.EnsureCreated();
                }

                bool isFirstRun = !context.AppCategories.Any();
                FocusTrack.Helpers.StorageHelper.EnsureDefaultCategories();
                
                if (isFirstRun)
                {
                    FocusTrack.Helpers.StorageHelper.EnsureDefaultIgnoredApps();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"The database could not be prepared.\n\n{ex.Message}",
                    "FocusTrack",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (args.Length > 0 && args[0] == "--export-test")
            {
                try
                {
                    Console.WriteLine("Running PDF Export Test runner...");
                    Console.WriteLine("[TRACE] Instantiating MainForm...");
                    var form = new MainForm();
                    form.Shown += async (s, e) =>
                    {
                        try
                        {
                            Console.WriteLine("[TRACE] MainForm shown. Calling RunTestExportAsync...");
                            await form.RunTestExportAsync();
                            Console.WriteLine("PDF Export Test completed successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"PDF Export Test failed: {ex}");
                        }
                        finally
                        {
                            form.Close();
                        }
                    };
                    Application.Run(form);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PDF Export Test failed: {ex}");
                }
                Environment.Exit(0);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}
