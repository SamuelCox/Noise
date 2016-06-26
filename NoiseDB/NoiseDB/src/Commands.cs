namespace NoiseDB
{
    /// <summary>
    /// An enum that represents all commands in the system,
    /// if the command doesn't explicitly exist it will 
    /// be UNKNOWN. 
    /// </summary>
    public enum Commands
    {
        UNKNOWN = 0,
        GET = 1,
        SET = 2,
        DELETE = 3,
        SERVER_START = 4,
        SERVER_STOP = 5,
        SERVER_CONNECT = 6,
        SAVE = 7,
        LOAD = 8,
        SERVER_DISCONNECT = 9
        
    }
}
