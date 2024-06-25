using System.Collections.Generic;

namespace FirstSnow.Common
{    
    public class CommonOptions
    {
        public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
        public ErrorNotifyOptions ErrorNotifyOptions { get; set; } = new ErrorNotifyOptions();
        public AuthOptions AuthOptions { get; set; } = new AuthOptions();
    }
}