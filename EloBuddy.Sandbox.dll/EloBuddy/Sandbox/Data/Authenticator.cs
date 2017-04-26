namespace EloBuddy.Sandbox.Data
{
    using EloBuddy.Sandbox.Shared;
    using Microsoft.Win32;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Authenticator
    {
        private const string SignaturePublicKey = "BgIAAACkAABSU0ExAAoAAAEAAQDBGWkW67ml1g0gcT9RO51SOxT9RgDOGEpcxOiHcvCeW6z4cX5UJbt/i2yFT78N+Gn18OVzph4kVHcc7l5HKVIG/NdUjNam+/XHSWLFR1fEYxDwde6/w6TVpvnEZRjFvyuVe82ovHgmuTqNE/6RgaCs1k/Hq+yMLMsIdA8H0pboc6Xgc//moCPDWjVLDv9tJbVF0EgjBASd8gmGm+fbIUyzXXmsJ0ZQSP/Brxk5vFU9wQ0Ab3KQLHNO7Vt/k5FOQuEIucZn4OnILEz/dHLVQM3lanbKBiiz7HUb+rE/vOBj3FFKD/XW7V/L9nzMsc+0DmNDWydQyzHPbN+ixpq1ZuQE0qEgNcAHPLuUuOxG/k9OefG+eqE86NIUz9BT/muyzCGoodTEinjiV8uvFgRuah9O15ye/dQ+wvj/fgckN/69tw==";

        private static string GetHwid()
        {
            string str;
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey key2 = key.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    if (key2 == null)
                    {
                        return "failed";
                    }
                    object obj2 = key2.GetValue("MachineGuid");
                    if (obj2 == null)
                    {
                        return "failed2";
                    }
                    str = obj2.ToString();
                }
            }
            return str;
        }

        internal static void Verify(Configuration config)
        {
            IsBuddy = false;
            if (config.Signature != null)
            {
                CspParameters parameters = new CspParameters {
                    ProviderType = 1
                };
                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(parameters))
                {
                    using (SHA1CryptoServiceProvider provider2 = new SHA1CryptoServiceProvider())
                    {
                        provider.ImportCspBlob(Convert.FromBase64String("BgIAAACkAABSU0ExAAoAAAEAAQDBGWkW67ml1g0gcT9RO51SOxT9RgDOGEpcxOiHcvCeW6z4cX5UJbt/i2yFT78N+Gn18OVzph4kVHcc7l5HKVIG/NdUjNam+/XHSWLFR1fEYxDwde6/w6TVpvnEZRjFvyuVe82ovHgmuTqNE/6RgaCs1k/Hq+yMLMsIdA8H0pboc6Xgc//moCPDWjVLDv9tJbVF0EgjBASd8gmGm+fbIUyzXXmsJ0ZQSP/Brxk5vFU9wQ0Ab3KQLHNO7Vt/k5FOQuEIucZn4OnILEz/dHLVQM3lanbKBiiz7HUb+rE/vOBj3FFKD/XW7V/L9nzMsc+0DmNDWydQyzHPbN+ixpq1ZuQE0qEgNcAHPLuUuOxG/k9OefG+eqE86NIUz9BT/muyzCGoodTEinjiV8uvFgRuah9O15ye/dQ+wvj/fgckN/69tw=="));
                        object[] objArray1 = new object[] { config.Username, GetHwid(), config.GroupName, config.GroupId };
                        if (provider.VerifyData(Encoding.UTF8.GetBytes(string.Concat(objArray1)), provider2, config.Signature))
                        {
                            IsBuddy = PremiumGroups.Contains<int>(config.GroupId);
                        }
                    }
                }
            }
        }

        internal static bool IsBuddy
        {
            [CompilerGenerated]
            get => 
                <IsBuddy>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <IsBuddy>k__BackingField = value;
            }
        }

        private static int[] PremiumGroups =>
            new int[] { 7, 4, 8, 10, 13, 9, 11, 0x15, 12, 20, 6, 0x12 };
    }
}

