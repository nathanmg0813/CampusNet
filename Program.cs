using CampusNet.Controllers;

namespace CampusNet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocialNetworkController controller = new SocialNetworkController();
            controller.Run();
        }
    }
}
