using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Caching;
using WendtEquipmentTracking.Common.DTO;

namespace WendtEquipmentTracking.Common
{
    public static class ActiveDirectoryHelper
    {
        private const string DOMAIN_NAME = "WendtCorp";

        public static string CurrentUserUsername()
        {
            var fullUsername = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            var usernameParts = fullUsername.Split('\\');

            var username = string.Empty;
            if (usernameParts.Length == 2)
            {
                username = usernameParts[1];
            }

            return username;
        }

        public static string DisplayName(string username)
        {
            var user = GetUser(username);

            return user.DisplayName;
        }

        public static ActiveDirectoryUser GetUser(string username)
        {

            ActiveDirectoryUser user = HttpContext.Current.Cache["currentUser" + username] as ActiveDirectoryUser;

            if (user == null)
            {

                if (!string.IsNullOrWhiteSpace(username))
                {
                    try
                    {
                        var principalContext = new PrincipalContext(ContextType.Domain, DOMAIN_NAME);

                        UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
                            principalContext,
                            IdentityType.SamAccountName,
                            DOMAIN_NAME + "\\" + username);

                        user = mapToUser(userPrincipal);

                        HttpContext.Current.Cache.Add("currentUser" + username, user, null, DateTime.Now.AddDays(2), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                    catch
                    {
                        user = new ActiveDirectoryUser();
                        HttpContext.Current.Cache.Add("currentUser" + username, user, null, DateTime.Now.AddDays(2), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

                    }
                }
            }

            //Comment this in and out depending on testing locally or deploying
            user.Role = UserRoles.ReadWrite;
            user.Email = "keith.abramo@gmail.com";

            return user;
        }

        public static string GetUserInfo(string username)
        {

            var principalContext = new PrincipalContext(ContextType.Domain, DOMAIN_NAME);

            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
                principalContext,
                IdentityType.SamAccountName,
                DOMAIN_NAME + "\\" + username);

            PrincipalSearchResult<Principal> groups = userPrincipal.GetAuthorizationGroups();


            var result = string.Empty;
            // iterate over all groups
            foreach (Principal p in groups)
            {
                // make sure to add only group principals
                if (p is GroupPrincipal)
                {
                    result += " : " + (((GroupPrincipal)p).Name);
                }
            }


            return result;


        }

        //public static IEnumerable<ActiveDirectoryUser> ActiveDirectoryUsers(string searchString)
        //{
        //    IList<ActiveDirectoryUser> users = new List<ActiveDirectoryUser>();
        //    string userName = ActiveDirectoryHelper.CurrentUserUsername();

        //    try
        //    {
        //        var principalContext = new PrincipalContext(ContextType.Domain, DOMAIN_NAME, "DC=corp,DC=ene,DC=com");

        //        //Get the current users's principal
        //        UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
        //            principalContext,
        //            IdentityType.SamAccountName,
        //            DOMAIN_NAME + "\\" + userName);

        //        // if user is found
        //        if (userPrincipal != null)
        //        {

        //            //Query by example display name and username (aka Name)
        //            UserPrincipal qbeDisplayNamePrincipal = new UserPrincipal(principalContext);
        //            qbeDisplayNamePrincipal.DisplayName = "*" + searchString + "*";
        //            UserPrincipal qbeUsernamePrincipal = new UserPrincipal(principalContext);
        //            qbeUsernamePrincipal.SamAccountName = "*" + searchString + "*";

        //            //Prepare
        //            PrincipalSearcher displayNameSearcher = new PrincipalSearcher(qbeDisplayNamePrincipal);
        //            PrincipalSearcher usernameSearcher = new PrincipalSearcher(qbeUsernamePrincipal);

        //            PrincipalSearchResult<Principal> foundFullNameSearcher = null;
        //            if (searchString.Contains(" "))
        //            {
        //                var searchStrings = searchString.Split(' ');

        //                if (!string.IsNullOrWhiteSpace(searchStrings[0]) || !string.IsNullOrWhiteSpace(searchStrings[1]))
        //                {
        //                    UserPrincipal qbeFullNamePrincipal = new UserPrincipal(principalContext);

        //                    if (!string.IsNullOrWhiteSpace(searchStrings[0]))
        //                    {
        //                        qbeFullNamePrincipal.GivenName = "*" + searchStrings[0] + "*";
        //                    }
        //                    if (!string.IsNullOrWhiteSpace(searchStrings[1]))
        //                    {
        //                        qbeFullNamePrincipal.Surname = "*" + searchStrings[1] + "*";
        //                    }

        //                    PrincipalSearcher fullNameSearcher = new PrincipalSearcher(qbeFullNamePrincipal);

        //                    foundFullNameSearcher = fullNameSearcher.FindAll();
        //                }
        //            }

        //            //Execute
        //            var foundDisplayNameUsers = displayNameSearcher.FindAll();
        //            var foundUsernameUsers = usernameSearcher.FindAll();

        //            //Union
        //            var foundUsers = foundUsernameUsers.Union(foundDisplayNameUsers);
        //            if (foundFullNameSearcher != null)
        //            {
        //                foundUsers = foundUsers.Union(foundFullNameSearcher);
        //            }

        //            //Iterate over users found
        //            foreach (UserPrincipal foundUser in foundUsers.ToList())
        //            {
        //                users.Add(mapToUser(foundUser));
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    return users
        //        .GroupBy(u => u.Username)
        //        .Select(g => g.First())
        //        .ToList();
        //}

        private static ActiveDirectoryUser mapToUser(UserPrincipal userPrincipal)
        {
            var user = new ActiveDirectoryUser();

            if (userPrincipal != null)
            {
                var role = UserRoles.None;
                if (userPrincipal.GetAuthorizationGroups().Any(ag => ag.Name == "MSL_RW"))
                {
                    role = UserRoles.ReadWrite;
                }
                else if (userPrincipal.GetAuthorizationGroups().Any(ag => ag.Name == "MSL_R"))
                {
                    role = UserRoles.ReadOnly;
                }


                user = new ActiveDirectoryUser
                {
                    Username = userPrincipal.SamAccountName,
                    FirstName = userPrincipal.GivenName,
                    MiddleName = userPrincipal.MiddleName,
                    LastName = userPrincipal.Surname,
                    Email = userPrincipal.EmailAddress,
                    Role = role
                };
            }
            return user;
        }
    }
}