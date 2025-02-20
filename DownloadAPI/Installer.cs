﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Spaceware
{
    class DownloadAPI
    {

        /// <summary>
        /// Method: DisplayBanner(int option)
        /// Description: Displays Banner With Question. 
        /// Usage: DownloadWorker.DisplayBanner(1);
        /// Values: 1 = Installing | 2 = MenuOptions
        /// </summary>
        public void DisplayBanner(int option)
        {
            switch (option)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Green;
                    InstallLog("=======================================================================================================================");
                    InstallLog("                                                     INSTALLER                                                         ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                    █████╗ ██████╗ ███████╗ █████╗     ███████╗ ██╗                                    ");
                    InstallLog("                                   ██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔════╝███║                                    ");
                    InstallLog("                                   ███████║██████╔╝█████╗  ███████║    ███████╗╚██║                                    ");
                    InstallLog("                                   ██╔══██║██╔══██╗██╔══╝  ██╔══██║    ╚════██║ ██║                                    ");
                    InstallLog("                                   ██║  ██║██║  ██║███████╗██║  ██║    ███████║ ██║                                    ");
                    InstallLog("                                   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝    ╚══════╝ ╚═╝                                    ");
                    InstallLog("                                *__________________________________________________*                                   ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                        A Spaced out installer for Area51                                              ");
                    InstallLog("                          The Developers's: Joshua, Maxie, PandaStudios & Swordsith                                    ");
                    InstallLog($"                        Client Website: https://outerspace.store/ | Client Version: {ClientVersion}                   ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("=======================================================================================================================\n");
                    Console.ForegroundColor = ConsoleColor.White; 
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    InstallLog("=======================================================================================================================");
                    InstallLog("                                                     INSTALLER                                                         ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                    █████╗ ██████╗ ███████╗ █████╗     ███████╗ ██╗                                    ");
                    InstallLog("                                   ██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔════╝███║                                    ");
                    InstallLog("                                   ███████║██████╔╝█████╗  ███████║    ███████╗╚██║                                    ");
                    InstallLog("                                   ██╔══██║██╔══██╗██╔══╝  ██╔══██║    ╚════██║ ██║                                    ");
                    InstallLog("                                   ██║  ██║██║  ██║███████╗██║  ██║    ███████║ ██║                                    ");
                    InstallLog("                                   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝    ╚══════╝ ╚═╝                                    ");
                    InstallLog("                                *__________________________________________________*                                   ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                        A Spaced out installer for Area51                                              ");
                    InstallLog("                          The Developers's: Joshua, Maxie, PandaStudios & Swordsith                                    ");
                    InstallLog($"                                  Client Website: https://outerspace.store/                                           ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("                                             Please Choose An Option:                                                  ");
                    InstallLog("                                              - 1.)  Installer                                                         ");
                    InstallLog("                                              - 2.)  Unban Tool                                                        ");
                    InstallLog("                                                                                                                       ");
                    InstallLog("=======================================================================================================================\n");
                    Console.ForegroundColor = ConsoleColor.White; InstallLog("[Info] Choose an Option?: ");
                    break;
            }
        }

        /// <summary>
        /// Method: InitDownload();
        /// Description: Initializes the installer process. 
        /// Usage: DownloadWorker.InitDownload();
        /// Return Values: 0 = Folder Not Found | 1 = Lastest Version | 2 = ERROR | 3 = Valid Response
        /// </summary>
        public int InitDownload()
        {
            try
            {
                using (var client = new WebClient { Headers = { "Accept: text/html, application/xhtml+xml, */*", "User-Agent: SpacewareAPI/9.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; SpacewareAPI/9.0)" } })
                {
                    /*if selected folder path is Empty or doesnt = VRChat return folder not found*/
                    if (VRChatFolder() == null || !vrchatPath.Contains("VRChat")) return 0;
                    InstallLog("[Info] Checking for update....");
                   
                    /*Obtains ClientVersion / defaults to 1.0.0.0*/
                    if (File.Exists($"{vrchatPath}\\Area51\\DLL\\Area51.dll"))
                    {

                        ClientVersion = FileVersionInfo.GetVersionInfo($"{vrchatPath}\\Area51\\DLL\\Area51.dll").FileVersion;
                    }
                    else 
                    {
                        InstallLog("[Info] New version found!");
                        ClientVersion = "1.0.0.0";
                    }

                    /*Obtains ServerVersion, zippath and Displays install banner*/
                    ServerVersion = client.DownloadString($"{APILink}version");
                    zipPath = $"{Guid.NewGuid().ToString("N").Substring(0, 8)}.zip";
                    DisplayBanner(1);

                    /*If server version equals version on disk dont download else download the lastest version!*/
                    if (ServerVersion == ClientVersion) return 1;
                   
                    /*removes older melonloader Folder/Files/proxy.dll*/
                    isYesOrNo = MessageBox.Show("Would you like to reinstall MelonLoader?", "YesOrNo?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (isYesOrNo == DialogResult.Yes)
                    {
                        for (int i = 0; i < FoldersNames.Length; i++)
                        {
                            if (i == FoldersNames.Length) break;
                            if (Directory.Exists($"{vrchatPath}\\{FoldersNames[i]}"))
                            {
                                Directory.Delete($"{vrchatPath}\\{FoldersNames[i]}", true);
                                InstallLog($"[Info] Removed {vrchatPath}\\{FoldersNames[i]}, Done!");
                            }
                            HasMelons = "melon51";
                        }

                        for (int i = 0; i < FileNames.Length; i++)
                        {
                            if (i == FileNames.Length) break;
                            if (File.Exists($"{vrchatPath}\\{FileNames[i]}"))
                            {
                                File.Delete($"{vrchatPath}\\{FileNames[i]}");
                                InstallLog($"[Info] Removed {vrchatPath}\\{FileNames[i]}, Done!");
                            }
                        }
                    }
                    else if (isYesOrNo == DialogResult.No)
                    {
                        for (int i = 0; i < FoldersNamesNOML.Length; i++)
                        {
                            if (i == FoldersNamesNOML.Length) break;
                            if (Directory.Exists($"{vrchatPath}\\{FoldersNamesNOML[i]}"))
                            {
                                Directory.Delete($"{vrchatPath}\\{FoldersNamesNOML[i]}", true);
                                InstallLog($"[Info] Removed {vrchatPath}\\{FoldersNamesNOML[i]}, Done!");
                            }
                            HasMelons = "update";
                        }
                        for (int i = 0; i < FileNamesNOML.Length; i++)
                        {
                            if (i == FileNamesNOML.Length) break;
                            if (File.Exists($"{vrchatPath}\\{FileNamesNOML[i]}"))
                            {
                                File.Delete($"{vrchatPath}\\{FileNamesNOML[i]}");
                                InstallLog($"[Info] Removed {vrchatPath}\\{FileNamesNOML[i]}, Done!");
                            }
                        }
                    }

                    

                    InstallLog($"[Info] Downloading New Update: {ServerVersion}");
                    client.DownloadFile(new Uri($"{APILink}download/{HasMelons}/{File.ReadAllText("Authorization.json")}"), zipPath);
                    return 3;
                }
            }
            catch (Exception ErrorOnInstall) { InstallLog($"[Install] {ErrorOnInstall.StackTrace}"); if (ErrorOnInstall.StackTrace != null) { return 2; } }
            return 2;
        }

        /// <summary>
        /// Method: InitExtaction();
        /// Description: Initializes the extaction process. 
        /// Usage: DownloadWorker.InitExtaction();
        /// </summary>
        public bool InitExtaction()
        {
            try
            {
                /*if path is not empty install else return exception*/
                if (zipPath == string.Empty) return false;
                InstallLog("Installing....", true);
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, $"{vrchatPath}");
                return true;
            }
            catch (Exception ErrorOnExtaction)
            {
                if (ErrorOnExtaction.InnerException != null)
                {
                    InstallLog($"[Extaction] {ErrorOnExtaction.StackTrace}\nSomething really done goofed on Extaction, please join server and make a ticket.\nDiscord: https://discord.gg/Paul", true);
                }
            }
            return false;
        }

        /// <summary>
        /// Method: ValidateToken(string token);
        /// Description: Checks validity of the token. 
        /// Usage: DownloadWorker.ValidateToken("A513-A512-A514");
        /// </summary>
        public bool ValidateToken(string token)
        {
            try
            {
                using (var client = new WebClient { Headers = { "Accept: text/html, application/xhtml+xml, */*", "User-Agent: SpacewareAPI/9.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; SpacewareAPI/9.0)" } })
                {
                    if (client.DownloadString($"{APILink}check/{token}").Contains("True")) return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Method: VRChatFolder();
        /// Description: Shows A FileDialogBrowser To Obtain VRC Folder Path. 
        /// Usage: DownloadWorker.VRChatFolder();
        /// </summary>
        public string VRChatFolder()
        {
            using (OpenFileDialog fdb = new OpenFileDialog())
            {
                if (fdb.ShowDialog() == DialogResult.OK)
                {   
                    fdb.ValidateNames = false;
                    fdb.CheckFileExists = false;
                    fdb.CheckPathExists = true;
                    fdb.RestoreDirectory = true;
                    vrchatPath = Path.GetDirectoryName(fdb.FileName);
                }
                return vrchatPath;
            }
        }

        #region Public Functions / Variables - Log, APILink, VRChatpath ect. these are gloabally called throughout this project.
        public string[] FoldersNames = { "MelonLoader", "Area51", "UserData\\Icons", "UserData\\Icons", "Plugins" };
        public string[] FoldersNamesNOML = {"Area51", "UserData\\Icons", "UserData\\Icons", "Plugins" };
        public string[] FileNames = { "version.dll", "Mods\\SpaceShip.dll", "Mods\\AstralCore.dll" };
        public string[] FileNamesNOML = { "Mods\\SpaceShip.dll", "Mods\\AstralCore.dll" };
        public static void InstallLog(string text, bool state = false) { Console.WriteLine(text); if (state) { Console.Read(); } }
        public readonly string APILink = "https://api.outerspace.store/session/";
        public string zipPath, vrchatPath, ClientVersion, ServerVersion, HasMelons = string.Empty;
        public static DialogResult isYesOrNo = new DialogResult();
        #endregion

    }
}
