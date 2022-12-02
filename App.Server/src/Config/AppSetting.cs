namespace App.Server;

public class AppSetting {

    IConfiguration config;
    public AppSetting(IConfiguration config) {
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