using System;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Configuration;

using System.Security.Cryptography;
using System.Text;
using System.IO;

using FilmsStorage.Models.Entities;
using FilmsStorage.Models.Serialization;

namespace FilmsStorage.SL
{
    public static class _SL
    {
        public static class Hasher
        {
            public static byte[] createHash(string strToHash)
            {
                SHA512 hasher = SHA512.Create();

                byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(strToHash));

                return hash;
            }

            public static bool checkPassword(byte[] sourcePasswordHash, string textPasswordString)
            {
                byte[] textPasswordStringHash = createHash(textPasswordString);

                return compareTwoHashes(sourcePasswordHash, textPasswordStringHash);
            }

            public static bool compareTwoHashes(byte[] sourceHash, byte[] destinationHash)
            {
                bool areHashedEqual = true;

                //It makes sense to check hashes with the same length
                if (sourceHash.Length == destinationHash.Length)
                {
                    for (int i = 0; i < sourceHash.Length; ++i)
                    {
                        if (sourceHash[i] != destinationHash[i])
                        {
                            areHashedEqual = false;
                            break;
                        }
                    }
                }

                return areHashedEqual;
            }
        }

        public static class Cookies
        {
            public static void SetLoginCookie(User loggedInUser)
            {
                UserSerializationModel userSerializationModel = new UserSerializationModel()
                {
                    UserID = loggedInUser.UserID
                };

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                string userDataJson = jsonSerializer.Serialize(userSerializationModel);

                int cookieTimeoutMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["LoginTimeout"]);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    version: 1,
                    name: loggedInUser.Login,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddMinutes(cookieTimeoutMinutes),
                    isPersistent: true,
                    userData: userDataJson
                    );

                string encryptedAuthTicket = FormsAuthentication.Encrypt(authTicket);

                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedAuthTicket);

                // Cookie lifetime should be no longer than uath ticket lifetime 
                authCookie.Expires = authTicket.Expiration;

                HttpContext.Current.Response.Cookies.Add(authCookie);
            }

            public static void SetLanguageCookie(string webSiteLang)
            {
                HttpCookie langCookie = new HttpCookie("WebSiteLang", webSiteLang) { Expires = DateTime.MaxValue };

                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
        }

        public static class Files
        {
            public static FileSaveResult SaveFilm(HttpPostedFileBase postedFile, int uploadedByUser)
            {
                FileSaveResult fileSaveResult = new FileSaveResult();

                try
                {
                    //FileSavePath = WebSite Path + fileSaveFolder + fileName[renamed] + ext.
                    string webSiteFolder = HttpContext.Current.Server.MapPath("~");
                    string fileSaveFolder = ConfigurationManager.AppSettings["FileUploadFolder"];

                    string fileSaveRootFolder = Path.Combine(webSiteFolder, fileSaveFolder);
                    string userSaveFolder = Path.Combine(fileSaveRootFolder, uploadedByUser.ToString());
                    string fileSaveName = postedFile.FileName;

                    if (!Directory.Exists(fileSaveRootFolder))
                    {
                        Directory.CreateDirectory(fileSaveRootFolder);
                    }

                    if (!Directory.Exists(userSaveFolder))
                    {
                        Directory.CreateDirectory(userSaveFolder);
                    }

                    if (File.Exists(Path.Combine(userSaveFolder, fileSaveName)))
                    {
                        //rename strategy: OriginalFileName_Ticks.Exit
                        fileSaveName = Path.GetFileNameWithoutExtension(fileSaveName)
                            + "_"
                            + DateTime.Now.Ticks
                            + Path.GetExtension(fileSaveName);
                    }

                    string fileSaveFullPath = Path.Combine(userSaveFolder, fileSaveName);

                    postedFile.SaveAs(fileSaveFullPath);

                    fileSaveResult.IsSaved = true;
                    fileSaveResult.FileName = fileSaveName;
                    fileSaveResult.FilePath = userSaveFolder;
                }
                catch (Exception ex)
                {
                    fileSaveResult.Error = ex;
                }


                return fileSaveResult;
            }

            public static bool DeleteFilm(string filePath, string fileName)
            {
                bool fileDeleteResult = false;

                string fileFullPath = Path.Combine(filePath, fileName);

                if (File.Exists(fileFullPath))
                {
                    try
                    {
                        File.Delete(fileFullPath);

                        fileDeleteResult = true;
                    }
                    catch { }
                }

                return fileDeleteResult;
            }
        }

        public static class StringValidator
        {
            public static bool IsContainMinimumSpecialSymbols(string str, int minAmount) 
            {
                int specialSymbolsAmount = 0;

                foreach(char symbol in str)
                {
                    if (IsSpecialSymbol(symbol))
                        specialSymbolsAmount++;
                }

                return specialSymbolsAmount >= minAmount;
            }

            private static bool IsSpecialSymbol(char symbol)
            {
                string specialSymbols = "[]{}()^-=$!|?*+.,@#%&~";

                return specialSymbols.Contains(symbol.ToString());
            }

            public static bool IsAlternationUpperAndLowerCaseSymbols(string str)
            {
                for (int i = 0; i < str.Length - 1; ++i)
                {
                    if (Char.IsLower(str[i]) == Char.IsLower(str[i + 1]) &&
                        !IsSpecialSymbol(str[i]) &&
                        !IsSpecialSymbol(str[i + 1]))
                        return false;
                }

                return true;
            }
        }
    }
}