# Mission Control

This is the repository for the KSP Mission Control plugin. Here is where the code for the plugin is written. The plugin handles ship telemetry and data transmission.

Further uses of *MC* in this document refer to *Mission Control*.

## Limitations

Due to the availability of HttpListener, this plugin is only compatible with Windows computers.

## Network Transmission

** == BEFORE INSTALLATION, PLEASE READ THE FOLLOWING == **

Squad (and by extension, Private Divison) requires all mods to reveal what network transmission is being used for. The team behind Mission Control respectfully agrees, due to the security implications behind opening a port on your machine.

You have to give your KSP executable rights to pass through your firewall when asked (and we suggest that you only allow it on private networks.)

```
Add-ons that contact another network or computer system must tell users exactly what it's sending or receiving in a clear and obvious way in all locations it is offered for download.
```

- Frybert (January 12, 2017) - [KSP Forums](https://forum.kerbalspaceprogram.com/index.php?/topic/154851-add-on-posting-rules-november-24-2017/)

Therefore, on this page, and before the mod is first activated, we must inform the user of what the network abilities of the plugin are used for.

By default, MC will be using the loopback address (`127.0.0.1`) for communication. Unless your copy of MC has been modified, THIS CANNOT BE CHANGED. If you wish to broadcast your telemetry data over to your local network or perhaps to the internet, you must enable this functionality on the Mission Control GUI (to be added).

MC uses the port `21818` for transmission. You can access its network endpoints with a browser (e.g. `http://127.0.0.1:21818`) due to how the interface is set up.

## Network Transmission (for developers)

The plugin opens an HttpListener at `127.0.0.1:21818` and listens on specific endpoints.