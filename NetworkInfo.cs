﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;

namespace Test.Net
{
    static internal class NetworkInfo
    {
        // basic option to get DNS serves via NetworkInfo. We may get it directly later via proper APIs. 
        public static ResolverOptions GetOptions()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            Collection<IPAddress> servers = new Collection<IPAddress>();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties properties = nic.GetIPProperties();
                // avoid loopback, VPN etc. Should be re-visited.

                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (IPAddress server in properties.DnsAddresses)
                    {
                        if (!servers.Contains(server))
                        {
                            servers.Add(server);
                        }
                    }
                }
            }

            return new ResolverOptions(servers!.ToArray<IPAddress>());
        }
    }
}
