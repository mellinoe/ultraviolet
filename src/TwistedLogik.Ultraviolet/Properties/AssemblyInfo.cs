﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]

#if PRODUCTION
[assembly: InternalsVisibleTo("TwistedLogik.Ultraviolet.Design, PublicKey=" +
    "00240000048000009400000006020000002400005253413100040000010001005dd0e010413925" +
    "79d63e81ea2cce6eeb67e8bde9256a39ba0ae06d5c96eef50905c7ee69ac28905ef5f2c9a8efce" +
    "6cc414dafe1ef66180873448e75c875dafa6d976c9642cc1dbf14ec53c97d81046059d7a17f0ed" +
    "30184ead039903031f7d8cbd02fa5021796e92dd810141ad3288ace425af60305ed8b090910d12" +
    "59a204da")]
#else
[assembly: InternalsVisibleTo("TwistedLogik.Ultraviolet.Design")]
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Ultraviolet Core Library")]
[assembly: AssemblyProduct("Ultraviolet Framework")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCompany("TwistedLogik Software")]
[assembly: AssemblyCopyright("Copyright © Cole Campbell 2014-2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type. Only Windows
// assemblies support COM.
[assembly: ComVisible(false)]

// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("d417c400-6ade-4f96-b45e-26bc7d685d36")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]
