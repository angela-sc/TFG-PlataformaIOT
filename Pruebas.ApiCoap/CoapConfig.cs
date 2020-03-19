using CoAP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas.ApiCoap
{
    public class CoapConfig : ICoapConfig
    {
        public string Version => throw new NotImplementedException();

        public int DefaultPort => throw new NotImplementedException();

        public int DefaultSecurePort => throw new NotImplementedException();

        public int HttpPort => throw new NotImplementedException();

        public int AckTimeout => throw new NotImplementedException();

        public double AckRandomFactor => throw new NotImplementedException();

        public double AckTimeoutScale => throw new NotImplementedException();

        public int MaxRetransmit => throw new NotImplementedException();

        public int MaxMessageSize => throw new NotImplementedException();

        public int DefaultBlockSize => throw new NotImplementedException();

        public int BlockwiseStatusLifetime => throw new NotImplementedException();

        public bool UseRandomIDStart => throw new NotImplementedException();

        public bool UseRandomTokenStart => throw new NotImplementedException();

        public long NotificationMaxAge => throw new NotImplementedException();

        public long NotificationCheckIntervalTime => throw new NotImplementedException();

        public int NotificationCheckIntervalCount => throw new NotImplementedException();

        public int NotificationReregistrationBackoff => throw new NotImplementedException();

        public string Deduplicator => throw new NotImplementedException();

        public int CropRotationPeriod => throw new NotImplementedException();

        public int ExchangeLifetime => throw new NotImplementedException();

        public long MarkAndSweepInterval => throw new NotImplementedException();

        public int ChannelReceiveBufferSize => throw new NotImplementedException();

        public int ChannelSendBufferSize => throw new NotImplementedException();

        public int ChannelReceivePacketSize => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load(string configFile)
        {
            throw new NotImplementedException();
        }

        public void Store(string configFile)
        {
            throw new NotImplementedException();
        }
    }
}
