namespace EloBuddy
{
    using EloBuddy.Native;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Chat
    {
        internal static List<ChatClientSideMessage> ChatClientSideMessageHandlers;
        internal static List<ChatInput> ChatInputHandlers;
        internal static List<ChatMessage> ChatMessageHandlers;
        internal static List<ChatSendWhisper> ChatSendWhisperHandlers;
        internal static IntPtr m_ChatClientSideMessageNative = new IntPtr();
        internal static OnChatClientSideMessageNativeDelegate m_ChatClientSideMessageNativeDelegate;
        internal static IntPtr m_ChatInputNative = new IntPtr();
        internal static OnChatInputNativeDelegate m_ChatInputNativeDelegate;
        internal static IntPtr m_ChatMessageNative = new IntPtr();
        internal static OnChatMessageNativeDelegate m_ChatMessageNativeDelegate;
        internal static IntPtr m_ChatSendWhisperNative = new IntPtr();
        internal static OnChatSendWhisperNativeDelegate m_ChatSendWhisperNativeDelegate;

        public static  event ChatClientSideMessage OnClientSideMessage
        {
            add
            {
                ChatClientSideMessageHandlers.Add(handler);
            }
            remove
            {
                ChatClientSideMessageHandlers.Remove(handler);
            }
        }

        public static  event ChatInput OnInput
        {
            add
            {
                ChatInputHandlers.Add(handler);
            }
            remove
            {
                ChatInputHandlers.Remove(handler);
            }
        }

        public static  event ChatMessage OnMessage
        {
            add
            {
                ChatMessageHandlers.Add(handler);
            }
            remove
            {
                ChatMessageHandlers.Remove(handler);
            }
        }

        public static  event ChatSendWhisper OnSendWhisper
        {
            add
            {
                ChatSendWhisperHandlers.Add(handler);
            }
            remove
            {
                ChatSendWhisperHandlers.Remove(handler);
            }
        }

        static Chat()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(Chat.DomainUnloadEventHandler);
            ChatInputHandlers = new List<ChatInput>();
            m_ChatInputNativeDelegate = new OnChatInputNativeDelegate(Chat.OnChatInputNative);
            m_ChatInputNative = Marshal.GetFunctionPointerForDelegate(m_ChatInputNativeDelegate);
            EloBuddy.Native.EventHandler<40,bool __cdecl(char * *),char * *>.Add(EloBuddy.Native.EventHandler<40,bool __cdecl(char * *),char * *>.GetInstance(), m_ChatInputNative.ToPointer());
            ChatMessageHandlers = new List<ChatMessage>();
            m_ChatMessageNativeDelegate = new OnChatMessageNativeDelegate(Chat.OnChatMessageNative);
            m_ChatMessageNative = Marshal.GetFunctionPointerForDelegate(m_ChatMessageNativeDelegate);
            EloBuddy.Native.EventHandler<41,bool __cdecl(EloBuddy::Native::AIHeroClient *,char * *),EloBuddy::Native::AIHeroClient *,char * *>.Add(EloBuddy.Native.EventHandler<41,bool __cdecl(EloBuddy::Native::AIHeroClient *,char * *),EloBuddy::Native::AIHeroClient *,char * *>.GetInstance(), m_ChatMessageNative.ToPointer());
            ChatClientSideMessageHandlers = new List<ChatClientSideMessage>();
            m_ChatClientSideMessageNativeDelegate = new OnChatClientSideMessageNativeDelegate(Chat.OnChatClientSideMessageNative);
            m_ChatClientSideMessageNative = Marshal.GetFunctionPointerForDelegate(m_ChatClientSideMessageNativeDelegate);
            EloBuddy.Native.EventHandler<42,bool __cdecl(char * *),char * *>.Add(EloBuddy.Native.EventHandler<42,bool __cdecl(char * *),char * *>.GetInstance(), m_ChatClientSideMessageNative.ToPointer());
            ChatSendWhisperHandlers = new List<ChatSendWhisper>();
            m_ChatSendWhisperNativeDelegate = new OnChatSendWhisperNativeDelegate(Chat.OnChatSendWhisperNative);
            m_ChatSendWhisperNative = Marshal.GetFunctionPointerForDelegate(m_ChatSendWhisperNativeDelegate);
            EloBuddy.Native.EventHandler<43,bool __cdecl(char * *,char * *),char * *,char * *>.Add(EloBuddy.Native.EventHandler<43,bool __cdecl(char * *,char * *),char * *,char * *>.GetInstance(), m_ChatSendWhisperNative.ToPointer());
        }

        public static unsafe void Close()
        {
            pwConsole* consolePtr = EloBuddy.Native.pwConsole.GetInstance();
            if (consolePtr != null)
            {
                EloBuddy.Native.pwConsole.Close(consolePtr);
            }
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<40,bool __cdecl(char * *),char * *>.Remove(EloBuddy.Native.EventHandler<40,bool __cdecl(char * *),char * *>.GetInstance(), m_ChatInputNative.ToPointer());
            EloBuddy.Native.EventHandler<41,bool __cdecl(EloBuddy::Native::AIHeroClient *,char * *),EloBuddy::Native::AIHeroClient *,char * *>.Remove(EloBuddy.Native.EventHandler<41,bool __cdecl(EloBuddy::Native::AIHeroClient *,char * *),EloBuddy::Native::AIHeroClient *,char * *>.GetInstance(), m_ChatMessageNative.ToPointer());
            EloBuddy.Native.EventHandler<42,bool __cdecl(char * *),char * *>.Remove(EloBuddy.Native.EventHandler<42,bool __cdecl(char * *),char * *>.GetInstance(), m_ChatClientSideMessageNative.ToPointer());
            EloBuddy.Native.EventHandler<43,bool __cdecl(char * *,char * *),char * *,char * *>.Remove(EloBuddy.Native.EventHandler<43,bool __cdecl(char * *,char * *),char * *,char * *>.GetInstance(), m_ChatSendWhisperNative.ToPointer());
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnChatClientSideMessageNative(sbyte modopt(IsSignUnspecifiedByte)** A_0)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                int num3 = *((int*) A_0);
                int num2 = num3;
                while (true)
                {
                    if (num2[0] == 0)
                    {
                        break;
                    }
                    num2++;
                }
                if ((num2 - num3) > 0)
                {
                    ChatClientSideMessageEventArgs args = new ChatClientSideMessageEventArgs(A_0);
                    foreach (ChatClientSideMessage message in ChatClientSideMessageHandlers.ToArray())
                    {
                        try
                        {
                            message(args);
                            flag = args.Process && flag;
                        }
                        catch (Exception exception4)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                            System.Console.WriteLine();
                            System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                            System.Console.WriteLine("Message: {0}", exception4.Message);
                            System.Console.WriteLine();
                            System.Console.WriteLine("Stracktrace:");
                            System.Console.WriteLine(exception4.StackTrace);
                            innerException = exception4.InnerException;
                            if (innerException != null)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("InnerException(s):");
                                do
                                {
                                    System.Console.WriteLine("----------------------------------------");
                                    System.Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                                    System.Console.WriteLine("Message: {0}", innerException.Message);
                                    System.Console.WriteLine();
                                    System.Console.WriteLine("Stracktrace:");
                                    System.Console.WriteLine(innerException.StackTrace);
                                    innerException = innerException.InnerException;
                                }
                                while (innerException != null);
                                System.Console.WriteLine("----------------------------------------");
                            }
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine();
                        }
                    }
                }
                return flag;
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnChatInputNative(sbyte modopt(IsSignUnspecifiedByte)** A_0)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                int num3 = *((int*) A_0);
                int num2 = num3;
                while (true)
                {
                    if (num2[0] == 0)
                    {
                        break;
                    }
                    num2++;
                }
                if ((num2 - num3) > 0)
                {
                    ChatInputEventArgs args = new ChatInputEventArgs(A_0);
                    foreach (ChatInput input in ChatInputHandlers.ToArray())
                    {
                        try
                        {
                            input(args);
                            flag = args.Process && flag;
                        }
                        catch (Exception exception4)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                            System.Console.WriteLine();
                            System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                            System.Console.WriteLine("Message: {0}", exception4.Message);
                            System.Console.WriteLine();
                            System.Console.WriteLine("Stracktrace:");
                            System.Console.WriteLine(exception4.StackTrace);
                            innerException = exception4.InnerException;
                            if (innerException != null)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("InnerException(s):");
                                do
                                {
                                    System.Console.WriteLine("----------------------------------------");
                                    System.Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                                    System.Console.WriteLine("Message: {0}", innerException.Message);
                                    System.Console.WriteLine();
                                    System.Console.WriteLine("Stracktrace:");
                                    System.Console.WriteLine(innerException.StackTrace);
                                    innerException = innerException.InnerException;
                                }
                                while (innerException != null);
                                System.Console.WriteLine("----------------------------------------");
                            }
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine();
                        }
                    }
                }
                return flag;
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnChatMessageNative(EloBuddy.Native.Obj_AI_Base* A_0, sbyte modopt(IsSignUnspecifiedByte)** A_1)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                int num3 = *((int*) A_1);
                int num2 = num3;
                while (true)
                {
                    if (num2[0] == 0)
                    {
                        break;
                    }
                    num2++;
                }
                if (((num2 - num3) > 0) && (A_0 != null))
                {
                    EloBuddy.AIHeroClient player = ObjectManager.Player;
                    ChatMessageEventArgs args = new ChatMessageEventArgs(player, A_1);
                    foreach (ChatMessage message in ChatMessageHandlers.ToArray())
                    {
                        try
                        {
                            message(player, args);
                            flag = args.Process && flag;
                        }
                        catch (Exception exception4)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                            System.Console.WriteLine();
                            System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                            System.Console.WriteLine("Message: {0}", exception4.Message);
                            System.Console.WriteLine();
                            System.Console.WriteLine("Stracktrace:");
                            System.Console.WriteLine(exception4.StackTrace);
                            innerException = exception4.InnerException;
                            if (innerException != null)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("InnerException(s):");
                                do
                                {
                                    System.Console.WriteLine("----------------------------------------");
                                    System.Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                                    System.Console.WriteLine("Message: {0}", innerException.Message);
                                    System.Console.WriteLine();
                                    System.Console.WriteLine("Stracktrace:");
                                    System.Console.WriteLine(innerException.StackTrace);
                                    innerException = innerException.InnerException;
                                }
                                while (innerException != null);
                                System.Console.WriteLine("----------------------------------------");
                            }
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine();
                        }
                    }
                }
                return flag;
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnChatSendWhisperNative(sbyte modopt(IsSignUnspecifiedByte)** A_0, sbyte modopt(IsSignUnspecifiedByte)** A_1)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                int num3 = *((int*) A_1);
                int num2 = num3;
                while (true)
                {
                    if (num2[0] == 0)
                    {
                        break;
                    }
                    num2++;
                }
                if ((num2 - num3) > 0)
                {
                    ChatWhisperEventArgs args = new ChatWhisperEventArgs(A_0, A_1);
                    foreach (ChatSendWhisper whisper in ChatSendWhisperHandlers.ToArray())
                    {
                        try
                        {
                            whisper(args);
                            flag = args.Process && flag;
                        }
                        catch (Exception exception4)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                            System.Console.WriteLine();
                            System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                            System.Console.WriteLine("Message: {0}", exception4.Message);
                            System.Console.WriteLine();
                            System.Console.WriteLine("Stracktrace:");
                            System.Console.WriteLine(exception4.StackTrace);
                            innerException = exception4.InnerException;
                            if (innerException != null)
                            {
                                System.Console.WriteLine();
                                System.Console.WriteLine("InnerException(s):");
                                do
                                {
                                    System.Console.WriteLine("----------------------------------------");
                                    System.Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                                    System.Console.WriteLine("Message: {0}", innerException.Message);
                                    System.Console.WriteLine();
                                    System.Console.WriteLine("Stracktrace:");
                                    System.Console.WriteLine(innerException.StackTrace);
                                    innerException = innerException.InnerException;
                                }
                                while (innerException != null);
                                System.Console.WriteLine("----------------------------------------");
                            }
                            System.Console.WriteLine("========================================");
                            System.Console.WriteLine();
                        }
                    }
                }
                return flag;
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
            return flag;
        }

        public static void Print(object @object)
        {
            Print(@object.ToString());
        }

        public static unsafe void Print(string text)
        {
            if (!EloBuddy.Native.Hacks.GetDisableDrawings())
            {
                IntPtr hglobal = Marshal.StringToHGlobalAnsi(text);
                pwConsole* consolePtr = EloBuddy.Native.pwConsole.GetInstance();
                if (consolePtr != null)
                {
                    EloBuddy.Native.pwConsole.ShowClientSideMessage(consolePtr, (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) hglobal.ToPointer());
                }
                Marshal.FreeHGlobal(hglobal);
            }
        }

        public static void Print(string text, Color color)
        {
            byte b = color.B;
            byte g = color.G;
            byte r = color.R;
            Print($"<font color='#{r.ToString("X2") + g.ToString("X2") + b.ToString("X2")}'>{text}</font>");
        }

        public static void Print(string format, params object[] @params)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(format, @params);
            Print(builder.ToString());
        }

        public static void Print(string format, Color color, params object[] @params)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(format, @params);
            byte b = color.B;
            byte g = color.G;
            byte r = color.R;
            Print($"<font color='#{r.ToString("X2") + g.ToString("X2") + b.ToString("X2")}'>{builder}</font>");
        }

        public static unsafe void Say(string text)
        {
            if (!EloBuddy.Native.Hacks.GetDisableDrawings())
            {
                IntPtr hglobal = Marshal.StringToHGlobalAnsi(text);
                pwConsole* consolePtr = EloBuddy.Native.pwConsole.GetInstance();
                EloBuddy.Native.MenuGUI* uguiPtr = EloBuddy.Native.MenuGUI.GetInstance();
                if ((consolePtr != null) && (uguiPtr != null))
                {
                    strcpy_s(EloBuddy.Native.MenuGUI.GetInput(uguiPtr), 0x400, (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) hglobal.ToPointer());
                    EloBuddy.Native.pwConsole.ProcessCommand(consolePtr);
                }
                Marshal.FreeHGlobal(hglobal);
            }
        }

        public static void Say(string format, params object[] @params)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(format, @params);
            Say(builder.ToString());
        }

        public static unsafe void Show()
        {
            pwConsole* consolePtr = EloBuddy.Native.pwConsole.GetInstance();
            if (consolePtr != null)
            {
                EloBuddy.Native.pwConsole.Show(consolePtr);
            }
        }

        public static bool IsClosed
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.MenuGUI* uguiPtr = EloBuddy.Native.MenuGUI.GetInstance();
                return ((uguiPtr != null) && ((bool) ((byte) (*(EloBuddy.Native.MenuGUI.GetIsChatOpen(uguiPtr)) == 0))));
            }
        }

        public static bool IsOpen
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.MenuGUI* uguiPtr = EloBuddy.Native.MenuGUI.GetInstance();
                if (uguiPtr != null)
                {
                    return *(EloBuddy.Native.MenuGUI.GetIsChatOpen(uguiPtr));
                }
                return false;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnChatClientSideMessageNativeDelegate(sbyte modopt(IsSignUnspecifiedByte)** A_0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnChatInputNativeDelegate(sbyte modopt(IsSignUnspecifiedByte)** A_0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnChatMessageNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, sbyte modopt(IsSignUnspecifiedByte)** A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnChatSendWhisperNativeDelegate(sbyte modopt(IsSignUnspecifiedByte)** A_0, sbyte modopt(IsSignUnspecifiedByte)** A_1);
    }
}

