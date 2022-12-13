namespace App.Server;

public class ServerSetting {

    IConfiguration config;
    public ServerSetting(IConfiguration config) {
        this.config = config;
    }

    public string Connectionstring { 
        get {
            return config["ConnectionStrings:Default"] ?? "";
        }
    }
    public string AppTitle { 
        get {
            return config["AppTitle"] ?? "Default Title";
        }
    }
}