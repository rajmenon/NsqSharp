﻿using System;

namespace NsqSharp.Core
{
    // https://github.com/bitly/go-nsq/blob/master/delegates.go#L50

    /// <summary>
    /// LogLevel specifies the severity of a given log message
    /// </summary>
    public enum LogLevel
    {
        /// <summary>Debug</summary>
        Debug = 0,
        /// <summary>Info</summary>
        Info = 1,
        /// <summary>Warning</summary>
        Warning = 2,
        /// <summary>Error</summary>
        Error = 3,
        /// <summary>Critical</summary>
        Critical = 4,
    }

    /// <summary>
    /// Logging constants
    /// </summary>
    internal static class Log
    {
        /// <summary>LogLevelDebugPrefix</summary>
        public const string DebugPrefix = "DBG";
        /// <summary>LogLevelInfoPrefix</summary>
        public const string InfoPrefix = "INF";
        /// <summary>LogLevelWarningPrefix</summary>
        public const string WarningPrefix = "WRN";
        /// <summary>LogLevelErrorPrefix</summary>
        public const string ErrorPrefix = "ERR";
        /// <summary>LogLevelCriticalPrefix</summary>
        public const string CriticalPrefix = "FAT";

        /// <summary>LogPrefix Resolution</summary>
        internal static string Prefix(LogLevel lvl)
        {
            string prefix = string.Empty;

            switch (lvl)
            {
                case LogLevel.Debug:
                    prefix = DebugPrefix;
                    break;
                case LogLevel.Info:
                    prefix = InfoPrefix;
                    break;
                case LogLevel.Warning:
                    prefix = WarningPrefix;
                    break;
                case LogLevel.Error:
                    prefix = ErrorPrefix;
                    break;
                case LogLevel.Critical:
                    prefix = CriticalPrefix;
                    break;
            }

            return prefix;
        }
    }

    /// <summary>
    /// MessageDelegate is an interface of methods that are used as
    /// callbacks in Message
    /// </summary>
    internal interface IMessageDelegate
    {
        /// <summary>
        /// OnFinish is called when the Finish() method
        /// is triggered on the Message
        /// </summary>
        void OnFinish(Message m);

        /// <summary>
        /// OnRequeue is called when the Requeue() method
        /// is triggered on the Message
        /// </summary>
        TimeSpan OnRequeue(Message m, TimeSpan? delay, bool backoff);

        /// <summary>
        /// OnTouch is called when the Touch() method
        /// is triggered on the Message
        /// </summary>
        void OnTouch(Message m);
    }

    internal class ConnMessageDelegate : IMessageDelegate
    {
        public Conn c { get; set; }

        public void OnFinish(Message m) { c.onMessageFinish(m); }
        public TimeSpan OnRequeue(Message m, TimeSpan? delay, bool backoff)
        {
            return c.onMessageRequeue(m, delay, backoff);
        }
        public void OnTouch(Message m) { c.onMessageTouch(m); }
    }

    /// <summary>
    /// ConnDelegate is an interface of methods that are used as
    /// callbacks in Conn
    /// </summary>
    public interface IConnDelegate
    {
        /// <summary>
        /// OnResponse is called when the connection
        /// receives a FrameTypeResponse from nsqd
        /// </summary>
        void OnResponse(Conn c, byte[] data);

        /// <summary>
        /// OnError is called when the connection
        /// receives a FrameTypeError from nsqd
        /// </summary>
        void OnError(Conn c, byte[] data);

        /// <summary>
        /// OnMessage is called when the connection
        /// receives a FrameTypeMessage from nsqd
        /// </summary>
        void OnMessage(Conn c, Message m);

        /// <summary>
        /// OnMessageFinished is called when the connection
        /// handles a FIN command from a message handler
        /// </summary>
        void OnMessageFinished(Conn c, Message m);

        /// <summary>
        /// OnMessageRequeued is called when the connection
        /// handles a REQ command from a message handler
        /// </summary>
        void OnMessageRequeued(Conn c, Message m);

        /// <summary>
        /// OnBackoff is called when the connection triggers a backoff state
        /// </summary>
        void OnBackoff(Conn c);

        /// <summary>
        /// OnResume is called when the connection triggers a resume state
        /// </summary>
        void OnResume(Conn c);

        /// <summary>
        /// OnIOError is called when the connection experiences
        /// a low-level TCP transport error
        /// </summary>
        void OnIOError(Conn c, Exception err);

        /// <summary>
        /// OnHeartbeat is called when the connection
        /// receives a heartbeat from nsqd
        /// </summary>
        void OnHeartbeat(Conn c);

        /// <summary>
        /// OnClose is called when the connection
        /// closes, after all cleanup
        /// </summary>
        void OnClose(Conn c);
    }
}
