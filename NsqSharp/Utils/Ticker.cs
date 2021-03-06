﻿using System;
using System.Threading;
using NsqSharp.Utils.Channels;

namespace NsqSharp.Utils
{
    /// <summary>
    /// A Ticker holds a channel that delivers `ticks' of a clock at intervals. http://golang.org/pkg/time/#Ticker
    /// </summary>
    public class Ticker
    {
        private readonly Chan<DateTime> _tickerChan = new Chan<DateTime>();
        private bool _stop;

        /// <summary>
        /// Initializes a new instance of the Ticker class.
        /// </summary>
        /// <param name="duration">The interval between ticks on the channel.</param>
        public Ticker(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("duration", "duration must be > 0");

            GoFunc.Run(() =>
            {
                while (!_stop)
                {
                    Thread.Sleep(duration);
                    if (!_stop)
                    {
                        _tickerChan.Send(DateTime.Now);
                    }
                }
            }, string.Format("Ticker started:{0} duration:{1}", DateTime.Now, duration));
        }

        /// <summary>
        /// Stop turns off a ticker. After Stop, no more ticks will be sent. Stop does not close the channel, to prevent a
        /// read from the channel succeeding incorrectly.
        /// </summary>
        public void Stop()
        {
            _stop = true;
        }

        /// <summary>
        /// The channel on which the ticks are delivered.
        /// </summary>
        public IReceiveOnlyChan<DateTime> C
        {
            get { return _tickerChan; }
        }
    }
}
