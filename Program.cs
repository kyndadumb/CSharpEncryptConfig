using System.Diagnostics;

namespace EncryptionConfig
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EncryptString(true);
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["ConnectionString01"];
            Console.WriteLine(connectionString);
        }

        public static void EncryptString(bool encrypt)
        {
            Configuration? config = null;
            
            try
            {
                // open app.config and retrieve connectionStrings-section
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConnectionStringsSection connectionStringsSection = config.GetSection("connectionStrings") as ConnectionStringsSection;

                // if - section & element are not locked
                if ((!connectionStringsSection.ElementInformation.IsLocked) && (!connectionStringsSection.SectionInformation.IsLocked)) 
                {
                    // if - enrypt the section
                    if (encrypt && !connectionStringsSection.SectionInformation.IsProtected)
                    {
                        connectionStringsSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    }

                    // if - enrypt is false --> decrypt
                    if (!encrypt && connectionStringsSection.SectionInformation.IsProtected)
                    {
                        connectionStringsSection.SectionInformation.UnprotectSection();
                    }

                    // save the new app.config and open in notepad
                    connectionStringsSection.SectionInformation.ForceSave = true;
                    config.Save();

                    Process.Start("notepad.exe", config.FilePath);
                    
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}