# Noise
A naive Key-Value store DBMS, built in C#.
## Getting Started
To use this DBMS, you can go to [samuelcox.me/Noise.html](http://samuelcox.me/Noise.html) and
download the version you need from there. There is a class library to operate with a Noise
server running on some machine, and a command line client/server.

Noise supports a few commands:

### Set Key Value
This stores a value against the key specified. If you are connected to a 
server, it will store this on the server. Otherwise it will store this
locally.
### Get Key
This attempts to get a value stored against the key supplied. If you
are connected to a server, it will look at the server's datastore.
Otherwise it will look locally.

### Delete Key
This attempts to delete a key-value relationship,
deleting both the value stored against the key specified,
and the key from the data-store.

### Server_Connect Key
This connects to a Noise server running on the hostname
specified by the key.

### Server_Disconnect
This disconnects from any server you are connected to.

### Server_Start
This starts a Noise server running on the machine
the command line client.server is running from.

### Server_Stop
If the server is running, then this stops the server.

### Save key
This attempts to persist the Key-Value store
to disk, with filename of the key specified.

### Load key
This attempts to read a Key-Value store from
disk, with filename of the key specified.

## App.Config/ Web.Config switches needed.
There are a few app.config/web.config switches that are needed
both client side and server side for this program to run.

### DataStoreFilePath
This is where the Key-Value data store will be saved to,
and read from. 

### LoggingDirectory
The directory to write a historical log of Queries executed to.

### LoggingEnabled
Whether or not to turn historical logging of Queries on.

### UseTls
Whether or not to use secure, Tls 1.2 encrypted connections
when connecting to a Noise server.

### ServerTlsCertificateFilePath
This is the filepath to the Tls certificate 
the server will use for authentication.

### ClientTlsCertificateFilePath
This is the filepath to the Tls certificate the client
will use for authentication.

### ByteArraySize
This is the size of Byte Arrays to send queries over the network.
This highly impacts performance, but if you need to store a larger
amount of data and you are having issues, try increasing this on both
the client and server.