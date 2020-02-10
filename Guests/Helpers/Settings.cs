using DotNetEnv;

namespace Guests.Helpers
{
    public class Settings{
        public string Secret{get;}
        public Settings(){
            DotNetEnv.Env.Load();
            Secret=DotNetEnv.Env.GetString("Secret");
        }
    }
}