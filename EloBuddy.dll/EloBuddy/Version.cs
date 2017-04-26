namespace EloBuddy
{
    using System;
    using System.Runtime.InteropServices;

    public class Version
    {
        private static int m_build;
        private static int m_majorVersion;
        private static int m_minorVersion;
        private static int m_revision;
        private static System.Version m_version;

        static Version()
        {
            char[] separator = new char[] { '.' };
            string[] strArray = Game.Version.Split(separator);
            if (strArray.Length == 4)
            {
                m_majorVersion = int.Parse(strArray[0]);
                m_minorVersion = int.Parse(strArray[1]);
                m_build = int.Parse(strArray[2]);
                m_revision = int.Parse(strArray[3]);
                m_version = new System.Version(m_majorVersion, m_minorVersion, m_build, m_revision);
            }
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IsEqual(string version) => 
            m_version.Equals(new System.Version(version));

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IsEqual(System.Version version) => 
            m_version.Equals(version);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IsNewer(string version) => 
            (m_version > new System.Version(version));

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IsNewer(System.Version version) => 
            (m_version > version);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IsOlder(string version) => 
            (m_version < new System.Version(version));

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IsOlder(System.Version version) => 
            (m_version < version);

        public static int Build =>
            m_build;

        public static System.Version CurrentVersion =>
            m_version;

        public static int MajorVersion =>
            m_majorVersion;

        public static int MinorVersion =>
            m_minorVersion;

        public static int Revision =>
            m_revision;
    }
}

