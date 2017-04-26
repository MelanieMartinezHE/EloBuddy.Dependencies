namespace EloBuddy
{
    using std;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class AudioManager
    {
        internal static List<AudioManagerPlaySound> AudioManagerPlaySoundHandlers;
        internal static IntPtr m_AudioManagerPlaySoundNative = new IntPtr();
        internal static OnAudioManagerPlaySoundNativeDelegate m_AudioManagerPlaySoundNativeDelegate;

        public static  event AudioManagerPlaySound OnPlaySound
        {
            add
            {
                AudioManagerPlaySoundHandlers.Add(handler);
            }
            remove
            {
                AudioManagerPlaySoundHandlers.Remove(handler);
            }
        }

        static AudioManager()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(AudioManager.DomainUnloadEventHandler);
            AudioManagerPlaySoundHandlers = new List<AudioManagerPlaySound>();
            m_AudioManagerPlaySoundNativeDelegate = new OnAudioManagerPlaySoundNativeDelegate(AudioManager.OnAudioManagerPlaySoundNative);
            m_AudioManagerPlaySoundNative = Marshal.GetFunctionPointerForDelegate(m_AudioManagerPlaySoundNativeDelegate);
            EloBuddy.Native.EventHandler<50,void __cdecl(std::basic_string<char,std::char_traits<char>,std::allocator<char> >),std::basic_string<char,std::char_traits<char>,std::allocator<char> > >.Add(EloBuddy.Native.EventHandler<50,void __cdecl(std::basic_string<char,std::char_traits<char>,std::allocator<char> >),std::basic_string<char,std::char_traits<char>,std::allocator<char> > >.GetInstance(), m_AudioManagerPlaySoundNative.ToPointer());
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<50,void __cdecl(std::basic_string<char,std::char_traits<char>,std::allocator<char> >),std::basic_string<char,std::char_traits<char>,std::allocator<char> > >.Remove(EloBuddy.Native.EventHandler<50,void __cdecl(std::basic_string<char,std::char_traits<char>,std::allocator<char> >),std::basic_string<char,std::char_traits<char>,std::allocator<char> > >.GetInstance(), m_AudioManagerPlaySoundNative.ToPointer());
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnAudioManagerPlaySoundNative(basic_string<char,std::char_traits<char>,std::allocator<char> > modreq(IsCopyConstructed)* soundFile)
        {
            Exception innerException = null;
            bool flag;
            try
            {
                flag = true;
                try
                {
                    AudioManagerPlaySoundEventArgs args = new AudioManagerPlaySoundEventArgs(new string((0x10 > *(((int*) (soundFile + 20)))) ? ((sbyte*) soundFile) : ((sbyte*) *(((int*) soundFile)))));
                    foreach (AudioManagerPlaySound sound in AudioManagerPlaySoundHandlers.ToArray())
                    {
                        try
                        {
                            sound(args);
                            flag = args.Process && flag;
                        }
                        catch (Exception exception4)
                        {
                            Console.WriteLine();
                            Console.WriteLine("========================================");
                            Console.WriteLine("Exception occured! EloBuddy might crash!");
                            Console.WriteLine();
                            Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                            Console.WriteLine("Message: {0}", exception4.Message);
                            Console.WriteLine();
                            Console.WriteLine("Stracktrace:");
                            Console.WriteLine(exception4.StackTrace);
                            innerException = exception4.InnerException;
                            if (innerException != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("InnerException(s):");
                                do
                                {
                                    Console.WriteLine("----------------------------------------");
                                    Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                                    Console.WriteLine("Message: {0}", innerException.Message);
                                    Console.WriteLine();
                                    Console.WriteLine("Stracktrace:");
                                    Console.WriteLine(innerException.StackTrace);
                                    innerException = innerException.InnerException;
                                }
                                while (innerException != null);
                                Console.WriteLine("----------------------------------------");
                            }
                            Console.WriteLine("========================================");
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception exception3)
                {
                    Console.WriteLine();
                    Console.WriteLine("========================================");
                    Console.WriteLine("Exception occured! EloBuddy might crash!");
                    Console.WriteLine();
                    Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                    Console.WriteLine("Message: {0}", exception3.Message);
                    Console.WriteLine();
                    Console.WriteLine("Stracktrace:");
                    Console.WriteLine(exception3.StackTrace);
                    Exception exception2 = exception3.InnerException;
                    if (exception2 != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("InnerException(s):");
                        do
                        {
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                            Console.WriteLine("Message: {0}", exception2.Message);
                            Console.WriteLine();
                            Console.WriteLine("Stracktrace:");
                            Console.WriteLine(exception2.StackTrace);
                            exception2 = exception2.InnerException;
                        }
                        while (exception2 != null);
                        Console.WriteLine("----------------------------------------");
                    }
                    Console.WriteLine("========================================");
                    Console.WriteLine();
                }
            }
            fault
            {
                ___CxxCallUnwindDtor(std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{dtor}, soundFile);
            }
            std.basic_string<char,std::char_traits<char>,std::allocator<char> >._Tidy(soundFile, true, 0);
            return flag;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnAudioManagerPlaySoundNativeDelegate(basic_string<char,std::char_traits<char>,std::allocator<char> > modreq(IsCopyConstructed)* soundFile);
    }
}

