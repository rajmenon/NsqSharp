﻿using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace NsqSharp.Utils
{
    /// <summary>
    /// TlsConfig. Minimal implementation of http://golang.org/pkg/crypto/tls/#Config
    /// </summary>
    internal class TlsConfig
    {
        /// <summary>
        /// Initializes a new instance of the TlsConfig class.
        /// </summary>
        public TlsConfig()
        {
            MinVersion = SslProtocols.Ssl3;
#if NETFX_3_5 || NETFX_4_0
            MaxVersion = SslProtocols.Tls;
#else
            MaxVersion = SslProtocols.Tls12;
#endif
            Certificates = new X509Certificate2Collection();
            RootCAs = new X509Certificate2Collection();
        }

        /// <summary>Minimum TLS version (default = SSLv3).</summary>
        public SslProtocols MinVersion { get; set; }
        /// <summary>Maximum TLS version (default = TLS 1.2).</summary>
        public SslProtocols MaxVersion { get; set; }
        /// <summary>X.509 certificates.</summary>
        public X509Certificate2Collection Certificates { get; set; }
        /// <summary>X.509 certificates.</summary>
        public X509Certificate2Collection RootCAs { get; set; }

        /// <summary>
        /// InsecureSkipVerify controls whether a client verifies the
        /// server's certificate chain and host name.
        /// If InsecureSkipVerify is true, TLS accepts any certificate
        /// presented by the server and any host name in that certificate.
        /// In this mode, TLS is susceptible to man-in-the-middle attacks.
        /// This should be used only for testing.
        /// </summary>
        public bool InsecureSkipVerify { get; set; }
    }
}
