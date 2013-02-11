#Poller


Poller is a windows service that polls a source repository and writes a summary of the commits to something

##Default implementation


The included configuration file is configured to retrive changes from TFS and write the changes to Campfire

##Version
Unknown ?.?.?

##Configuration

    <StructureMap>
        <Assembly Name="Poller"/>
        <PluginFamily Type="Poller.Services.ISourceRepository" Assembly="Poller" DefaultKey="Default">
            <Plugin Type="Poller.Services.Tfs" Assembly="Poller" ConcreteKey="Default"/>
        </PluginFamily>
        <PluginFamily Type="Poller.Services.IChat" Assembly="Poller" DefaultKey="Default">
            <Plugin Type="Poller.Services.HipChat" Assembly="Poller" ConcreteKey="Default"/>
        </PluginFamily>
        <PluginFamily Type="Poller.Services.ILogger" Assembly="Poller" DefaultKey="Default">
            <Plugin Type="Poller.Services.EventLogger" Assembly="Poller" ConcreteKey="Default"/>
        </PluginFamily>
        <PluginFamily Type="Poller.Services.IMessageGenerator" Assembly="Poller" DefaultKey="Default">
            <Plugin Type="Poller.Services.MessageGenerator" Assembly="Poller" ConcreteKey="Default"/>
        </PluginFamily>
    </StructureMap>
    
    <appSettings>
        <add key="PollingInterval" value="10"/>
    </appSettings>

##Installation


1. Clone Repo
2. Build application
3. Install application as a windows service `installutil -i poller.exe`
4. Run the service as a user with permission to TFS.