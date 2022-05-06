using System.Security.Principal;

namespace FilmsStorage.Models.Login
{
    public class CustomPrincipal: ICustomPrincipal
    {
        public int UserID { get; set; }

        public IIdentity Identity { get; private set; }

        //Always false, because we dont use role system
        public bool IsInRole(string role)
        {
            return false;
        }

        public CustomPrincipal(string userName, int userID)
        {
            Identity = new GenericIdentity(userName);
            UserID = userID;
        }
    }
}